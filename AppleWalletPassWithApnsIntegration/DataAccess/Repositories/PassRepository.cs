using BL.Abstractions;
using BL.Entities;

namespace DataAccess.Repositories;

public class PassRepository(AppDbContext appContext) : IPassRepository
{
    public async Task<AppleWalletPass> CreatePass(AppleWalletPass appleWalletPass)
    {
        var result = await appContext.AppleWalletPasses.AddAsync(appleWalletPass);
        
        return result.Entity;
    }
}