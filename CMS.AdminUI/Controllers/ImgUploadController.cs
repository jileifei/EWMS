using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.AdminUI.Controllers
{
    public class ImgUploadController : BaseController
    {
        public ActionResult Index()
        {
            ViewData["Type"] = Request.QueryString["Type"];
            return View();
        }

    }
}
