using System;
using System.Linq;

using OpenQA.Selenium;
using Should;
using Xunit;

namespace Macchiato.Smoke.Test.Selenium.AnonymousVisitor
{
  public class ItemServiceTestBehaviour : SeleniumBrowserTests
  {
    public ItemServiceTestBehaviour()
    {
      Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
    }

    [Fact]
    public void ItemService_passes()
    {
      // Dev. Note:   need to load the item service test page over HTTPS to avoid auth/login 
      //              CORS failure - ref https://trello.com/c/NNjhy1xh
      var urlBuilder = new UriBuilder(SiteBaseUrl + "test/itemservice") { Port = 443, Scheme = "https" };

      Driver.Navigate().GoToUrl(urlBuilder.Uri);

      System.Threading.Thread.Sleep(10 * 1000);

      Driver.FindElements(By.TagName("h1"))
            .First()
            .Text.ShouldEqual("Given an Item Controller in the server side");

      var fail = Driver.FindElements(By.CssSelector("li[class='test fail']"));
      fail.Count.ShouldEqual(0);
    }
  }
}