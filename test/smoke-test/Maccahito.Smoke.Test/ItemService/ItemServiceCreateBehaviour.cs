using System.Net;
using System.Web;

using Should;
using Xunit;

using Sitecore.Services.Core.Model;

namespace Macchiato.Smoke.Test.ItemService
{
    public class ItemServiceCreateBehaviour : ItemServiceTest
    {
        private readonly ItemModel _item;
        private const string HomeItemPath = "sitecore/content/home";

        public ItemServiceCreateBehaviour()
        {
            _item = new ItemModel
                {
                    {"ItemName", "Item Name"},
                    {"TemplateID", "76036f5e-cbce-46d1-af0a-4143f9b557aa"},
                };
        }

        [Fact]
        public void Returns_Bad_Request_for_invalid_path()
        {
            var url = string.Format("{0}/{1}", BaseUrl, "sitecore/content/home/doesnotexist");

            CreateItem(url, _item);

            Request.Execute<HttpResponse>(url, _item, "POST");
            Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void Returns_Bad_Request_for_invalid_language()
        {
            var url = string.Format("{0}/{1}?language=foo", BaseUrl, HomeItemPath);

            CreateItem(url, _item);
            Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void Returns_Bad_Request_for_invalid_database()
        {
            var url = string.Format("{0}/{1}?database=foo", BaseUrl, HomeItemPath);

            CreateItem(url, _item);
            Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void Returns_Created_when_successful()
        {
            var url = string.Format("{0}/{1}", BaseUrl, HomeItemPath);

            CreateItem(url, _item);
            Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.Created);
        }

        [Fact]
        public void Returns_Location_header_when_successful()
        {
            var url = string.Format("{0}/{1}", BaseUrl, HomeItemPath);

            CreateItem(url, _item);
            Request.HttpResponse.Headers["Location"].ShouldNotBeNull();
        }
    }
}