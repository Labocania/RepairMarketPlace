using RepairMarketPlace.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairMarketPlace.ApplicationCore.Entities
{
    public class ComponentType : BaseEntity, IAggregateRoot
    {
        [Required]
        public ComponentTypeName Name { get; set; }
        public ICollection<Component> Components { get; set; }
    }

    public enum ComponentTypeName
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
