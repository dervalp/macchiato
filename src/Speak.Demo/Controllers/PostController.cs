using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Sitecore.Services;
using Macchiato.Data;
using Macchiato.Model;

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