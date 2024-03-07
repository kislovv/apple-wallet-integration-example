using BL.Dtos;

namespace BL.Abstractions;

public interface IPassService
{
    Task<byte[]> CreatePass(PassDto passDto);
}