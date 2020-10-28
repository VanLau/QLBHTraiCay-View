using QLBHTraiCayMVC_View.DTO;
using QLBHTraiCayMVC_View.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace QLBHTraiCayMVC_View.Controllers
{
    [RoutePrefix("quan-ly/loai-hang")]
    public class AdminLoaiController : Controller
    {
        #region Đọc tất cả loại
        // GET: AdminLoai
        [Route("danh-sach-loai-hang")]
        public async Task<ActionResult> Index()
        {
            try
            {
                string url = "admin-loai/doc-tat-ca";
                var loais = await ApiHelper<List<LoaiOutput>>.RunGetAsync(url);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_IndexPartial", loais);
                }
                return View(loais);
            }
            catch(Exception ex)
            {
                return View("BaoLoi", $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}");
            }
        }
        #endregion

        #region Đọc chi tiết loại
        // GET: AdminLoai/Details/5
        [Route("chi-tiet-loai-hang/{id?}")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"admin-loai/doc-chi-tiet/{id}";
                var loai = await ApiHelper<LoaiOutput>.RunGetAsync(url);

                if (loai == null)
                {
                    return View("BaoLoi", model: $"Không tìm thấy hàng hóa.");
                }
                return View(loai);
            }
            catch(Exception ex)
            {
                object cauBaoLoi = $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}";
                return View("BaoLoi", cauBaoLoi);
            }
            
        }
        #endregion

        #region Search loại theo Mã hoặc Tên
        public async Task<ActionResult> Search(string search)
        {
            try
            {
                string url = $"admin-loai/tim-kiem-loai/{search}";
                if (!String.IsNullOrEmpty(search))
                {
                    var loais = await ApiHelper<List<LoaiOutput>>.RunGetAsync(url);
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_IndexPartial", loais);
                    }
                    return View("Index", loais);
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

        #region Select Danh sách Chủng Loại
        // GET:
        [Route]
        public async Task<SelectList> TaoDanhSachChungLoai(int IDChon = 0)
        {
            string url = "admin-chung-loai/doc-tat-ca";
            List<ChungLoaiOutput> chungLoais = await ApiHelper<List<ChungLoaiOutput>>.RunGetAsync(url);
            var items = await chungLoais.Select(p => new
                                        {
                                            p.ID,
                                            ThongTin = p.MaCL + " - " + p.TenCL
                                        })
                                        .ToListAsync();

            items.Insert(0, new { ID = 0, ThongTin = "------ Chọn chủng loại ------" });
            var result = new SelectList(items, "ID", "ThongTin", selectedValue: IDChon);
            return result;
        }
        #endregion

        #region Thêm Loại
        // GET: AdminLoai/Create
        [Route("them-moi-loai-hang")]
        public async Task<ActionResult> Create()
        {
            try
            {
                ViewBag.ChungLoaiID = await TaoDanhSachChungLoai();
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
        public async Task<ActionResult> Create(LoaiInput loai)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url = "admin-loai/them-moi";
                    var result = await ApiHelper<LoaiInput>.RunPostAsync(url,loai);
                    TempData["SuccessMsg"] = $"Ghi thành công!<br/>ID: {result.ID}<br/>Mã số: {result.MaLoai}<br/>Tên chủng loại: {result.TenLoai}";
                    return RedirectToAction("Create");
                }

                ViewBag.ChungLoaiID = await TaoDanhSachChungLoai(loai.ChungLoaiID);
                return View(loai);
            }
            catch (Exception ex)
            {
                //return View("BaoLoi", model: $"Không ghi được dữ liệu. {ex.Message}");
                ViewBag.ErrorMsg = $"Không ghi được dữ liệu. {ex.Message}";
                ViewBag.ChungLoaiID = await TaoDanhSachChungLoai(loai.ChungLoaiID);
                return View(loai);
            }
        }
        #endregion

        #region Sửa loại
        // GET: AdminLoai/Edit/5
        [Route("sua-loai-hang/{id?}")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"admin-loai/doc-chi-tiet/{id}";
                var item = await ApiHelper<LoaiInput>.RunGetAsync(url);
                if (item == null) throw new Exception($"Hàng hóa ID={id} không tồn tại.");

                ViewBag.ChungLoaiID = await TaoDanhSachChungLoai(item.ChungLoaiID);
                return View(item);
            }
            catch (Exception ex)
            {
                return View("BaoLoi", $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}");
            }

        }

        // POST: AdminLoai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LoaiInput input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url1 = "admin-loai/sua-loai";
                    var result =await ApiHelper<string>.RunPostAsync(url1, input);
                    TempData["SuccessMsg"] = result;
                    return RedirectToAction("Edit");
                }
                string url = $"admin-loai/doc-chi-tiet/{input.ID}";
                var item = await ApiHelper<LoaiInput>.RunGetAsync(url);
                ViewBag.ChungLoaiID = await TaoDanhSachChungLoai(input.ChungLoaiID);
                return View(item);
            }
            catch (Exception ex)
            {
                //return View("BaoLoi", model: $"Không ghi được dữ liệu. <br/> Lý do:{ex.Message}");
                ViewBag.ErrorMsg = $"Không sửa được dữ liệu. {ex.Message}";
                //string url = $"admin-loai/doc-chi-tiet/{input.ID}";
                //var loai = await ApiHelper<LoaiInput>.RunGetAsync(url);
                ViewBag.ChungLoaiID = await TaoDanhSachChungLoai(input.ChungLoaiID);
                return View(input);
            }
        }
        #endregion

        #region Xóa loại
        // GET: AdminLoai/Delete/5
        [Route("xoa-loai-hang/{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"admin-loai/doc-chi-tiet/{id}";
                var loai = await ApiHelper<LoaiOutput>.RunGetAsync(url);
                if (loai == null) throw new Exception($"Loại ID={id} không tồn tại.");
                return View(loai);
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
                string url = $"admin-loai/xoa-loai/{id}";
                string result = await ApiHelper<string>.RunPostAsync(url);
                TempData["SuccessMsg"] = result;
                return View("Delete");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = $"Không xóa được dữ liệu. {ex.Message}";
                string url = $"admin-loai/doc-chi-tiet/{id}";
                var loai = await ApiHelper<LoaiOutput>.RunGetAsync(url);
                ViewBag.ChungLoaiID = await TaoDanhSachChungLoai(loai.ChungLoaiID);
                return View(loai);
            }
        }
        #endregion
    }
}
