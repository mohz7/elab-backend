using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class ReportTemplateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<FieldDefinition> FieldsSchema { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }

        public class FieldDefinition
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Unit { get; set; }
        }
    }
}
