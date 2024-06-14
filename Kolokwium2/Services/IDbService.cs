using Kolokwium2.Models;
using Kolokwium2.Models.DTOs;

namespace Kolokwium2.Services;

public interface IDbService
{
    Task<CharacterDto> GetCharacterDetails(int characterId);
    Task<bool> DoesCharacterExist(int characterId);

    Task<List<ItemDto>> GetCharacterItems(int characterId);
    Task<List<TitleDto>> GetCharacterTitles(int characterId);
    
    Task<List<BackpackDto>> AddItemsToCharacter(int characterId, List<int> itemIds);

    Task<bool> DoGivenItemsExist(List<int> itemIds);

    Task<bool> DoesCharacterHasEnoughMaxWeight(int characterId, List<int> itemIds);
    
}