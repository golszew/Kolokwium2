using Kolokwium2.Data;
using Kolokwium2.Models;
using Kolokwium2.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<CharacterDto> GetCharacterDetails(int CharacterId)
    {
        if (!await DoesCharacterExist(CharacterId))
            throw new InvalidOperationException("Character doesnt exist");
        
        Character character = await _context.Characters.FirstOrDefaultAsync(e=> e.Id ==CharacterId)
            ;
        return new CharacterDto()
        {
            FirstName = character.FirstName,
            LastName = character.LastName,
            CurrentWeight = character.CurrentWeight,
            MaxWeight = character.MaxWeight,
            BackpackItems =await GetCharacterItems(CharacterId),
            Titles = await GetCharacterTitles(CharacterId)
        };
    }

    public async Task<bool> DoesCharacterExist(int characterId)
    {
        return await _context.Characters.AnyAsync(e => e.Id == characterId);
    }

    public async Task<List<ItemDto>> GetCharacterItems(int characterId)
    {
        var character = await _context.Characters
            .Include(e => e.Backpacks)
            .ThenInclude(e=>e.Item)
            .FirstOrDefaultAsync(e => e.Id == characterId);

        List<ItemDto> list = new List<ItemDto>();
        foreach (var e in character.Backpacks)
        {
            list.Add(new ItemDto()
            {
                ItemName = e.Item.Name,
                ItemWeight = e.Item.Weight,
                Amount = e.Item.Backpacks.Count
            });
        }

        return list;
    }

    public async Task<List<TitleDto>> GetCharacterTitles(int characterId)
    {
        var character = await _context.Characters
            .Include(e => e.CharacterTitles)
            .ThenInclude(e => e.Title)
            .FirstOrDefaultAsync(e => e.Id == characterId);

        List<TitleDto> list = new List<TitleDto>();
        foreach (var e in character.CharacterTitles)
        {
            list.Add(new TitleDto()
            {
                title = e.Title.Name,
                AquiredAt = e.AcquiredAt
            });
        }

        return list;
    }

    public async Task<bool> DoGivenItemsExist(List<int> itemIds)
    {
        var items = await _context.Items
            .Where(item => itemIds.Contains(item.Id))
            .ToListAsync();
        return items.Count == itemIds.Count;
    }


    

    public async Task<bool> DoesCharacterHasEnoughMaxWeight(int characterId, List<int> itemIds)
    {
        var items = await _context.Items
            .Where(item => itemIds.Contains(item.Id))
            .ToListAsync();
        
        var character = await _context.Characters
            .FirstOrDefaultAsync(e => e.Id == characterId);
        var maxWeight = character.MaxWeight;
        var currentWeight = character.CurrentWeight;
        foreach (var item in items)
        {
            currentWeight += item.Weight;
            if (currentWeight > maxWeight)
                throw new InvalidOperationException("Weight limit exceeded");
        }

        return true;
    }
    
    public async Task<List<BackpackDto>> AddItemsToCharacter(int characterId, List<int> itemIds)
    {
        var character = await _context.Characters
            .Include(c => c.Backpacks)
            .FirstOrDefaultAsync(e => e.Id == characterId);

        List<BackpackDto> backpacks = new List<BackpackDto>();
        
        var items = await _context.Items
            .Where(item => itemIds.Contains(item.Id))
            .ToListAsync();

        foreach (var item in items)
        {
            character.CurrentWeight += item.Weight;

            
            var backpack = character.Backpacks.FirstOrDefault(e => e.ItemId == item.Id);
            if (backpack == null)
            {
                var newBackpack = new Backpack
                {
                    CharacterId = characterId,
                    ItemId = item.Id,
                    Amount = 1
                };
                _context.Backpacks.Add(newBackpack);
                backpacks.Add(new BackpackDto()
                {
                    characterId = characterId,
                    itemId = item.Id,
                    amount = 1
                });
            }
            else
            {
                backpack.Amount += 1;
                _context.Backpacks.Update(backpack);
                backpacks.Add(new BackpackDto()
                {
                    characterId = characterId,
                    itemId = item.Id,
                    amount = backpack.Amount
                });
            }
        }
        
        _context.Characters.Update(character);
        await _context.SaveChangesAsync();

        return backpacks;
    }
}