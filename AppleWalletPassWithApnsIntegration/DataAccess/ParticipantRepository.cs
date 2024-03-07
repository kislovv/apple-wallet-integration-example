using BL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ParticipantRepository(AppDbContext appDbContext)
{
    public async Task<List<Participant>> GetAllParticipants()
    {
        return await appDbContext.Participants.ToListAsync();
    }
}