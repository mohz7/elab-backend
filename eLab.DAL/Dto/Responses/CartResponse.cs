using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class CartResponse
    {
        public int Id { get; set; }
        public int TestCatalogId { get; set; }
        public string TestCatalogName { get; set; }
        public decimal Price { get; set; }
    }
}
