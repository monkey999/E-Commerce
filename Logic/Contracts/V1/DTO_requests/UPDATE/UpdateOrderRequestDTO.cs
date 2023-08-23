using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B_DataAccess.Contracts.V1.DTO_requests.UPDATE
{
    public class UpdateOrderRequestDTO
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
