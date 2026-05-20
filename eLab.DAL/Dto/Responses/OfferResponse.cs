using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class OfferResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BranchId { get; set; }
        public int TestCatalogId { get; set; }
        public bool IsActive { get; set; }



    }
}
