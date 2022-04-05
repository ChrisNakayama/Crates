using Microsoft.AspNetCore.Mvc;

namespace RecordBox.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }
  }
}