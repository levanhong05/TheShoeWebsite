using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TheShoeWebsite.Models
{
    public class SendOrderModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ tên đầy đủ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Địa chỉ người nhận")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập điện thoại liên lạc")]
        [Display(Name = "Điện thoại liên lạc")]
        public string PhoneNumber { get; set; }

    }
}