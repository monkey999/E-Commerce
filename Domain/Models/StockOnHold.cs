using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Domain.Models
{
    public class StockOnHold
    {
        [Key]
        public Guid StockOnHoldId { get; set; }

        public Guid StockId { get; set; }
        [ForeignKey(nameof(StockId))]
        public Stock Stock { get; set; }

        public string SessionId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
