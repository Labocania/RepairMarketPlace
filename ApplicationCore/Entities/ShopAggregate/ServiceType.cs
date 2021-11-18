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

        internal ServiceType(int shopId, string name, string description)
        {
            ShopId = shopId;
            Name = name;
            Description = description;
        }

        public void AddComponentType(ComponentType componentType)
        {
            _componentTypes.Add(componentType);
        }

        //-----------------------------------------------
        // Relationships
        public int ShopId { get; private set; }
        public Shop Shop { get; set; }

        private readonly List<ComponentType> _componentTypes = new();
        IReadOnlyCollection<ComponentType> ComponentTypes => _componentTypes.AsReadOnly();
    }
}