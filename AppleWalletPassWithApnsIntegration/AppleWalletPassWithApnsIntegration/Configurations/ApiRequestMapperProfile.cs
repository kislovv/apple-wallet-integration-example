using AppleWalletPassWithApnsIntegration.Models;
using AutoMapper;
using BL.Dtos;
using BL.Entities;
using BL.Services;

namespace AppleWalletPassWithApnsIntegration.Configurations;

public class ApiRequestMapperProfile :Profile
{
    public ApiRequestMapperProfile()
    {
        CreateMap<PassRequest, PassDto>()
            .ReverseMap()
            .ForMember(request => request.DeviceName, opt => 
                opt.MapFrom(dto => dto.Device));

        CreateMap<UserRegistrationRequest, User>(MemberList.Source)
            .ForMember(user => user.Role, opt =>
            {
                opt.MapFrom(request => Roles.User);
            });
        CreateMap<LoginRequest, User>(MemberList.Source);
    }
}