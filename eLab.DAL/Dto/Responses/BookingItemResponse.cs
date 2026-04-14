using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class BookingItemResponse
    {
        public int Id { get; set; }

        public string TestCatalog { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal FinalPrice { get; set; }

        public string? Offer { get; set; }

    }
}
