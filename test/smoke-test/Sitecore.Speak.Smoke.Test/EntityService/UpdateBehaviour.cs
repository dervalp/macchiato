using System.Net;

using Sitecore.Services.Samples.Model;

using Should;
using Xunit;

namespace Sitecore.Speak.Smoke.Test.EntityService
{
  public class UpdateBehaviour : EntityServiceTest
  {
    [Fact]
    public void Should_return_404_for_entity_does_not_exist()
    {
      var entity = new SimpleData { Value = "Item 6", Payload = "cdef", Id = "8f942970-367f-4089-a51b-50c3252fa24e" };
      Request.Execute(BaseUrl + "namedpath/simple", entity, "PUT");
      Request.HttpResponse.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
    }
  }
}