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
        public string Name { get; set; }

        [JsonIgnore]
        [Required]
        public ComponentType Type { get; set; }
    }

    public enum ComponentType
    {
        WirelessNetworkCard,
        WiredNetworkCard,
        Webcam,
        VideoCard,
        UPS,
        ThermalPaste,
        Speakers,
        SoundCard,
        PowerSupply,
        OpticalDrive,
        Mouse,
        Motherboard,
        Monitor,
        Memory,
        Laptop,
        Keyboard,
        InternalHardDrive,
        Headphones,
        FanController,
        ExternalHardDrive,
        CPU,
        CPUCooler,
        Case,
        CaseFan,
        CaseAccessory
    }
}
