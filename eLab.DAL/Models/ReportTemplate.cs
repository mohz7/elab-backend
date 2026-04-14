using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class ReportTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FieldsSchema { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



        // Navigation properties

            // TestCatalog
            public int TestCatalogId { get; set; }
            public TestCatalog? TestCatalog { get; set; }

            // User
            public string? CreatedById { get; set; }
            public User? CreatedBy { get; set; }


        // Reverse Navigation

            // Result
            public ICollection<Result> Results { get; set; } = new List<Result>();

            // ReferenceRange
            public ICollection<ReferenceRange> ReferenceRanges { get; set; } = new List<ReferenceRange>();
    }
}
