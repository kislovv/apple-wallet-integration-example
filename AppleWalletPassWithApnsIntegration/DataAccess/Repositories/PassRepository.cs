﻿using BL.Abstractions;
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

    public void Delete(AppleWalletPass appleWalletPass)
    {
        appContext.AppleWalletPasses.Remove(appleWalletPass);
    }

    public async Task<List<AppleWalletPass>> GetLastUpdatedPassesByDeviceId(string deviceId, DateTimeOffset updatedBefore)
    {
        var device = await appContext.AppleDevices
            .Include(d => d.AppleWalletPasses)
            .SingleAsync(d => d.Id == deviceId);
        
        return device.AppleWalletPasses.Where(p => p.LastUpdated < updatedBefore).ToList();
    }

    public Task<AppleWalletPass> GetPassBySerialNumber(string serialNumber)
    {
        return appContext.AppleWalletPasses
            .Include(p => p.AppleDevices)
            .SingleAsync(p => p.PassId == serialNumber);
    }
}