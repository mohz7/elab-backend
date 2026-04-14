using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class Price
    {
        public int Id { get; set; }
        public decimal BasePrice { get; set; }
        public string Currency { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }


        // Navigation properties

            //TestCatalog
            public int TestCatalogId { get; set; }
            public TestCatalog TestCatalog { get; set; }

            // Branch
            public int BranchId { get; set; }
            public Branch Branch { get; set; }

            // User
            public string CreatedById { get; set; }
            public User CreatedBy { get; set; }
    }
}
