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

    public async Task<Participant> AddParticipant(Participant participant)
    {
        var result = await appDbContext.Participants.AddAsync(participant);
        
        return result.Entity;
    }
}