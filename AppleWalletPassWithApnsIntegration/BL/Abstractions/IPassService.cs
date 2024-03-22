using BL.Dtos;

namespace BL.Abstractions;

public interface IPassService
{
    Task<byte[]> CreatePass(PassDto passDto);

    Task RegisterPass(RegisteredPassDto passDto);
    Task UnregisterPass(UnregisterPassDto passDto);
    Task<LastUpdatedPassesDto?> GetLastUpdatedPasses(string deviceId, DateTimeOffset updatedBefore);
}