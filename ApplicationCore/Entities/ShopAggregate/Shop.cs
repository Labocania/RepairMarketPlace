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
        public Guid UserId { get; private set; }
        [Required]
        [MaxLength(200)]
        [DataType(DataType.Text)]
        [Display(Name = "Shop's Name")]
        public string Name { get; private set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Shop's Address")]
        public string Address { get; private set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Shop's Email")]
        public string Email { get; private set; }
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Shop's Phone Number")]
        public string PhoneNumber { get; private set; }
        [Url]
        [DataType(DataType.Url)]
        [Display(Name = "Shop's Website")]
        public string WebSite { get; private set; }

        [Display(Name = "Shop's Status")]
        public bool IsOpen { get; private set; }

        public Shop(Guid userId, string name, string address, string email, string phoneNumber, string webSite)
        {
            UserId = userId;
            Name = name;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            WebSite = webSite;
        }

        public void UpdateShopProfile(string name, string address, string webSite, bool isOpen)
        {
            if (name != Name) Name = name;
            if (address != Address) Address = address;
            if (webSite != WebSite) WebSite = webSite;
            if (isOpen != IsOpen) IsOpen = isOpen;
        }

        public void AddServiceType(string name, string description)
        {
            _serviceTypes.Add(new ServiceType(Id, name, description));
        }

        //-----------------------------------------------
        // Relationships
        private readonly List<ServiceType> _serviceTypes;
        public IReadOnlyCollection<ServiceType> ServiceTypes => _serviceTypes?.AsReadOnly();
    }
}
