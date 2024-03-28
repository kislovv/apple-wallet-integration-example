using BL.Abstractions;
using BL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class DeviceRepository(AppDbContext dbContext): IDevicesRepository
{
    public async Task<AppleDevice> AddDevice(AppleDevice device)
    {
        var result = await dbContext.AppleDevices.AddAsync(device);
        return result.Entity;
    }

    public Task<AppleDevice?> GetDeviceById(string id)
    {
        return dbContext.AppleDevices.SingleOrDefaultAsync(device => device.Id == id);
    }
}