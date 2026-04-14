using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class ReferenceRange
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public int AgeMin { get; set; }
        public int AgeMax { get; set; }
        public Gender Gender { get; set; }
        public decimal ValueMin { get; set; }
        public decimal ValueMax { get; set; }
        public string Units { get; set; }
        public string Notes { get; set; }



        // Navigation properties

            // ReportTemplate
            public int ReportTemplateId { get; set; }
            public ReportTemplate ReportTemplate { get; set; }

            // User
            public string? CreatedById { get; set; }
            public User CreatedBy { get; set; }
    }
}
