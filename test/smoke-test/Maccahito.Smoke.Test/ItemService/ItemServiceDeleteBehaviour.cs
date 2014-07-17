using System.Net;

using Should;
using Xunit;

using Sitecore.Services.Core.Model;

namespace Macchiato.Smoke.Test.ItemService
{
    public class ItemServiceDeleteBehaviour : ItemServiceTest
    {
        private readonly ItemModel _item;
        private const string HomeItemPath = "sitecore/content/home";

        public ItemServiceDeleteBehaviour()
        {
            _item = new ItemModel
                {
                    {"ItemName", "Item Name"},
                    {"TemplateID", "76036f5e-cbce-46d1-af0a-4143f9b557aa"},
                };
        }

        [Fact]
        public void Returns_Not_Found_for_invalid_item_id()
        {
            var url = string.Format("{0}/{1}", BaseUrl, "110D559F-DEA5-42EA-9C1C-8A5DF7E70EF8");

            DeleteItem(url);

            Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
        }

        [Fact]
        public void Returns_Bad_Request_for_invalid_language()
        {
            var url = string.Format("{0}/{1}?language=foo", BaseUrl, HomeItem);

            DeleteItem(url);

            Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void Returns_Bad_Request_for_invalid_database()
        {
            var url = string.Format("{0}/{1}?database=foo", BaseUrl, HomeItem);

            DeleteItem(url);

            Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void Returns_NoContent_when_successful()
        {
            var url = string.Format("{0}/{1}", BaseUrl, HomeItemPath); 
            
            url = CreateItem(url, _item);

            var item = GetItem(url);
            
            url = string.Format("{0}/{1}?database=master", BaseUrl, item["ItemID"]);

            DeleteItem(url);

            Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.NoContent);
        }
    }
}