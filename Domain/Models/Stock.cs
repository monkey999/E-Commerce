using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Domain.Models
{
    public class Stock
    {
        public Stock()
        {
            OrderStocks = new HashSet<OrderStock>();
        }

        [Key]
        public Guid StockId { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public string Description { get; set; }
        public int Quantity { get; set; }

        public virtual ICollection<OrderStock> OrderStocks { get; set; }
    }
}
