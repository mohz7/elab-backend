using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class AIChatSessionResponse
    {
        public int Id { get; set; }
        public DateTime StartedAt { get; set; }
        public string? PatientProfileId { get; set; }
        public int? ResultId { get; set; }
        public ResultSummaryResponse Result { get; set; }
        public List<AIChatMessageResponse> AIChatMessages { get; set; } = new();
    }
}
