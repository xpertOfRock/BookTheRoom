using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace Tests.ControllersTests
{
    [Binding]
    public class AccoutControllerTests
    {
        private IWebDriver _driver;

        [Given("User is on auth page")]
        public void Given_КористувачIsInAuthPage()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://localhost:3000/login");
        }

        [When("User enters valid password and email / nickname")]
        public void When_ValidPasswordAndEmailOrNickname()
        {
            _driver.FindElement(By.Id("emailOrUsername")).SendKeys("validUser");
            _driver.FindElement(By.Id("password")).SendKeys("validPassword");
        }

        [When("User presses button \"Login\"")]
        public void When_PressKeyEnter()
        {
            _driver.FindElement(By.Id("login-button")).Click();
        }

        [Then("User is redirected to home page")]
        public void Then_UserRedirectsToHomePage()
        {
            Assert.Equal("https://localhost:3000/", _driver.Url); 
            _driver.Quit();
        }
    }
}
