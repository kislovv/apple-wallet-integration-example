using AppleWalletPassWithApnsIntegration.Models;
using AutoMapper;
using BL.Dtos;
using BL.Entities;
using BL.Services;

namespace AppleWalletPassWithApnsIntegration.Configurations;

/// <inheritdoc />
public class ApiRequestMapperProfile :Profile
{
    /// <inheritdoc />
    public ApiRequestMapperProfile()
    {
        CreateMap<PassRequest, PassDto>();

        CreateMap<UserRegistrationRequest, User>(MemberList.Source)
            .ForMember(user => user.Role, opt =>
            {
                opt.MapFrom(request => Roles.User);
            });
        CreateMap<LoginRequest, User>(MemberList.Source);
        CreateMap<CreateCardRequest, CardDto>(MemberList.Source);
    }
}