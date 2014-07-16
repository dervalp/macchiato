using System.Net;
using Should;
using Xunit;

namespace Sitecore.Speak.Smoke.Test.EntityService
{
  public class OverridePathBehaviour : EntityServiceTest
  {
    [Fact]
    public void Should_not_respond_to_services_controller_namespace_url()
    {
      Request.Execute(BaseUrl + "Sitecore-Services-Samples-Controllers/SimpleNamedServices", null, "GET");
      Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }

    [Fact]
    public void Should_respond_to_services_controller_unique_name_url()
    {
      Request.Execute(BaseUrl + "NamedPath/Simple", null, "GET");
      Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }
  }
}