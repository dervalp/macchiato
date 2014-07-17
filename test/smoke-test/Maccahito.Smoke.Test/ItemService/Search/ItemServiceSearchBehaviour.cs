using System;
using System.Linq;
using System.Net;
using Should;
using Xunit;
using Sitecore.Services.Core.Model;

namespace Macchiato.Smoke.Test.ItemService.Search
{
  public class ItemServiceSearchBehaviour : ItemServiceTest
  {
    [Fact]
    public void Search_returns_OK()
    {
      var url = string.Format("{0}/search?term={1}", BaseUrl, "sitecore");

      Request.Execute(url, null, "GET");

      Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    [Fact]
    public void Search_returns_item_array()
    {
      var url = string.Format("{0}/search?term={1}", BaseUrl, "Home");

      var response = (ItemResults)Request.Execute<ItemResults>(url, null, "GET");

      response.Results.ShouldNotBeNull();
    }

    [Fact]
    public void Search_returns_different_results_for_master_and_core_database()
    {
      var url = string.Format("{0}/search?term={1}", BaseUrl, "Home");

      var masterResponse = (ItemResults)Request.Execute<ItemResults>(url + "&database=master", null, "GET");
      var coreResponse = (ItemResults)Request.Execute<ItemResults>(url + "&database=core", null, "GET");

      masterResponse.Results.ShouldNotEqual(coreResponse.Results);
    }

    [Fact]
    public void Search_can_sort_results_from_database_in_descending_order()
    {
      var url = string.Format("{0}/search?term={1}&sorting={2}&database=master&fields=TemplateName&pageSize=100", BaseUrl, "sitecore", "dTemplateName");

      var itemResults = (ItemResults)Request.Execute<ItemResults>(url, null, "GET");

      CompareResults(itemResults, ItemModel.TemplateName, i => i.ShouldBeGreaterThanOrEqualTo(0));
    }

    [Fact]
    public void Search_can_sort_results_from_database_in_ascending_order()
    {
      var url = string.Format("{0}/search?term={1}&sorting={2}&database=master&fields=TemplateName&pageSize=100", BaseUrl, "sitecore", "aTemplateName");

      var itemResults = (ItemResults)Request.Execute<ItemResults>(url, null, "GET");

      CompareResults(itemResults, ItemModel.TemplateName, i => i.ShouldBeLessThanOrEqualTo(0));
    }

    [Fact]
    public void Search_results_prev_next_links_contain_sorting_parameter()
    {
      var url = string.Format("{0}/search?term={1}&sorting={2}&database=master", BaseUrl, "sitecore", "aItemName");

      var itemResults = (ItemResults)Request.Execute<ItemResults>(url, null, "GET");

      itemResults.Links[0].Href.Contains("sorting=").ShouldBeTrue();
    }

    [Fact]
    public void Search_rejects_invalid_sort_direction_values()
    {
      var url = string.Format("{0}/search?term={1}&sorting={2}", BaseUrl, "sitecore", "wItemName");

      Request.Execute(url, null, "GET");

      Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void Search_can_sort_results_from_database_using_multiple_order_parameters()
    {
      var url = string.Format("{0}/search?term={1}&sorting={2}&database=master&fields=TemplateName,ItemID&pageSize=100", BaseUrl, "sitecore", "aTemplateName|aItemName");

      var itemResults = (ItemResults)Request.Execute<ItemResults>(url, null, "GET");

      url = string.Format("{0}/search?term={1}&sorting={2}&database=master&fields=TemplateName,ItemID&pageSize=100",
                          BaseUrl, "sitecore", "aTemplateName|dItemName");

      var itemResultsMultiSortFields = (ItemResults)Request.Execute<ItemResults>(url, null, "GET");

      itemResults.TotalCount.ShouldEqual(itemResultsMultiSortFields.TotalCount);

      var sortOrderDifferent = false;

      for (var i = 0; i < itemResults.TotalCount; i++)
      {
        if (itemResults.Results[i][ItemModel.ItemID] != itemResultsMultiSortFields.Results[i][ItemModel.ItemID])
        {
          sortOrderDifferent = true;
        }
      }

      sortOrderDifferent.ShouldBeTrue();
    }


    private static void CompareResults(ItemResults itemResults, string selector, Action<int> shouldBeTest)
    {
      for (var i = 0; i < itemResults.Results.Count() - 2; i++)
      {
        var compare = string.Compare((string)itemResults.Results[i][selector],
                                     (string)itemResults.Results[i + 1][selector],
                                     StringComparison.InvariantCultureIgnoreCase);

        shouldBeTest(compare);
      }
    }
  }
}