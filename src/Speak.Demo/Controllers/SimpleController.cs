using System.Web.Mvc;

namespace Macchiato.Controllers
{
  public class SimpleController : Controller
  {
    //
    // GET: /Simple/

    public ActionResult Index()
    {
      return View();
    }
  }
}
