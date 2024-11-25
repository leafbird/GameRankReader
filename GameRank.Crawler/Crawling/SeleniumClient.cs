﻿namespace GameRank.Crawler.Crawling;

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Cs.Logging;
using GameRank.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

internal sealed class SeleniumClient
{
    public bool GetRankingData([MaybeNullWhen(false)] out DailyRankData result)
    {
        ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
        chromeDriverService.HideCommandPromptWindow = true;

        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--disable-infobars"); // 브라우저 창 상단에 표시되는 정보 표시줄을 숨기는 옵션
        // chromeOptions.AddArgument("--headless"); // 브라우저를 보이지 않게 설정 (headless 모드)
        IWebDriver driver = new ChromeDriver(chromeDriverService, chromeOptions);

        try 
        {
            result = ReadSensorTower(driver);
        }
        catch (Exception e)
        {
            Log.Debug(e.Message);
            result = default;
            return false;
        }
        finally
        {
            driver.Close();
            driver.Quit();
        }

        return true;
    }

    //// ---------------------------------------------------------------------------------------------

    private static DailyRankData ReadSensorTower(IWebDriver driver)
    {
        // 보안 요소는 없다. 다만 렌더링 width가 좁으면 dom 구조가 바뀐다.
        driver.Manage().Window.Maximize(); // 창 크기 최대화

        var current = DateTime.Now;
        var targetDateStr = current.ToString("yyyy-MM-dd");
        var targetUrl = $"https://app.sensortower.com/top-charts?category=game&country=KR&date={targetDateStr}&device=iphone&os=android";
        driver.Navigate().GoToUrl(targetUrl);
        
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        var selector = "#mainContent > div.MuiBox-root.css-i9gxme > div > div.infinite-scroll-component__outerdiv > div > div.MuiTableContainer-root.css-1p6ntod > table > tbody";
        var element = wait.Until(ExpectedConditions.ElementExists(By.CssSelector(selector)));

        var result = new DailyRankData
        {
            Date = current,
            Source = new Uri(targetUrl),
        };
        
        var rows = element.FindElements(By.CssSelector("tr"));
        while (rows.Count == 0)
        {
            Log.Debug("waiting for data...");
            Thread.Sleep(1000);
            rows = element.FindElements(By.CssSelector("tr"));
        }

        foreach (var child in rows)
        {
            // find ranking
            var subElement = child.FindElement(By.XPath("td[1]"));
            var rankingText = subElement.Text;
            
            // find summary page url
            selector = "td[4]/span/div/div/div[1]/a";
            subElement = child.FindElement(By.XPath(selector));
            var summaryUrl = subElement.GetAttribute("href");

            // find image url
            selector = "td[4]/span/div/div/div[1]/a/span/img";
            subElement = child.FindElement(By.XPath(selector));
            var imgUrl = subElement.GetAttribute("src");

            // find game title
            selector = "td[4]/span/div/div/div[1]/div/div[1]/a/span";
            subElement = child.FindElement(By.XPath(selector));
            var gameTitle = subElement.Text;

            // find publisher
            selector = "td[4]/span/div/div/div[1]/div/div[2]/a/span";
            subElement = child.FindElement(By.XPath(selector));
            var publisherText = subElement.Text;
            
            // find star grade
            selector = "td[4]/span/div/div/div[2]/span";
            subElement = child.FindElement(By.XPath(selector));
            var buffer = subElement.GetAttribute("aria-label");
            float starGrade = 0f;

            Match match = Regex.Match(buffer, @"\d+\.\d+");
            if (match.Success == false)
            {
                Log.Debug($"star grade parsing error. {buffer}");
            }
            else
            {
                starGrade = float.Parse(match.Value);
            }

            // find category page url
            selector = "td[4]/span/div/div/div[3]/a";
            subElement = child.FindElement(By.XPath(selector));
            var categoryUrl = subElement.GetAttribute("href");
            
            // find category text
            selector = "td[4]/span/div/div/div[3]/a/span";
            subElement = child.FindElement(By.XPath(selector));
            var categoryText = subElement.Text;

            Log.Debug($"gathring data... ranking:{rankingText} {gameTitle}");
            
            result.Ranks.Add(new SingleRankData
            {
                Ranking = int.Parse(rankingText),
                Title = gameTitle,
                Publisher = publisherText,
                ImageUrl = imgUrl,
                SummaryPageUrl = summaryUrl,
                StarGrade = starGrade,
                CategoryPageUrl = categoryUrl,
                CategoryText = categoryText,
            });
        }

        if (result.Ranks.Any() == false)
        {
            throw new Exception("No data found.");
        }
        
        result.Ranks.Sort();
        return result;
    }

    private static void ReadSimilarWeb(IWebDriver driver)
    {
        // 별다른 보안 요소 없이 바로 접근할 수 있지만, 랭킹 데이터의 갱신이 3~4일 차이날 만큼 느리다.
        var targetUrl = $"https://www.similarweb.com/apps/top/google/store-rank/kr/games/top-grossing/";
        driver.Navigate().GoToUrl(targetUrl);

        var selector = "body > div.wrapperBody--topRanking.wrapper-body.js-wrapperBody > main > div > div.topAppsSection.js-leaderBoard.js-appsSection > div.topAppsGrid > div > table > tbody";
        IWebElement element = driver.FindElement(By.CssSelector(selector));

        foreach (var child in element.FindElements(By.CssSelector("tr")))
        {
            // find image url
            var imgElement = child.FindElement(By.XPath("td[2]/div/a/img"));
            var imgUrl = imgElement.GetAttribute("src");

            // find game title
            var titleElement = child.FindElement(By.XPath("td[3]/div/a[1]/span"));
            var titleText = titleElement.Text;

            // find game title
            var companyElement = child.FindElement(By.XPath("td[4]/span"));
            var companyText = companyElement.Text;

            Log.Debug($"{titleText} ({companyText})");
        }
    }

    private static void ReadMobileIndex(IWebDriver driver)
    {
        // note: not working :(
        // need to salve 'shadow dom' problem...
        // 웹 페이지로 이동
        var targetUrl = "https://www.mobileindex.com/mi-chart/realtime-rank";
        driver.Navigate().GoToUrl(targetUrl);

        // 페이지 내용 크롤링을 위해 대기
        var waitingTargetXpath = "//*[@id=\"page-scroll-obj\"]/section/div";

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        IWebElement dynamicElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(waitingTargetXpath)));

        // Shadow DOM 내부 요소에 액세스하기 위한 JavaScript 코드
        string script = @"
            const shadowHost = document.querySelector('#page-scroll-obj > section > div > div:nth-child(4)');
            const shadowRoot = shadowHost.shadowRoot;
            const elementInsideShadowDOM = shadowRoot.querySelector('table');
            return elementInsideShadowDOM.textContent;
        ";

        try
        {
            // JavaScript를 실행하여 데이터 추출
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            var textInsideShadowDOM = jsExecutor.ExecuteScript(script)?.ToString();
            // Log.Debug(textInsideShadowDOM);
        }
        catch (Exception e)
        {
            Log.Debug(e.Message);
        }
        finally
        {
            driver.Close();
            driver.Quit();
        }
    }
}
