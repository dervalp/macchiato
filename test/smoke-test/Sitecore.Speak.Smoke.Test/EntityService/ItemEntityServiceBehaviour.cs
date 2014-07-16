using System.Net;

using Sitecore.Services.Contrib.Model;

using Should;
using Xunit;

namespace Sitecore.Speak.Smoke.Test.EntityService
{
  public class ItemEntityServiceBehaviour : EntityServiceTest
  {
    [Fact]
    public void Should_return_items()
    {
      Request.Execute(BaseUrl + "custom/item", null, "GET");
      Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    [Fact]
    public void Should_return_home_item()
    {
      var itemEntity = (ItemEntity)Request.Execute<ItemEntity>(BaseUrl + "custom/item/110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9", null, "GET");

      itemEntity.Id.ToUpper().ShouldEqual("110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9");
    }
  }
}