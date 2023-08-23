using System;

namespace A_Domain.Models
{
    public class OrderStock
    {
        //public Guid OrderId { get; set; }
        //public Order Order { get; set; }
        
        public Guid StockId { get; set; }
        public Stock Stock { get; set; }

        public int Quantity { get; set; }
    }
}
