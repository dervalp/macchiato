using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Sitecore.Services;
using Speak.Blog.Data;
using Speak.Blog.Model;

namespace Speak.Blog.Controllers
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