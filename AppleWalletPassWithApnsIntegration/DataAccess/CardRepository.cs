using BL.Abstractions;
using BL.Entities;

namespace DataAccess;

public class CardRepository: ICardRepository
{
    public Task<Card> GetCardByUserHashId(string userHashId)
    {
        throw new NotImplementedException();
    }
}