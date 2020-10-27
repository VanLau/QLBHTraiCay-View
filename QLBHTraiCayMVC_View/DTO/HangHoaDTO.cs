using QLBHTraiCayMVC_View.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLBHTraiCayMVC_View.DTO
{
    public class HangHoaOutPut
    {
        public int ID { get; set; }
        [Display(Name = "Mã hàng")]
        public string MaHang { get; set; }
        [Display(Name = "Tên hàng")]
        public string TenHang { get; set; }
        [Display(Name = "Đơn vị tính")]
        public string DVT { get; set; }
        [Display(Name = "Quy cách")]
        public string QuyCach { get; set; }
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }
        [Display(Name = "Giá bán")]
        public int GiaBan { get; set; }
        [Display(Name = "Giá thị trường")]
        public int GiaThiTruong { get; set; }
        [Display(Name = "Loại")]
        public int LoaiID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime NgayTao { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime NgaySua { get; set; }

        [Display(Name = "Xuất xứ")]
        public string XuatXu { get; set; }
        [Display(Name = "Tình Trạng")]
        public int TinhTrang { get; set; }
        
        public LoaiOutput loai { get; set; }
        public List<string> HinhURLs { get; set; }

        public int Gia1
        {
            get
            {
                int gia1 = 0;
                if (GiaThiTruong != 0)
                {
                    gia1 = GiaThiTruong;
                }
                else
                {
                    gia1 = 0;
                }
                return gia1;
            }
            set { }
        }

        public string BiDanh
        {
            get
            {
                string db = XuLyChuoi.LoaiBoDauTiengViet(TenHang);
                return db;
            }
        }
    }

    public class HangHoaInPut
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Mã hàng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [MaxLength(10, ErrorMessage = "{0} tối đa là {1} ký tự")]
        [MinLength(2, ErrorMessage = "{0} tối thiểu là {1} ký tự")]
        public string MaHang { get; set; }

        [Display(Name = "Tên hàng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [MaxLength(100, ErrorMessage = "{0} tối đa là {1} ký tự")]
        public string TenHang { get; set; }


        [Display(Name = "Đơn vị tính")]
        [MaxLength(20, ErrorMessage = "{0} tối đa là {1} ký tự")]
        public string DVT { get; set; }

        [Display(Name = "Quy cách")]
        [MaxLength(50, ErrorMessage = "{0} tối đa là {1} ký tự")]
        public string QuyCach { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [Display(Name = "Giá bán")]
        [DisplayFormat(DataFormatString = "{0:#,##0VND}")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [RegularExpression(@"\d*", ErrorMessage = "{0} Phải nhập số nguyên >=0.")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} phải từ {1} đến {2}")]
        public int GiaBan { get; set; }

        [Display(Name = "Giá thị trường")]
        [DisplayFormat(DataFormatString = "{0:#,##0VND}")]
        [RegularExpression(@"\d*", ErrorMessage = "{0} Phải nhập số nguyên >=0.")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} phải từ {1} đến {2}")]
        public Nullable<int> GiaThiTruong { get; set; }

        [Display(Name = "Loại")]
        [Range(1, int.MaxValue, ErrorMessage = "Phải chọn {0} cho mặt hàng.")]
        [RegularExpression(@"\d*", ErrorMessage = "{0} phải là số nguyên")]
        public int LoaiID { get; set; }       

        [Display(Name = "Xuất xứ")]
        public string XuatXu { get; set; }

        [Display(Name = "Tình Trạng")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [RegularExpression(@"\d*", ErrorMessage = "{0} phải là số nguyên")]
        public int TinhTrang { get; set; } 
    }

    public class HangHoaOutputBS
    {
        public int ID { get; set; }
        public string MaHang { get; set; }
        public string TenHang { get; set; }
        public string DVT { get; set; }
        public string QuyCach { get; set; }
        public string MoTa { get; set; }
        public string TenHinh { get; set; }
        public int GiaBan { get; set; }
        public Nullable<int> GiaThiTruong { get; set; }
        public string XuatXu { get; set; }
        public int TinhTrang { get; set; }

        public List<string> HinhURLs { get; set; }

        public int SoLuong { get; set; }

        public string BiDanh
        {
            get
            {
                string db = XuLyChuoi.LoaiBoDauTiengViet(TenHang);
                return db;
            }
        }
    }
}