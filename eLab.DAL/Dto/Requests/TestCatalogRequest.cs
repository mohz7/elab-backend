using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class TestCatalogRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SampleType { get; set; }
        public int TurnaroundHours { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
