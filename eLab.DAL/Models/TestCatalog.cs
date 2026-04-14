using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class TestCatalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SampleType { get; set; }
        public int TurnaroundHours { get; set; }
        public bool IsActive { get; set; }


        // Navigation properties

            // User
            public string CreatedById { get; set; }
            public User? CreatedBy { get; set; }


        // Reverse Navigation

            // Price
            public ICollection<Price> Prices { get; set; } = new List<Price>();

            // Offer
            public ICollection<Offer>? Offers { get; set; } = new List<Offer>();

            // BookingItem
            public ICollection<BookingItem>? BookingItems { get; set; } = new List<BookingItem>();

            // ReportTemplate
            public ICollection<ReportTemplate> ReportTemplates { get; set; } = new List<ReportTemplate>();

    }
}
