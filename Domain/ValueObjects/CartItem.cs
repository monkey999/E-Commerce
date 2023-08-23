using System;

namespace A_Domain.ValueObjects
{
    public class CartItem
    {
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
    }
}
