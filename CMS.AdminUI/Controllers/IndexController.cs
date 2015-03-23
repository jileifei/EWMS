using System.Web.Mvc;

namespace CMS.AdminUI.Controllers
{
    public class IndexController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.UserName = UserName;
            return View();
        }

    }
}
