using BL.Entities;

namespace BL.Abstractions;

public interface IUserRepository
{
    Task<User> Add(User user);
    Task<bool> IsExistWithLogin(string login);
    Task<User?> GetByLogin(string login);
}