using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class ReportTemplateUpdateRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Version { get; set; }
        public int? TestCatalogId { get; set; }
        public List<FieldDefinition> Fields { get; set; }

        public class FieldDefinition
        {
            public string? Name { get; set; }
            public string? Type { get; set; }
            public string? Unit { get; set; }
        }
    }
}
