using BL.Dtos;
using BL.Entities;

namespace BL.Abstractions;

public interface ICardService
{
    Task<Card> CreateCard(CardDto card);
}