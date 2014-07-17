using System;
using Should;
using Xunit;

using Sitecore.Services.Core.Model;

namespace Macchiato.Smoke.Test.ItemService
{
    public class ItemServiceReadChildrenBehaviour : ItemServiceTest
    {
        [Fact]
        public void Returns_children_of_Home_item()
        {
            var url = string.Format("{0}/{1}/children", BaseUrl, HomeItem);
            var response = (ItemModel[])Request.Execute<ItemModel[]>(url, null, "GET");
            response.ShouldNotBeNull();
            response.ShouldNotBeEmpty();

            foreach (var itemModel in response)
            {
                string.Compare((string)itemModel["ParentID"],
                               HomeItem,
                               StringComparison.InvariantCultureIgnoreCase)
                      .ShouldEqual(0);
            }
        }
    }
}