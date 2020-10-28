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
    public class GioHangController : Controller
    {
        #region Giỏ hàng
        // GET: GioHang
        [Route("gio-hang")]
        public ActionResult Index()
        {
            try
            {
                //Tham chiếu đến giỏ hàng lưu trong Session
                var gioHang = Session["GioHang"] as GioHangDTO;
                ViewBag.ShoppingCartAct = "active";
                ViewBag.GioHang = gioHang;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_IndexPartial");
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Lỗi truy cập dữ liệu.<br>{ex.Message}");
            }

        }
        #endregion

        #region Thêm Vào giỏ hàng
        // POST: GioHang/Create
        [HttpPost]
        [Route("gio-hang-them")]
        public ActionResult AddToCart(int HangHoaID, int SoLuong = 1)
        {
            try
            {
                //Tham chiếu đến giỏ hàng lưu trong Session
                var gioHang = Session["GioHang"] as GioHangDTO;
                if (gioHang == null)
                {//Lần đầu chọn mua 1 mã hàng
                    gioHang = new GioHangDTO();
                    Session["GioHang"] = gioHang;
                }
                string url = $"hang-hoa/doc-chi-tiet/{HangHoaID}";
                HangHoaOutPut hangHoa = ApiHelper<HangHoaOutPut>.RunGet(url);
                var item = new GioHangItem(hangHoa, SoLuong);
                gioHang.Them(item);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Lỗi truy cập dữ liệu.<br>{ex.Message}");
            }

        }
        #endregion

        #region Sửa giỏ hàng
        // POST: GioHang/Edit/5
        [HttpPost]
        [Route("gio-hang-sua")]
        public ActionResult Edit(int HangHoaID, int SoLuong)
        {
            try
            {
                //Tham chiếu đến giỏ hàng trong Session
                var gioHang = Session["GioHang"] as GioHangDTO;
                gioHang.HieuChinh(HangHoaID, SoLuong);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Lỗi truy cập dữ liệu.<br>{ex.Message}");
            }
        }
        #endregion

        #region Xóa giỏ hàng
        // POST: GioHang/Delete/5
        [HttpPost]
        [Route("gio-hang-xoa")]
        public ActionResult Delete(int HangHoaID)
        {
            try
            {
                //Tham chiếu đến giỏ hàng trong Session
                var gioHang = Session["GioHang"] as GioHangDTO;
                gioHang.Xoa(HangHoaID);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Lỗi truy cập dữ liệu.<br>{ex.Message}");
            }
        }
        #endregion

        #region Xử lý đơn đặt hàng
        [HttpGet]
        [Route("dat-hang")]
        public ActionResult DatHang()
        {
            return View();
        }

        [HttpPost]
        [Route("dat-hang")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DatHang(HoaDonDTO hoaDon)
        {
            var gioHang = Session["GioHang"] as GioHangDTO;
            if (gioHang == null || gioHang.TongSanPham == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                string url = "gio-hang/hoa-don";
                //1.Thêm HoaDon
                hoaDon.NgayDatHang = DateTime.Now;
                hoaDon.TongTien = gioHang.TongTriGia;

                //2.Thêm HoaDonChiTiet
                List<HoaDonChiTietDTO> hoaDonChiTiet = new List<HoaDonChiTietDTO>();
                foreach (var item in gioHang.DanhSach)
                {
                    HoaDonChiTietDTO ct = new HoaDonChiTietDTO();
                    ct.HoaDonID = hoaDon.ID;
                    ct.HangHoaID = item.HangHoa.ID;
                    ct.SoLuong = item.SoLuong;
                    ct.DonGia = item.HangHoa.GiaBan;
                    ct.ThanhTien = item.HangHoa.GiaBan * item.SoLuong;
                    hoaDonChiTiet.Add(ct);
                }

                var input = new InputHDDTO { HDItem = hoaDon, HDCTItems = hoaDonChiTiet };
                var hoaDonCTs = await ApiHelper<List<HoaDonChiTietBSDTO>>.RunPostAsync(url, input);
                gioHang.XoaTatCa();  
                
                ViewBag.hoaDonCT = hoaDonCTs;
                return View("DatHangThanhCong");
            }
            catch (Exception ex)
            {
                TempData["LoiDatHang"] = "Đặt hàng không thành công.<br>" + ex.Message;
                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}