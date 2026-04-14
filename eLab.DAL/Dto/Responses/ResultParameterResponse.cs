using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class ResultParameterResponse
    {
        public string ParameterName { get; set; }
        public decimal Value { get; set; }
        public ResultFlags Flag { get; set; }        
        public decimal? RangeMin { get; set; }
        public decimal? RangeMax { get; set; }
        public string? Unit { get; set; }
    }
}
