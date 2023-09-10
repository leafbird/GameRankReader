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

        ReadGooglePlayRanking(driver);
    }

    private static void ReadGooglePlayRanking(IWebDriver driver)
    {
        // 웹 페이지로 이동
        var targetUrl = "https://play.google.com/store/games";
        driver.Navigate().GoToUrl(targetUrl);

        // Shadow DOM 내부 요소에 액세스하는 방법 (예시)
        var selector = "#yDmH0d > c-wiz.SSPGKf.glB9Ve > div > div > div.N4FjMb.Z97G4e > c-wiz > div > c-wiz > c-wiz:nth-child(2) > c-wiz > section > div > div:nth-child(3) > div > div > div";
        IWebElement element = driver.FindElement(By.CssSelector(selector));
        //IWebElement shadowHost = driver.FindElement(By.XPath("//*[@id='yDmH0d']/c-wiz[2]/div/div/div[1]/c-wiz/div/c-wiz/c-wiz[2]/c-wiz/section/div/div[3]/div/div/div/div[1]/div[1]/div[1]/div/a/div[1]/div")

        // 크롤링 작업 수행 (예시)
        string rankText = element.Text;

        // 결과 출력
        Console.WriteLine(rankText);
    }

    private static void ReadMobileIndexRanking(IWebDriver driver)
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
