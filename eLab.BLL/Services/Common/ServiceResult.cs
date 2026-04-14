using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ServiceResult<T> Ok(T data) =>
            new() { Success = true, StatusCode = 200, Data = data };

        public static ServiceResult<T> Fail(int statusCode, string errorCode, string message) =>
            new() { Success = false, StatusCode = statusCode, ErrorCode = errorCode, Message = message };
    }
}
