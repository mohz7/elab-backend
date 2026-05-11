using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class PriceResponse
    {
        public int Id { get; set; }
        public decimal BasePrice { get; set; }
        public string Currency { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public int BranchId { get; set; }
        public int TestCatalogId { get; set; }


    }
}
