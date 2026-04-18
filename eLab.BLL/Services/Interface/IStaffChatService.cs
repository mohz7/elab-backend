using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface IStaffChatService
    {
        Task<ServiceResult<StaffChatSessionResponse>> CreateSessionAsync(int bookingId, string patientId);
        Task<ServiceResult<StaffChatSessionResponse>> GetSessionAsync(int chatId, string requestingUserId);
        Task<ServiceResult<StaffChatMessageResponse>> SendMessageAsync(int chatId, string senderId, string message);
        Task<ServiceResult<string>> MarkAsReadAsync(int chatId, string readerId);
        Task<ServiceResult<List<StaffChatSessionResponse>>> GetByPatientIdAsync(string patientId);
        Task<ServiceResult<List<StaffChatSessionResponse>>> GetByStaffIdAsync(string staffId);


    }
}
