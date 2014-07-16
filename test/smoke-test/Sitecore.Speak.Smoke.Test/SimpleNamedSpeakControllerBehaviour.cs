using System.Linq;
using System.Net;
using System.Web;

using Should;
using Xunit;

using Sitecore.Services.Samples.Model;

namespace Sitecore.Speak.Smoke.Test
{
  public class SimpleNamedSpeakControllerBehaviour : EntityServiceTest<SimpleData>
  {
    public SimpleNamedSpeakControllerBehaviour()
      : base(UrlHelper.BuildApiRelativeUrl("namedpath/simple"), new SimpleData { Value = "ItemFour", Payload = "Data" })
    {
    }

    [Fact]
    public void OPTIONS_returns_entity_details()
    {
      Options().ShouldNotBeNull();
    }

    [Fact]
    public void OPTIONS_returns_OK_status_code()
    {
      Options().StatusCode.ShouldEqual(200);
    }

    [Fact]
    public void FetchEntities_returns_entity_collection()
    {
      FetchEntities().ShouldNotBeNull();
    }

    [Fact]
    public void FetchEntity_can_retrieve_specific_entity()
    {
      var entities = FetchEntities();

      if (entities.Length > 0)
      {
        var entityId = entities.First().Id;

        GetEntity(entityId).Id.ShouldEqual(entityId);
      }
    }

    [Fact]
    public void CreateEntity_can_create_new_entity()
    {
      Request.Execute<HttpResponse>(BaseUrl, EntityInstance, "POST");
      Request.HttpResponse.Headers["Location"].ShouldNotBeNull();
      Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.Created);

      EntityInstance.Id = GetIdentityFromLocationHeader(Request.HttpResponse.Headers["Location"]);
      ItemsToRemove.Add(EntityInstance);
    }

    [Fact]
    public void CreateEntity_returns_bad_request_if_entity_exists()
    {
      Request.Execute<HttpResponse>(BaseUrl, EntityInstance, "POST");

      EntityInstance.Id = GetIdentityFromLocationHeader(Request.HttpResponse.Headers["Location"]);
      ItemsToRemove.Add(EntityInstance);

      var duplicateRequest = new Request();
      duplicateRequest.Execute<HttpResponse>(BaseUrl, EntityInstance, "POST");

      duplicateRequest.HttpResponse.Headers["Location"].ShouldBeNull();
      duplicateRequest.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
      duplicateRequest.HttpResponse.StatusDescription.ShouldEqual("Entity exists");
    }

    [Fact]
    public void Can_delete_an_entity()
    {
      Request.Execute<HttpResponse>(BaseUrl, EntityInstance, "POST");
      Request.HttpResponse.Headers["Location"].ShouldNotBeNull();
      EntityInstance.Id = GetIdentityFromLocationHeader(Request.HttpResponse.Headers["Location"]);

      DeleteEntity(EntityInstance);

      Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.NoContent);
    }
  }
}