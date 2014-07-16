using System;
using System.Web;

using Sitecore.Services.Core.Model;

using sc = Sitecore;

namespace Sitecore.Speak.Smoke.Test.ItemService
{
  public abstract class ItemServiceTest : IDisposable
  {
    protected string BaseUrl;
    protected Request Request;

    protected const string HomeItem = "110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9";

    protected ItemServiceTest()
    {
      BaseUrl = UrlHelper.BuildApiUrl("item");
      Request = new Request();

      Login();
    }

    protected void Login()
    {
      const string loginCredentials = @"{
                                            ""domain"": ""sitecore"",
                                            ""username"": ""admin"",
                                            ""password"": ""b""
                                        }";

      var url = new UriBuilder(UrlHelper.BuildApiUrl("auth/login")) { Scheme = "https", Port = 443 };

      Request.Execute<HttpResponse>(url.Uri.ToString(), loginCredentials, "POST", "application/json");
    }

    protected void Logout()
    {
      Request.Execute<HttpResponse>(UrlHelper.BuildApiUrl("auth/logout"), "", "POST");
    }

    protected ItemModel GetItem(string url)
    {
      return (ItemModel)Request.Execute<ItemModel>(url, null, "GET");
    }

    protected string CreateItem(string url, ItemModel itemModel)
    {
      Request.Execute<HttpResponse>(url, itemModel, "POST");
      return Request.HttpResponse.Headers["Location"];
    }

    protected void UpdateItem(string url, ItemModel itemModel)
    {
      Request.Execute<HttpResponse>(url, itemModel, "PATCH");
    }

    protected void DeleteItem(string url)
    {
      Request.Execute<HttpResponse>(url, null, "DELETE");
    }

    protected void DeleteItem(Guid itemId, string database)
    {
      var url = string.Format("{0}/{1}?database={2}", BaseUrl, itemId, database);
      Request.Execute<HttpResponse>(url, null, "DELETE");
    }

    public void Dispose()
    {
      var url = string.Format("{0}/{1}/children?database=master", BaseUrl, HomeItem);
      var response = (ItemModel[])Request.Execute<ItemModel[]>(url, null, "GET");

      foreach (var itemModel in response)
      {
        if (String.CompareOrdinal((string)itemModel["ItemName"], "Item Name") == 0)
        {
          DeleteItem(new Guid((string)itemModel["ItemID"]), "master");
        }
      }

      Logout();
    }
  }
}