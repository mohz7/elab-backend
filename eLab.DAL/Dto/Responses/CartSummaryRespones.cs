using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class CartSummaryRespones
    {
        public List<CartResponse> Items { get; set; } = new List<CartResponse>();
        public decimal CartTotal => Items.Sum(i => i.Price);
    }
}
