using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.MapsterConfigurations.MapsterStaffProfile
{
    public class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<StaffProfile, StaffProfilesResponse>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id.ToString())
                .Map(dest => dest.FullName, src => src.User.FullName)
                .Map(dest => dest.IdentityNumber, src => src.User.IdentityNumber)
                .Map(dest => dest.Gender, src => src.User.Gender)
                .Map(dest => dest.DateOfBirth, src => src.User.DateOfBirth)
                .Map(dest => dest.JobTitle, src => src.JobTitle)
                .Map(dest => dest.HiredAt, src => src.HiredAt)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy.FullName)
                .Map(dest => dest.BranchName, src => src.Branch.Name);
        }
    }
}
