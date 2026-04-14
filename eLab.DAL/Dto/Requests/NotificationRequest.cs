using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class NotificationRequest
    {
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public int? ResultId { get; set; }
    }
}

