using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLBHTraiCayMVC_View.DTO
{
    public class LoaiInput
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Mã loại")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [MaxLength(10, ErrorMessage = "{0} tối đa là {1} ký tự.")]
        public string MaLoai { get; set; }

        [Display(Name = "Tên loại")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [MaxLength(100, ErrorMessage = "{0} phải nhập tối đa là {1} ký tự.")]
        public string TenLoai { get; set; }

        [Display(Name = "Chủng loại")]
        public int ChungLoaiID { get; set; }
    }
    public class LoaiOutput
    {
        public int ID { get; set; }
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public int ChungLoaiID { get; set; }
        public ChungLoaiOutput ChungLoai { get; set; }
    }
}