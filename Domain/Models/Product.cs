using System;
using System.Collections.Generic;

namespace A_Domain.Models
{
    public class Product
    {
        public Product()
        {
            Stock = new HashSet<Stock>();
        }

        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Stock> Stock { get; set; }
    }
}
