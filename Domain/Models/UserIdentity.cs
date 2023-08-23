using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace A_Domain.Models
{
    public class UserIdentity : IdentityUser
    {
        public UserIdentity()
        {
            Orders = new HashSet<Order>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; } //move to customerinfo session 

        public virtual ICollection<Order> Orders { get; set; }
    }
}
