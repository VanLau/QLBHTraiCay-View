using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLBHTraiCayMVC_View.DTO
{
    public class ChungLoaiInput
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Mã loại")]
        [Required(ErrorMessage = "{0} không được để trống.")]
        [MaxLength(10, ErrorMessage = "{0} tối đa là {1} ký tự.")]
        public string MaCL { get; set; }

        [Display(Name = "Tên loại")]
        [Required(ErrorMessage = "{0} không được để trống")]
        [MaxLength(100, ErrorMessage = "{0} phải nhập tối đa là {1} ký tự.")]
        public string TenCL { get; set; }
    }

    public class ChungLoaiOutput
    {
        public int ID { get; set; }
        public string MaCL { get; set; }
        public string TenCL { get; set; }
        public List<LoaiOutput> loais { get; set; }
    }
}