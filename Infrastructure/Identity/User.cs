using Microsoft.AspNetCore.Identity;
using RepairMarketPlace.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepairMarketPlace.Infrastructure.Identity
{
    public class User : IdentityUser
    {
        [PersonalData]
        [MaxLength(70)]
        public string Name { get; set; }
        [PersonalData]
        public DateTime Birthday { get; set; }
    }
}
