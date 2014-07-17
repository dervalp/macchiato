using System;
using System.Net;

using Sitecore.Services.Core.Model;

using Should;
using Xunit;

namespace Macchiato.Smoke.Test.ItemService.Query
{
  public class ItemServiceQueryViaItemBehaviour : ItemServiceTest
  {
    private string _url;

    private const string TestQueryId = "6179CE7D-7C5F-4679-8220-F8E401796FD0";

    public ItemServiceQueryViaItemBehaviour()
    {
      _url = string.Format("{0}/{1}/query?database=master", BaseUrl, TestQueryId);
    }

    [Fact]
    public void Query_returns_OK()
    {
      Request.Execute<ItemResults>(_url, null, "GET");

      Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    [Fact]
    public void Query_returns_item_array()
    {
      var response = (ItemResults)Request.Execute<ItemResults>(_url, null, "GET");

      response.Results.ShouldNotBeNull();
    }

    [Fact]
    public void Query_returns_bad_request_for_missing_definition_item()
    {
      _url = string.Format("{0}/{1}/query?database=core", BaseUrl, TestQueryId);

      Request.Execute<ItemResults>(_url, null, "GET");

      Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

      var expectedStatusMessage = string.Format("Query Definition ({0}) not found", TestQueryId);
      string.Compare(Request.HttpResponse.StatusDescription, expectedStatusMessage, StringComparison.InvariantCultureIgnoreCase).ShouldEqual(0);
    }
  }
}