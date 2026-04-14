using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class AIChat
    {
        public int Id { get; set; }
        public string ContextSummary { get; set; }
        public DateTime StartedAt { get; set; }


        // Navigation properties

            // PatientProfile
            public string? PatientProfileId { get; set; }
            public PatientProfile PatientProfile { get; set; }

            // Result
            public int? ResultId { get; set; }
            public Result Result { get; set; }
        

        // Reverse Navigation

            // AIChatMessage
            public ICollection<AIChatMessage> AIChatMessages { get; set; } = new List<AIChatMessage>();
    }
}
