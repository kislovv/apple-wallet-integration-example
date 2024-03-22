using BL.Entities;

namespace BL.Abstractions;

public interface IPassRepository
{
    Task<AppleWalletPass> CreatePass(AppleWalletPass appleWalletPass);
    void UpdatePass(AppleWalletPass appleWalletPass);
    void Delete(AppleWalletPass appleWalletPass);
    Task<List<AppleWalletPass>> GetLastUpdatedPassesByDeviceId(string deviceId, DateTimeOffset updatedBefore);
    Task<AppleWalletPass> GetPassBySerialNumber(string serialNumber);
}