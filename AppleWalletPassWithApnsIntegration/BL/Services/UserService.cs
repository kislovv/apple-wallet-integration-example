using BL.Abstractions;
using BL.Entities;
using BL.Exceptions;

namespace BL.Services;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IParticipantRepository participantRepository): IUserService
{
    public async Task<User> AddUser(User user)
    {
        if (await userRepository.IsExistWithLogin(user.Login))
        {
            throw new BusinessException($"User with login {user.Login} was exists!");
        }
        user.Password = PasswordHasher.Hash(user.Password);

        var result = await userRepository.Add(user);

        await unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<User> RegisterParticipant(User user)
    {
        if (await userRepository.IsExistWithLogin(user.Login))
        {
            throw new BusinessException($"User with login {user.Login} was exists!");
        }
        user.Password = PasswordHasher.Hash(user.Password);
        
        var participant = await participantRepository.AddParticipant(new Participant
        {
            Id = user.Id,
            Login = user.Login,
            Name = Guid.NewGuid().ToString(),
            Password = user.Password,
            Role = user.Role
        });
        
        await unitOfWork.SaveChangesAsync();

        return participant;
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