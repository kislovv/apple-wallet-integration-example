using BL.Entities;

namespace BL.Abstractions;

public interface IPartnerRepository
{
    Task<Partner> GetPartnerWithPassSpecificByCardId(long card);
}