using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class ReferenceRangeRequest
    {
        public string FieldName { get; set; }
        public int AgeMin { get; set; }
        public int AgeMax { get; set; }
        public Gender Gender { get; set; }
        public decimal ValueMin { get; set; }
        public decimal ValueMax { get; set; }
        public string Units { get; set; }
        public string Notes { get; set; }
        public int ReportTemplateId { get; set; }
    }
}
