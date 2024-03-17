using BL.Entities;

namespace BL.Abstractions;

public interface IUserRepository
{
    Task<User> Add(User user);
    Task<User?> GetByLogin(string login);
}