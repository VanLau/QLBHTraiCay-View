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
    public class HangHoaController : Controller
    {

        #region Hang Hóa Trong Nước
        //GET: HangHoaTN 
        public async Task<ActionResult> HangHoaIndex(int? page, int? CLID)
        {
            try
            {
                if (CLID == null) CLID = 1;
                int pageNumber = (page == null || page < 1) ? 1 : page.Value;
                string url = $"hang-hoa/doc-mot-trang-hang-hoa-theo-chung-loai/{CLID}";
                var input = new PagedInput { PageIndex = pageNumber, PageSize = 12 };

                var result = await ApiHelper<PagedOutput<HangHoaOutPut>>.RunPostAsync(url,input);
                ViewBag.OnePageOfData = new StaticPagedList<HangHoaOutPut>(result.Items, input.PageIndex, input.PageSize, result.TotalItemCount); ;

                ViewBag.TieuDe = "TRÁI CÂY TRONG NƯỚC";
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_HangHoaTNAjaxPartial");
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Lỗi truy cập dữ liệu.<br>{ex.Message}");
            }

        }
        #endregion

        #region Hang hóa Nhập Khẩu
        [ChildActionOnly]
        public PartialViewResult _HangHoaNKPartial(int? CLID)
        {
            try
            {
                if (CLID == null) CLID = 2;
                string url = $"hang-hoa/hang-hoa-nhap-khau/{CLID}";
                var hhs = ApiHelper<List<HangHoaOutPut>>.RunGet(url);
                ViewBag.HangHoas = hhs;
                ViewBag.TieuDe = "TRÁI CÂY NHẬP KHẨU";
                return PartialView();
            }
            catch(Exception ex)
            {
                return PartialView($"Lỗi truy cập dữ liệu.<br>{ex.Message}");
            }
        }
        #endregion

        #region Sản phẩm bán chạy
        [ChildActionOnly]
        public PartialViewResult _HangHoaBCPartial()
        {
            try
            {
                string url = "hang-hoa/san-pham-ban-chay";
                var rs = ApiHelper<List<HangHoaOutputBS>>.RunGet(url);
                ViewBag.HangHoas = rs;
                return PartialView();
            }
            catch (Exception ex)
            {
                return PartialView($"Lỗi truy cập dữ liệu.< br /> lý do: { ex.Message}");
            }
        }
        #endregion

        #region Shop
        public async Task<ActionResult> Shop(int? page)
        {
            try
            {
                int pageNumber = (page == null || page < 1) ? 1 : page.Value;
                string url = "hang-hoa/doc-mot-trang-hang-hoa";
                var input = new PagedInput { PageIndex= pageNumber, PageSize= 12 };
                var onePageOfData = await ApiHelper<PagedOutput<HangHoaOutPut>>.RunPostAsync(url, input);
                ViewBag.OnePageOfData = new StaticPagedList<HangHoaOutPut>(onePageOfData.Items, input.PageIndex, input.PageSize, onePageOfData.TotalItemCount);
                ViewBag.TieuDe = "SẢN PHẨM";

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ShopAjaxPartial");
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Lỗi truy cập dữ liệu.<br>{ex.Message}");
            }
        }
        #endregion

        #region Hàng hóa theo Loại
        public async Task<ActionResult> TraCuuTheoLoaiAjax(int? id, int? page)
        {
            if (id == null || id < 1) return RedirectToAction("HangHoaIndex", "HangHoa");
            try
            {
                int Pagenumber = (page < 1 || page == null) ? 1 : page.Value;
                string url = $"hang-hoa/doc-mot-trang-theo-loai/{id}";
                var input = new PagedInput { PageIndex = Pagenumber, PageSize = 12 };
                var onePageOfData = await ApiHelper<PagedOutput<HangHoaOutPut>>.RunPostAsync(url, input);
                ViewBag.OnePageOfData = new StaticPagedList<HangHoaOutPut>(onePageOfData.Items, input.PageIndex, input.PageSize, onePageOfData.TotalItemCount);
                ViewBag.LoaiID = id;
                ViewBag.page = onePageOfData.TotalItemCount;
                ViewBag.pageSize = input.PageSize;

                string url1 = $"loai/loai-theo-id/{id}";
                var loai = await ApiHelper<LoaiOutput>.RunGetAsync(url1);
                ViewBag.TieuDe = $"{loai.ChungLoai.TenCL} - {loai.TenLoai}";
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ShopAjaxPartial");
                }
                return View("Shop");
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Lỗi truy cập dữ liệu.<br>{ex.Message}");
            }

        }
        #endregion

        #region Chi tiết sản phẩm   
        public async Task<ActionResult> ChiTietSanPham(int? id, string name)
        {
            if (id == null || id < 1) return RedirectToAction("HangHoaIndex", "HangHoa");
            try
            {
                string url = $"hang-hoa/doc-chi-tiet/{id}";
                var hangHoa = await ApiHelper<HangHoaOutPut>.RunGetAsync(url);
                if (hangHoa == null) throw new Exception($"Mặt hàng ID ={id} không tồn tại.");
                return View(hangHoa);
            }
            catch (Exception ex)
            {
                object cauBaoLoi = $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}";
                return View("BaoLoi", cauBaoLoi);
            }
        }
        #endregion

        #region Search hàng hóa theo tên
        [HttpPost]
        public async Task<ActionResult> Search(int? page, string search)
        {
            try
            {
                if (!String.IsNullOrEmpty(search))
                {                   
                    int pageNumber = (page == null || page < 1) ? 1 : page.Value;
                    var input =new PagedInput { PageIndex = pageNumber, PageSize = 12};
                    string url = $"hang-hoa/tim-kiem/{search}";
                    var onePageOfData = await ApiHelper<PagedOutput<HangHoaOutPut>>.RunPostAsync(url,input);
                    ViewBag.OnePageOfData = new StaticPagedList<HangHoaOutPut>(onePageOfData.Items, input.PageIndex, input.PageSize, onePageOfData.TotalItemCount);
                    ViewBag.TieuDe = $"Kết quả tìm kiếm: {search}";
                    ViewBag.page = onePageOfData.TotalItemCount;
                    ViewBag.pageSize = input.PageSize;
                    ViewBag.search = search;
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_SearchAjaxPartial");
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("HangHoaIndex");
                }

            }
            catch (Exception ex)
            {
                object cauBaoLoi = "không truy cập được dữ liệu <br/>" + ex.Message;
                return View("BaoLoi", cauBaoLoi);
            }

        }
        #endregion

    }
}