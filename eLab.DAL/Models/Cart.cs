using Microsoft.EntityFrameworkCore;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int CountItems { get; set; }


        // User
        public string UserId { get; set; }
        public User User { get; set; }

        // TestCatalog
        public int TestCatalogId { get; set; }
        public TestCatalog TestCatalog { get; set; }
    }
}
