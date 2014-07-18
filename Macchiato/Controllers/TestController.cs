using System.Web.Http;

namespace Macchiato.Controllers
{
  public class TestController : ApiController
  {
    //
    // GET /api/Test/1

    [HttpGet]
    public string GetTestById(int id)
    {
      return string.Format("Hello WebApi {0}!", id);
    }
  }
}