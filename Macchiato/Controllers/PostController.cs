using Sitecore.Services.Core;
using Macchiato.Data;
using Macchiato.Model;
using Sitecore.Services.Infrastructure.Sitecore.Services;

namespace Macchiato.Controllers
{
  [ServicesController]
  public class PostController : EntityService<Post>
  {
    public PostController(IRepository<Post> repository)
      : base(repository)
    {
    }

    public PostController()
      : this(new PostRepository())
    {
    }
  }
}