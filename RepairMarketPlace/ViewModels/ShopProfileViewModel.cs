using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class ShopProfileViewModel
    {
        public Guid UserId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Shop's Name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Shop's Address")]
        public string Address { get; set; }

        [Url]
        [DataType(DataType.Url)]
        [Display(Name = "Shop's Website")]
        public string WebSite { get; set; }

        [Display(Name = "Shop's Status")]
        public bool IsOpen { get; set; }
    }
}
