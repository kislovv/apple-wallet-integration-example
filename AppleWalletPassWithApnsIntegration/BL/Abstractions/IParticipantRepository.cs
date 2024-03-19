using BL.Entities;

namespace BL.Abstractions;

public interface IParticipantRepository
{
    Task<Participant> GetParticipantByCard(Card card);
    Task<Participant> GetParticipantByUserId(long userId);
    Task<Participant> AddParticipant(Participant participant);
}