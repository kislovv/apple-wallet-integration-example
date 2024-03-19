using BL.Abstractions;
using BL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class PartnerRepository(AppDbContext appDbContext): IPartnerRepository
{
    public async Task<Partner> GetPartnerWithPassSpecificByCardId(long card)
    {
        return await appDbContext.Partners
            .Include(p => p.PartnerSpecific)
            .SingleAsync(p => p.Cards.Any(c => c.Id == card));
    }
}