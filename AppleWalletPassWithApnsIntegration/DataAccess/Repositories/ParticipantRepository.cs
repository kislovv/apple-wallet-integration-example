using BL.Abstractions;
using BL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ParticipantRepository(AppDbContext appDbContext) : IParticipantRepository
{

    public async Task<Participant> GetParticipantByCard(Card card)
    {
        return await appDbContext.Participants
            .Include(p=> p.Card)
            .SingleAsync(p => p.Card.Id == card.Id);
    }

    public Task<Participant> AddParticipant(Participant participant)
    {
        throw new NotImplementedException();
    }
}