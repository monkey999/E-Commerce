using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DataAccess.Contracts.V1.DTO_responses.CREATE
{
    public class CreateCartItemResponseDTO
    {
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
    }
}
