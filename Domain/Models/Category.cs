using System;
using System.Collections.Generic;

namespace A_Domain.Models
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
