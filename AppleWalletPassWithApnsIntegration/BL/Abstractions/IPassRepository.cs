using BL.Entities;

namespace BL.Abstractions;

public interface IPassRepository
{
    Task<AppleWalletPass> CreatePass(AppleWalletPass appleWalletPass);
    void UpdatePass(AppleWalletPass appleWalletPass);
    Task<AppleWalletPass> GetPassBySerialNumber(string serialNumber);
}