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
    public class AdminHoaDonController : Controller
    {
        #region Đọc tất cả hóa đơn
        // GET: AdminHoaDon
        [Route("danh-sach-hoa-don")]
        public async Task<ActionResult> Index()
        {
            try
            {
                string url = "admin-hoa-don/doc-tat-ca-hoa-don";
                var hoaDons = await ApiHelper<List<HoaDonDTO>>.RunGetAsync(url);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_IndexPartial", hoaDons);
                }
                return View(hoaDons);
            }
            catch (Exception ex)
            {
                return View("BaoLoi", $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}");
            }
        }
        #endregion

        #region Đọc chi tiết hóa đơn
        // GET: AdminHoaDon/Details/5
        [Route("chi-tiet-hoa-don/{id?}")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"admin-hoa-don/doc-chi-tiet/{id}";
                var hoaDonChiTietBS = await ApiHelper<List<HoaDonChiTietBSDTO>>.RunGetAsync(url);

                if (hoaDonChiTietBS == null)
                {
                    return View("BaoLoi", model: $"Không tìm thấy hóa đơn.");
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_DetailsPartial", hoaDonChiTietBS);
                }
                return View(hoaDonChiTietBS);
            }
            catch (Exception ex)
            {
                object cauBaoLoi = $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}";
                return View("BaoLoi", cauBaoLoi);
            }

        }
        #endregion

        #region Search chủng loại theo Tên Khách hàng
        [Route]
        public async Task<ActionResult> Search(string search)
        {
            try
            {
                string url = $"admin-hoa-don/tim-kiem-ten/{search}";
                if (!String.IsNullOrEmpty(search))
                {
                    var hoaDons = await ApiHelper<List<HoaDonDTO>>.RunGetAsync(url);
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_IndexPartial", hoaDons);
                    }
                    return View("Index", hoaDons);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                object cauBaoLoi = "không truy cập được dữ liệu <br/>" + ex.Message;
                return View("BaoLoi", cauBaoLoi);
            }

        }
        #endregion

        #region Sửa Hóa Đơn
        // GET: AdminHoaDon/Edit/5
        [Route("sua-hoa-don/{id?}")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"admin-hoa-don/doc-chi-tiet-hoa-don/{id}";
                var item = await ApiHelper<HoaDonDTO>.RunGetAsync(url);
                if (item == null) throw new Exception($"Chủng loại ID={id} không tồn tại.");

                return View(item);
            }
            catch (Exception ex)
            {
                return View("BaoLoi", $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}");
            }

        }

        // POST: AdminHoaDon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HoaDonDTO input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url1 = "admin-hoa-don/sua-hoa-don";
                    var result = await ApiHelper<string>.RunPostAsync(url1, input);
                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("Edit");
                }

                return View(input);
            }
            catch (Exception ex)
            {
                //return View("BaoLoi", model: $"Không ghi được dữ liệu. <br/> Lý do:{ex.Message}");
                ViewBag.ErrorMsg = $"Không sửa được dữ liệu. {ex.Message}";
                //string url = $"admin-loai/doc-chi-tiet/{input.ID}";
                //var loai = await ApiHelper<LoaiInput>.RunGetAsync(url);
                return View(input);
            }
        }
        #endregion
    }
}