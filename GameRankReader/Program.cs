using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace GameRankReader;

class Program
{
    static void Main(string[] args)
    {
        ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
        chromeDriverService.HideCommandPromptWindow = true;

        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--disable-infobars");
        IWebDriver driver = new ChromeDriver(chromeDriverService, chromeOptions);

        ReadSimilarWeb(driver);
    }
    private static void ReadSimilarWeb(IWebDriver driver)
    {
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
            
            Console.WriteLine($"{titleText} ({companyText})");
        }
   }


    private static void ReadSensorTower(IWebDriver driver)
    {
        var targetDateStr = DateTime.Now.ToString("yyyy-MM-dd");
        var targetUrl = $"https://app.sensortower.com/top-charts?category=game&country=KR&date={targetDateStr}&device=iphone&os=android";
        driver.Navigate().GoToUrl(targetUrl);

        // Shadow DOM 내부 요소에 액세스하는 방법 (예시)
        var selector = "#mainContent > div.MuiBox-root.css-i9gxme > div > div.infinite-scroll-component__outerdiv > div > div.MuiTableContainer-root.css-kge0eu > table > tbody";
        IWebElement element = driver.FindElement(By.CssSelector(selector));

        // 크롤링 작업 수행 (예시)
        string rankText = element.Text;

        // 결과 출력
        Console.WriteLine(rankText);
    }

    private static void ReadMobileIndex(IWebDriver driver)
    {
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
            string textInsideShadowDOM = jsExecutor.ExecuteScript(script).ToString();
            Console.WriteLine(textInsideShadowDOM);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            driver.Close();
            driver.Quit();
        }
    }
}
