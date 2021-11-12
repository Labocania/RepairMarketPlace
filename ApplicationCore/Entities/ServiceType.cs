using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepairMarketPlace.ApplicationCore.Entities
{
    public class ServiceType : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; private set; }
        public string Description { get; private set; }

        public ServiceType(string name, string description, int shopId)
        {
            Name = name;
            Description = description;
            ShopId = shopId;
        }

        //-----------------------------------------------
        // Relationships
        public int ShopId { get; private set; }
        public Shop Shop { get; set; }
        ICollection<ComponentType> ComponentTypes { get; set; }
    }
}