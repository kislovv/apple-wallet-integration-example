using BL.Abstractions;
using BL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class CardRepository(AppDbContext dbContext) : ICardRepository
{
    public async Task<Card> GetCardByUserHashId(string userHashId)
    {
        return await dbContext.Cards
            .SingleAsync(c => c.UserHashId == userHashId);
    }
}