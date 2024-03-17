using BL.Entities;

namespace BL.Abstractions;

public interface IUserService
{
    Task<User> AddUser(User user);
    Task<User> LoginUser(User user);
}