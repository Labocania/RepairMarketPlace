using Microsoft.AspNetCore.Identity;
using System;
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
