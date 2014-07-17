using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Sitecore.Services;
using Sitecore.Services.Samples.Data;
using Sitecore.Services.Samples.Model;

namespace Sitecore.Services.Samples.Controllers
{
  [ServicesController("NamedPath.Simple")]
  public class SimpleNamedServicesController : EntityService<SimpleData>
  {
    public SimpleNamedServicesController(IRepository<SimpleData> repository)
      : base(repository)
    {
    }

    public SimpleNamedServicesController()
      : this(new FakeEntityRepository())
    {
    }
  }
}