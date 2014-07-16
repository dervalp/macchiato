using System;
using System.Linq;
using OpenQA.Selenium;
using Should;
using Xunit;

namespace Sitecore.Speak.Smoke.Test.Selenium.AnonymousVisitor
{
  public class DataTestBehaviour : SeleniumBrowserTests
  {
    public DataTestBehaviour()
    {
      Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));       
    }

    [Fact]
    public void DataTest_passes()
    {
      Driver.Navigate().GoToUrl(SiteBaseUrl + "test/datatest");
      
      System.Threading.Thread.Sleep(10 * 1000);

      Driver.FindElements(By.TagName("h1"))
            .First()
            .Text.ShouldEqual("Given a Product Controller in the server side");

      var fail = Driver.FindElements(By.CssSelector("li[class='test fail']"));
      fail.Count.ShouldEqual(0);
    }
  }
}