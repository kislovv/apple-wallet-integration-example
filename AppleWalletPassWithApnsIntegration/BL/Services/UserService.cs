using BL.Abstractions;
using BL.Entities;
using BL.Exceptions;

namespace BL.Services;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork): IUserService
{
    public async Task<User> AddUser(User user)
    {
        user.Password = PasswordHasher.Hash(user.Password);
        
        var result = await userRepository.Add(user);
        await unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<User> LoginUser(User inputUser)
    {
        var user = await userRepository.GetByLogin(inputUser.Login);
        if (user is null)
        {
            throw new BusinessException("User not found by login!");
        }

        if (PasswordHasher.Verify(inputUser.Password, user.Password))
        {
            return user;
        }

        throw new BusinessException("Password incorrect");
    }
    
}