using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Http;
using System.Web.Mvc;

namespace QLBHTraiCayMVC_View.Controllers
{
    public class HomeController : Controller
    {
        //[Route()]
        public ActionResult Index()
        {
            //return View();
            return RedirectToAction("HangHoaIndex", "HangHoa");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        public int _ThongKeSoNguoiOnline()
        {
            int d = 0;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Application != null)
            {
                System.Web.HttpContext.Current.Application.Lock();
                d = (int)System.Web.HttpContext.Current.Application["SoNguoiOnline"];
                System.Web.HttpContext.Current.Application.UnLock();
            }
            return d;
        }
    }
}