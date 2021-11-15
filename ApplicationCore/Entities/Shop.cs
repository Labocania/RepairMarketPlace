using RepairMarketPlace.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairMarketPlace.ApplicationCore.Entities
{
    public class Shop : BaseEntity, IAggregateRoot, IUserId
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [MaxLength(200)]
        [DataType(DataType.Text)]
        [Display(Name = "Shop's Name")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Shop's Address")]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Shop's Email")]
        public string Email { get; set; }
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Shop's Phone Number")]
        public string PhoneNumber { get; set; }
        [Url]
        [DataType(DataType.Url)]
        [Display(Name = "Shop's Website")]
        public string WebSite { get; set; }

        [Display(Name = "Shop's Status")]
        public bool IsOpen { get; set; }

        public Shop(Guid userId, string name, string address, string email, string phoneNumber, string webSite)
        {
            UserId = userId;
            Name = name;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            WebSite = webSite;
        }


        //-----------------------------------------------
        // Relationships
        public ICollection<ServiceType> ServiceTypes { get; set; }
    }
}
