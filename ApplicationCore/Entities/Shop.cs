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
        public string Name { get; private set; }
        [Required]
        public string Address { get; private set; }
        [Required]
        [EmailAddress]
        public string Email { get; private set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; private set; }
        public string WebSite { get; private set; }
        public bool IsOpen { get; private set; }

        public Shop(Guid userId, string name, string address, string email, string phoneNumber, string webSite, bool isOpen)
        {
            UserId = userId;
            Name = name;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            WebSite = webSite;
            IsOpen = isOpen;
        }

        //-----------------------------------------------
        // Relationships
        public ICollection<ServiceType> ServiceTypes { get; set; }
    }
}
