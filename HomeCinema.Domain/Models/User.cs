using HomeCinema.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.Domain
{
   public class User:IdentityUser
    {
        public string FullName { get; set; }
        public IEnumerable<UserActions> UserActions { get; set; }
    }
}
