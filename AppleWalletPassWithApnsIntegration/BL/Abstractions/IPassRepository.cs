using BL.Entities;

namespace BL.Abstractions;

public interface IPassRepository
{
    Task<AppleWalletPass> CreatePass(AppleWalletPass appleWalletPass);
}