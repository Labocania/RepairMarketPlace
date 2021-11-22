using RepairMarketPlace.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RepairMarketPlace.ApplicationCore.Entities
{
    public class ComponentType : BaseEntity, IAggregateRoot
    {
        [Required]
        public string Name { get; set; }

        //-----------------------------------------------
        // Relationships
        private HashSet<Component> _components;
        public IReadOnlyCollection<Component> Components => _components?.ToList();

        private readonly List<ServiceType> _serviceTypes;
        IReadOnlyCollection<ServiceType> ServiceTypes => _serviceTypes?.AsReadOnly();
    }
}
