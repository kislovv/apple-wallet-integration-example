using AppleWalletPassWithApnsIntegration.Models;
using AutoMapper;
using BL.Dtos;

namespace AppleWalletPassWithApnsIntegration.Configurations;

public class ApiRequestMapperProfile :Profile
{
    public ApiRequestMapperProfile()
    {
        CreateMap<PassRequest, PassDto>()
            .ReverseMap()
            .ForMember(request => request.DeviceName, opt => 
                opt.MapFrom(dto =>dto.Device));
    }
}