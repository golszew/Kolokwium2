using Kolokwium2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium2.Controllers;

[ApiController]
[Route("api/characters")]
public class CharacterController : ControllerBase
{
    private readonly IDbService _service;

    public CharacterController(IDbService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCharacterDetails(int id)
    {
        if (!await _service.DoesCharacterExist(id))
        {
            return NotFound($"Character with id - {id} doesn't exist");
        }

        return Ok(await _service.GetCharacterDetails(id));
    }

    [HttpPost("{characterId}/backpacks")]
    public async Task<IActionResult> AddItemsToCharacter(int characterId, [FromBody] List<int> itemIds)
    {
        if (!await _service.DoGivenItemsExist(itemIds))
        {
            return NotFound("One of the items were not found");
        }

        if (!await _service.DoesCharacterHasEnoughMaxWeight(characterId, itemIds))
        {
            return BadRequest("Not enough capacity");
        }

        return Ok(await _service.AddItemsToCharacter(characterId, itemIds));
    }
}