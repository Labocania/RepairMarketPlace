using RepairMarketPlace.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RepairMarketPlace.ApplicationCore.Entities
{
    public class Component : BaseEntity, IAggregateRoot
    {
        [Required]
        [MaxLength(256)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        // Relationships
        [JsonIgnore]
        [Required]
        public ComponentType Type { get; set; }
        public int ComponentTypeId { get; set; }
    }
}
