using QLBHTraiCayMVC_View.DTO;
using QLBHTraiCayMVC_View.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace QLBHTraiCayMVC_View.Controllers
{
    [RoutePrefix("quan-ly/hang-hoa")]
    public class AdminHangHoaController : Controller
    {
        #region Danh sách hàng hóa
        // GET: AdminHangHoa
        [Route("danh-sach-hang-hoa")]
        public async Task<ActionResult> Index()
        {
            try
            {
                string url = "admin-hang-hoa/doc-tat-ca";
                var hangHoas = await ApiHelper<List<HangHoaOutPut>>.RunGetAsync(url);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_IndexPartial", hangHoas);
                }
                return View(hangHoas);
            }
            catch (Exception ex)
            {
                object cauBaoLoi = $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}";
                return View("BaoLoi", cauBaoLoi);
            }
        }
        #endregion

        #region Chi tiết hàng hóa
        // GET: AdminHangHoa/Details/5
        [Route("chi-tiet-hang-hoa/{id?}")]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null || id<0)
                {
                    return RedirectToAction("Index");
                }
                string url = $"admin-hang-hoa/doc-chi-tiet/{id}";
                var hangHoa = await ApiHelper<HangHoaOutPut>.RunGetAsync(url);

                if (hangHoa == null)
                {
                    return View("BaoLoi", model: $"Không tìm thấy hàng hóa.");
                }
                return View(hangHoa);
            }
            catch (Exception ex)
            {
                object cauBaoLoi = $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}";
                return View("BaoLoi", cauBaoLoi);
            }

        }
        #endregion

        #region Search hàng hóa theo Mã hoặc Tên
        [Route]
        public async Task<ActionResult> Search(string search)
        {
            try
            {
                string url = $"admin-hang-hoa/tim-kiem-ma-ten/{search}";
                if (!String.IsNullOrEmpty(search))
                {
                    var hangHoas = await ApiHelper<List<HangHoaOutPut>>.RunGetAsync(url);
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_IndexPartial", hangHoas);
                    }
                    return View("Index", hangHoas);
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

        #region Select Danh sách Loại
        // GET:
        public async Task<SelectList> TaoDanhSachLoai(int IDChon = 0)
        {
            string url = "loai/doc-tat-ca";
            List<LoaiOutput> loais = await ApiHelper<List<LoaiOutput>>.RunGetAsync(url);
            var items = await loais.Select(p => new
                                {
                                    p.ID,
                                    ThongTin = p.MaLoai + " - " + p.TenLoai
                                })
                                .ToListAsync();

            items.Insert(0, new { ID = 0, ThongTin = "------ Chọn loại ------" });
            var result = new SelectList(items, "ID", "ThongTin", selectedValue: IDChon);
            return result;
        }
        #endregion

        #region Thêm hàng hóa
        // GET: AdminHangHoa/Create
        [Route("them-moi-hang-hoa")]
        public async Task<ActionResult> Create()
        {
            try
            {
                ViewBag.LoaiID = await TaoDanhSachLoai();
                return View();
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Không truy cập được dữ liệu. {ex.Message}");
            }
        }

        // POST: AdminHangHoa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HangHoaInPut hangHoa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url = "admin-hang-hoa/them-moi-hang-hoa";
                    await ApiHelper<HangHoaInPut>.RunPostAsync(url, hangHoa);   
                    return RedirectToAction("Index");
                }

                ViewBag.LoaiID = await TaoDanhSachLoai(hangHoa.LoaiID);
                return View(hangHoa); 
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Không ghi được dữ liệu. {ex.Message}");
            }
        }
        #endregion

        #region Upload hình ảnh
        // GET: UploadFile/Upload/1
        [Route("upload-hinh-hang-hoa/{id?}")]
        public async Task<ActionResult> Upload(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"hang-hoa/doc-chi-tiet/{id}";
                var item = await ApiHelper<HangHoaOutPut>.RunGetAsync(url);
                if (item == null) throw new Exception("ID:" + id + "không tồn tại");
                return View(item);
            }
            catch (Exception ex)
            {
                object cauBaoLoi = "không truy cập được dữ liệu <br/>" + ex.Message;
                return View("BaoLoi", cauBaoLoi);
            }
        }
        // POST: UploadFile/Upload/1
        [HttpPost]
        [ValidateAntiForgeryToken]

        ////cách 1: Lưu 1 hình
        //public async Task<ActionResult> Upload(int? id, HttpPostedFileBase tapTins)
        //{
        //    try
        //    {
        //        string url1 = $"hang-hoa/doc-chi-tiet/{id}";
        //        var item = await ApiHelper<HangHoaOutPut>.RunGetAsync(url1);
        //        if (tapTins != null)
        //        {
        //            using (var client = new HttpClient())
        //            {
        //                byte[] b = new byte[tapTins.ContentLength];
        //                tapTins.InputStream.Read(b, 0, tapTins.ContentLength);
        //                var fileContent = new ByteArrayContent(b);
        //                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //                {
        //                    FileName = tapTins.FileName
        //                };
        //                var content = new MultipartFormDataContent();
        //                content.Add(fileContent);

        //                string apiURL = ConfigurationManager.AppSettings["apiURL"];
        //                client.BaseAddress = new Uri(apiURL);
        //                client.DefaultRequestHeaders.Accept.Clear();
        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                string url = $"admin-hang-hoa/upload-hinh-anh/{id}";
        //                var response = await client.PostAsync(url, content);
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    ViewBag.ThongBao = "Upload Thành công";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.ThongBao = "Bạn chưa chọn file hoặc file bạn chọn không có nội dung.";
        //        }

        //        return View(item);
        //    }
        //    catch (Exception ex)
        //    {
        //        object cauBaoLoi = "Upload không thành công <br/>" + ex.Message;
        //        return View("BaoLoi", cauBaoLoi);
        //    }
        //}

        //Cách 2: Lưu nhiều hình
        public async Task<ActionResult> Upload(int id, HttpPostedFileBase[] tapTins)
        {
            try
            {
                string url = $"admin-hang-hoa/doc-chi-tiet/{id}";
                var item = await ApiHelper<HangHoaOutPut>.RunGetAsync(url);
                if (item == null) return RedirectToAction("Index");

                if (tapTins[0] != null)
                {
                    using (var client = new HttpClient())
                    {
                        var content = new MultipartFormDataContent();
                        foreach (var tapTin in tapTins)
                        {                       
                            HttpPostedFileBase f = tapTin;
                            byte[] b = new byte[f.ContentLength];
                            f.InputStream.Read(b, 0, f.ContentLength);
                            var fileContent = new ByteArrayContent(b);
                            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = f.FileName
                            };
                            
                            content.Add(fileContent);     
                        }
                        string apiURL = ConfigurationManager.AppSettings["apiURL"];
                        client.BaseAddress = new Uri(apiURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string url1 = $"admin-hang-hoa/upload-hinh-anh/{id}";
                        var response = await client.PostAsync(url1, content);
                    }
                    ViewBag.ThongBao = "upload thành công";
                    return View(item);
                }
                // Trường hợp chưa chọn file hoặc file không có nội dung thì quay trở lại view upload
                ViewBag.ThongBao = "Bạn chưa chọn file hoặc file bạn chọn không có nội dung.";
                return View(item);
            }
            catch (Exception ex)
            {
                object cauBaoLoi = "Upload không thành công <br/>" + ex.Message;
                return View("BaoLoi", cauBaoLoi);
            }
        }
        #endregion

        #region Sửa hàng hóa
        // GET: AdminHangHoa/Edit/5
        [Route("sua-hang-hoa/{id?}")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"admin-hang-hoa/doc-chi-tiet/{id}";
                var item = await ApiHelper<HangHoaInPut>.RunGetAsync(url);
                if (item == null) throw new Exception($"Hàng hóa ID={id} không tồn tại.");

                ViewBag.LoaiID = await TaoDanhSachLoai(item.LoaiID);
                return View(item);
            }
            catch(Exception ex)
            {
                return View("BaoLoi", $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}");
            }
            
        }

        // POST: AdminHangHoa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HangHoaInPut input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url1 = "admin-hang-hoa/sua-hang-hoa";
                    await ApiHelper<HangHoaInPut>.RunPostAsync(url1, input);
                    return RedirectToAction("Index");
                }
                string url = $"hang-hoa/doc-chi-tiet/{input.ID}";
                var item = await ApiHelper<HangHoaInPut>.RunGetAsync(url);          
                ViewBag.LoaiID = await TaoDanhSachLoai(input.LoaiID);
                return View(item);
            }
            catch (Exception ex)
            {
                return View("BaoLoi", model: $"Không ghi được dữ liệu. <br/> Lý do:{ex.Message}");
            }
        }
        #endregion

        #region Xóa hàng hóa
        // GET: AdminHangHoa/Delete/5
        [Route("xoa-hang-hoa/{id?}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return RedirectToAction("Index");
            try
            {
                string url = $"admin-hang-hoa/doc-chi-tiet/{id}";
                var hangHoa = await ApiHelper<HangHoaOutPut>.RunGetAsync(url);
                if (hangHoa == null) throw new Exception($"Hàng hóa ID={id} không tồn tại.");
                return View(hangHoa);
            }
            catch(Exception ex)
            {
                return View("BaoLoi", $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}");
            }
            
        }

        // POST: AdminHangHoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                string url = $"admin-hang-hoa/xoa-hang-hoa/{id}";
                string result = await ApiHelper<string>.RunPostAsync(url);
                //TempData["SuccessMsg"] = result;
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View("BaoLoi", $"Lỗi truy cập dữ liệu.<br/>lý do: {ex.Message}");
            }
        }
        #endregion
    }
}
