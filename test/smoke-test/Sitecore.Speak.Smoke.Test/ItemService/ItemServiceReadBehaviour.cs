using System;
using System.Net;
using Should;
using Xunit;

namespace Sitecore.Speak.Smoke.Test.ItemService
{
    public class ItemServiceReadBehaviour : ItemServiceTest
    {
        [Fact]
        public void Returns_OK_for_item_that_exists()
        {
            var url = string.Format("{0}/{1}", BaseUrl, HomeItem);

            GetItem(url);

            Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }

        [Fact]
        public void Returns_Home_item()
        {
            var url = string.Format("{0}/{1}", BaseUrl, HomeItem);

            var itemModel = GetItem(url);

            string.Compare((string)itemModel["ItemID"],
                               HomeItem,
                               StringComparison.InvariantCultureIgnoreCase)
                .ShouldEqual(0);
        }
    
        [Fact]
        public void Returns_Home_item_by_content_path()
        {
            var url = string.Format("{0}?path=/sitecore/content/Home", BaseUrl);

            var itemModel = GetItem(url);

            string.Compare((string)itemModel["ItemID"],  // TODO replace hard coding
                               HomeItem,
                               StringComparison.InvariantCultureIgnoreCase)
                .ShouldEqual(0);
        }
    }
}