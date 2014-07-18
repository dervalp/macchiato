using System.Web.Http;
using System.Web.Http.Cors;
using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Sitecore.Services;
using Sitecore.Services.Samples.Data;
using Sitecore.Services.Samples.Model;

namespace Speak.Sample
{
  [ServicesController]
  [EnableCors(origins: "*", headers: "*", methods: "*")]
  public class SpeakTestController : EntityService<SimpleData>
  {
    public SpeakTestController(IRepository<SimpleData> entityRepository)
      : base(entityRepository)
    {
    }

    public SpeakTestController()
      : this(new FakeEntityRepository())
    {
    }

    [HttpGet]
    public string HelloWorld()
    {
      return "Hello World";
    }

    [HttpPut]
    public string Trigger(string id)
    {
      return "Triggering id : " + id;
    }
  }
}