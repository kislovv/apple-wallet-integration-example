﻿using BL.Abstractions;
using BL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class CardRepository(AppDbContext dbContext) : ICardRepository
{
    public async Task<Card> GetCardByUserHashId(string userHashId)
    {
        return await dbContext.Cards
            .SingleAsync(c => c.UserHashId == userHashId);
    }

    public async Task<Card> GetCardWithPassAndParticipantById(long cardId)
    {
        return await dbContext.Cards
            .Include(c => c.AppleWalletPass)
                .ThenInclude(aw => aw!.AppleDevices)
            .Include(c => c.Participant)
            .Include(c => c.Partner)
                .ThenInclude(p=> p.PartnerSpecific)
                    .ThenInclude(ps =>  ps.AppleAssociatedStoreApps)
            .SingleAsync(c => c.Id == cardId);
    }

    public async Task<Card> GetCardWithPartnerAndPassSpecificByUserHashId(string userHashId)
    {
        return await dbContext.Cards
            .Include(c => c.Participant)
            .Include(c => c.Partner)
                .ThenInclude(p=> p.PartnerSpecific)
                    .ThenInclude(ps =>  ps.AppleAssociatedStoreApps)
            .SingleAsync(c => c.UserHashId == userHashId);
    }

    public void UpdateCard(Card card)
    {
        dbContext.Cards.Update(card);
    }

    public async Task<Card> AddAsync(Card card)
    {
        var result = await dbContext.Cards.AddAsync(card);

        return result.Entity;
    }
}