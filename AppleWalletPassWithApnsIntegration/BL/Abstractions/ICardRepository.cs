using BL.Entities;

namespace BL.Abstractions;

public interface ICardRepository
{
    Task<Card> GetCardByUserHashId(string userHashId);
}