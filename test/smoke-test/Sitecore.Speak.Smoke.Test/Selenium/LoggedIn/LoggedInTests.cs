using OpenQA.Selenium;

namespace Sitecore.Speak.Smoke.Test.Selenium.LoggedIn
{
  public abstract class LoggedInTests : SeleniumBrowserTests
  {
    protected LoggedInTests()
    {
      Driver.Navigate().GoToUrl(SiteBaseUrl + "sitecore/login");
      Driver.FindElement(By.Id("Login_UserName")).Clear();
      Driver.FindElement(By.Id("Login_UserName")).SendKeys("admin");
      Driver.FindElement(By.Id("Login_Password")).Clear();
      Driver.FindElement(By.Id("Login_Password")).SendKeys("b");
      Driver.FindElement(By.Id("Login_Login")).Click();
    }
  }
}