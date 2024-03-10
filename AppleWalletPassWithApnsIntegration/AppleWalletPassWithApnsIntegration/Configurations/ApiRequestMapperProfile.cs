using AppleWalletPassWithApnsIntegration.Models;
using AutoMapper;
using BL.Dtos;

namespace AppleWalletPassWithApnsIntegration.Configurations;

public class ApiRequestMapperProfile :Profile
{
    public ApiRequestMapperProfile()
    {
        CreateMap<PassRequest, PassDto>().ReverseMap();
    }
}