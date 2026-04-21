using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class AIChatSessionResponse
    {
        public int AIChatId { get; set; }
        public int ResultId { get; set; }

        // what test this AI session is about
        // helps frontend show context like "Asking about: CBC Test"
        public string TestName { get; set; }
        public string PatientName { get; set; }

        public DateTime StartedAt { get; set; }

        // all messages in order
        public List<AIChatMessageResponse> Messages { get; set; } = new List<AIChatMessageResponse>();

        public int TotalMessages { get; set; }
    }
}
