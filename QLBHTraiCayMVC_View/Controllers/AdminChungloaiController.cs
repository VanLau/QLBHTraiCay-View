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
    public class AdminChungloaiController : Controller
    {
        #region Đọc tất cả chủng loại
        // GET: AdminChungLoai
        public async Task<ActionResult> Index()
        {
            try
            {
                string url = "admin-chung-loai/doc-tat-ca";
                var chungLoais = await ApiHelper<List<ChungLoaiOutput>>.RunGetAsync(url);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_IndexPartial", chungLoais);
                }
                return View(chungLoais);
            }
            catch (Exception ex)
            {
                return View("BaoLoi", $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}");
            }
        }
        #endregion

        #region Đọc chi tiết chủng loại
        // GET: AdminLoai/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"admin-chung-loai/doc-chi-tiet/{id}";
                var chungLoai = await ApiHelper<ChungLoaiOutput>.RunGetAsync(url);

                if (chungLoai == null)
                {
                    return View("BaoLoi", model: $"Không tìm thấy hàng hóa.");
                }
                return View(chungLoai);
            }
            catch (Exception ex)
            {
                object cauBaoLoi = $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}";
                return View("BaoLoi", cauBaoLoi);
            }

        }
        #endregion

        #region Search chủng loại theo Mã hoặc Tên
        public async Task<ActionResult> Search(string search)
        {
            try
            {
                string url = $"admin-chung-loai/tim-kiem-chung-loai/{search}";
                if (!String.IsNullOrEmpty(search))
                {
                    var chungLoais = await ApiHelper<List<ChungLoaiOutput>>.RunGetAsync(url);
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_IndexPartial", chungLoais);
                    }
                    return View("Index", chungLoais);
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

        #region Thêm chủng Loại
        // GET: AdminLoai/Create
        public ActionResult Create()
        {
            try
            {                
                return View();
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Không truy cập được dữ liệu. {ex.Message}");
            }
        }

        // POST: AdminLoai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ChungLoaiInput chungLoai)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url = "admin-chung-loai/them-moi";
                    var result = await ApiHelper<ChungLoaiInput>.RunPostAsync(url, chungLoai);
                    TempData["SuccessMsg"] = $"Ghi thành công!<br/>ID: {result.ID}<br/>Mã số: {result.MaCL}<br/>Tên chủng loại: {result.TenCL}";
                    return RedirectToAction("Create");
                }
                return View(chungLoai);
            }
            catch (Exception ex)
            {
                //return View("BaoLoi", model: $"Không ghi được dữ liệu. {ex.Message}");
                ViewBag.ErrorMsg = $"Không ghi được dữ liệu. {ex.Message}";
                return View(chungLoai);
            }
        }
        #endregion

        #region Sửa Chủng loại
        // GET: AdminChungLoai/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"admin-chung-loai/doc-chi-tiet/{id}";
                var item = await ApiHelper<ChungLoaiInput>.RunGetAsync(url);
                if (item == null) throw new Exception($"Chủng loại ID={id} không tồn tại.");

                return View(item);
            }
            catch (Exception ex)
            {
                return View("BaoLoi", $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}");
            }

        }

        // POST: AdminChungLoai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ChungLoaiInput input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url1 = "admin-chung-loai/sua";
                    var result = await ApiHelper<string>.RunPostAsync(url1, input);
                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("Edit");
                }
                string url = $"admin-chung-loai/doc-chi-tiet/{input.ID}";
                var item = await ApiHelper<ChungLoaiInput>.RunGetAsync(url);

                return View(item);
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

        #region Xóa chủng loại
        // GET: AdminChungLoai/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"admin-chung-loai/doc-chi-tiet/{id}";
                var chungLoai = await ApiHelper<ChungLoaiOutput>.RunGetAsync(url);
                if (chungLoai == null) throw new Exception($"Hàng hóa ID={id} không tồn tại.");
                return View(chungLoai);
            }
            catch (Exception ex)
            {
                return View("BaoLoi", $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}");
            }

        }

        // POST: AdminLoai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                string url = $"admin-chung-loai/xoa-chung-loai/{id}";
                string result = await ApiHelper<string>.RunPostAsync(url);
                TempData["SuccessMsg"] = result;
                return View("Delete");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = $"Không xóa được dữ liệu. {ex.Message}";
                string url = $"admin-chung-loai/doc-chi-tiet/{id}";
                var chungLoai = await ApiHelper<ChungLoaiOutput>.RunGetAsync(url);
                return View(chungLoai);
            }
        }
        #endregion
    }
}