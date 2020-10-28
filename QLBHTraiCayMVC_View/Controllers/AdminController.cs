using QLBHTraiCayMVC_View.DTO;
using QLBHTraiCayMVC_View.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QLBHTraiCayMVC_View.Controllers
{
    [RoutePrefix("quan-ly")]
    public class AdminController : Controller
    {
        // GET: Admin
        [Route]
        public async Task<ActionResult> Index()
        {
            string url1 = "admin-hang-hoa/doc-tat-ca";
            List<HangHoaOutPut> tsHH1 = await ApiHelper<List<HangHoaOutPut>>.RunGetAsync(url1);
            int tsHH = tsHH1.Count();

            string url2 = "admin-loai/doc-tat-ca";
            List<LoaiOutput> tsL1 = await ApiHelper<List<LoaiOutput>>.RunGetAsync(url2);
            int tsL = tsL1.Count();

            string url3 = "admin-chung-loai/doc-tat-ca";
            List<ChungLoaiOutput> tsCL1 = await ApiHelper<List<ChungLoaiOutput>>.RunGetAsync(url3);
            int tsCL = tsCL1.Count();

            string url4 = "admin-hoa-don/doc-tat-ca-hoa-don";
            List<ChungLoaiOutput> tsHD1 = await ApiHelper<List<ChungLoaiOutput>>.RunGetAsync(url4);
            int tsHD = tsHD1.Count();

            ViewBag.TongChungLoai = tsCL;
            ViewBag.TongLoai = tsL;
            ViewBag.TongSoHangHoa = tsHH;
            ViewBag.TongSoHoaDon = tsHD;
            return View();
        }
    }
}