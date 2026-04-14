using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class BookingItem
    {
         public int Id { get; set; }
         public decimal UnitPrice { get; set; } 
         public decimal FinalPrice { get; set; }


        // Navigation properties

            // Booking
            public int BookingId { get; set; }
            public Booking? Booking { get; set; }

            // TestCatalog
            public int TestCatalogId { get; set; }
            public TestCatalog? TestCatalog { get; set; }

            // Offer
            public int? OfferId { get; set; }
            public Offer Offer { get; set; }

            // Result
            public Result Results { get; set; }
    }
}
