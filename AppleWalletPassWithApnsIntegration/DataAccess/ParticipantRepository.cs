using BL.Abstractions;
using BL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ParticipantRepository(AppDbContext appDbContext) : IParticipantRepository
{

    public Task<Participant> GetParticipantByCard(Card card)
    {
        throw new NotImplementedException();
    }

    public Task<Participant> AddParticipant(Participant participant)
    {
        throw new NotImplementedException();
    }
}