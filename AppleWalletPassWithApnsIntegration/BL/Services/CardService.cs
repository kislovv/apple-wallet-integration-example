using BL.Abstractions;
using BL.Dtos;
using BL.Entities;

namespace BL.Services;

public class CardService(ICardRepository cardRepository, IUnitOfWork unitOfWork) : ICardService
{
    public async Task<Card> CreateCard(CardDto card)
    {
        var result = await cardRepository.AddAsync(new Card
        {
            ParticipantId = card.ParticipantId,
            PartnerId = card.PartnerId,
            UserHashId = card.UserHashId
        });
        
        await unitOfWork.SaveChangesAsync();
        
        return result;
    }
}