using QLBHTraiCayMVC_View.DTO;
using QLBHTraiCayMVC_View.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBHTraiCayMVC_View.Controllers
{
    public class ChungLoaiController : Controller
    {
        #region Danh sách chung loại
        // GET: ChungLoai
        [ChildActionOnly]
        public PartialViewResult _ChungLoaiPartial()
        {
            try
            {
                string url = "chung-loai/danh-sach-chung-loai";
                var chungLoai = ApiHelper<List<ChungLoaiOutput>>.RunGet(url);
                ViewBag.ChungLoais = chungLoai;
                return PartialView();
            }
            catch (Exception ex)
            {
                return PartialView("BaoLoi", model: $"Lỗi truy cập dữ liệu.<br>{ex.Message}");
            }
        }
        #endregion
    }
}