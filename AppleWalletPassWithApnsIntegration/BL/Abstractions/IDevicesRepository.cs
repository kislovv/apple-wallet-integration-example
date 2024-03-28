using BL.Entities;

namespace BL.Abstractions;

public interface IDevicesRepository
{
    Task<AppleDevice> AddDevice(AppleDevice device);
    Task<AppleDevice?> GetDeviceById(string id);
}