using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }


        // Navigation properties

            // TestCatalog
            public int TestCatalogId { get; set; }
            public TestCatalog? TestCatalog { get; set; }

            // Branch
            public int BranchId { get; set; }
            public Branch? Branch { get; set; }

            // User
            public string CreatedById { get; set; }
            public User? CreatedBy { get; set; }


        // Reverse Navigation

            // BookingItem
            public ICollection<BookingItem>? BookingItems { get; set; } = new List<BookingItem>();
    }
}
