using BL.Abstractions;
using BL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class PassRepository(AppDbContext appContext) : IPassRepository
{
    public async Task<AppleWalletPass> CreatePass(AppleWalletPass appleWalletPass)
    {
        var result = await appContext.AppleWalletPasses.AddAsync(appleWalletPass);
        
        return result.Entity;
    }

    public void UpdatePass(AppleWalletPass appleWalletPass)
    {
        appContext.AppleWalletPasses.Update(appleWalletPass);
    }

    public Task<AppleWalletPass> GetPassBySerialNumber(string serialNumber)
    {
        return appContext.AppleWalletPasses
            .Include(p => p.AppleDevices)
            .SingleAsync(p => p.PassId == serialNumber);
    }
}