using AutoMapper;
using finanzas_user_service.Data.Entities;
using finanzas_user_service.DTOs;
using finanzas_user_service.Enums;

namespace finanzas_user_service.Utilities;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<RegisterUserDTO, User>()
            .ForMember(dest => dest.RoleId, 
                opt => opt.MapFrom(src => RolesExtensions.GetNumeralRoleByEmail(src.Email)));

        CreateMap<User, GetUserDTO>()
            .ForMember(dest => dest.Id, 
                opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Role, 
                opt => opt.MapFrom(src => Enum.GetName(typeof(Roles), src.RoleId)))
            .ForMember(dest => dest.CreatedAt, 
                opt => opt.MapFrom(src => DateConvertionUtility.GetUnixTimestampMilliseconds(src.CreatedAt)))
            .ForMember(dest => dest.UpdatedAt, 
                opt => opt.MapFrom(src => DateConvertionUtility.GetUnixTimestampMilliseconds(src.UpdatedAt)));
    }
}