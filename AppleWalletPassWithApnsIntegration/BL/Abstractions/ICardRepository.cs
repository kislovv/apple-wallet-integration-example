using BL.Entities;

namespace BL.Abstractions;

public interface ICardRepository
{
    Task<Card> GetCardByUserHashId(string userHashId);
    Task<Card> GetCardWithPassAndParticipantById(long cardId);
    Task<Card> GetCardWithPartnerAndPassSpecificByUserHashId(string userHashId);
    void UpdateCard(Card card);
    Task<Card> AddAsync(Card card);
}