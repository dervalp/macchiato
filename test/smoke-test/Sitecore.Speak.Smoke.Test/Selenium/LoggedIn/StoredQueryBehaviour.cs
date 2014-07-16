using System;

using OpenQA.Selenium;
using Should;
using Xunit;

namespace Sitecore.Speak.Smoke.Test.Selenium.LoggedIn
{
  public class StoredQueryBehaviour : LoggedInTests
  {
    public StoredQueryBehaviour()
    {
      Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
      Driver.Navigate().GoToUrl(SiteBaseUrl + "sitecore/client/your%20apps/StoredQueryTest");
      System.Threading.Thread.Sleep(7 * 1000);
    }

    [Fact]
    public void Query_loads_data_from_query_content_item()
    {
      Driver.PageSource.ShouldContain("client");
    }

    [Fact]
    public void Page_size_7_loads_7_items()
    {
      var items = Driver.FindElements(By.CssSelector("table.sc-table.table tbody tr"));
      items.Count.ShouldEqual(7, string.Format("Expecting 7 got {0} item(s)\n\nPage Source:\n\n{1}", items.Count, Driver.PageSource));
    }

    [Fact]
    public void Standard_template_field_loaded()
    {
      var dataCells = Driver.FindElements(By.CssSelector("table.sc-table.table tbody tr:first-of-type td"));
      dataCells.Count.ShouldEqual(2);

      foreach (var cell in dataCells)
      {
        cell.Text.Length.ShouldBeGreaterThan(0);
      }
    }
  }
}