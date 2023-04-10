using Microsoft.AspNetCore.Mvc;

namespace EntityFramework_DotNet7_SQLServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet(nameof(GetAllCharactersAsync))]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAllCharactersAsync()
    {
        return Ok(await _characterService.GetAllAsync());
    }

    [HttpGet(nameof(GetCharacterByIdAsync) + "/{id}", Name = nameof(GetCharacterByIdAsync))]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetCharacterByIdAsync(int id)
    {
        return Ok(await _characterService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> CreateCharacterAsync(
        AddCharacterDto newCharacter)
    {
        var response = await _characterService.AddAsync(newCharacter);
        if (response.Data is null)
        {
            return NotFound(response.Message);
        }

        return CreatedAtRoute(nameof(GetCharacterByIdAsync), new { id = response.Data.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacterAsync(
        UpdateCharacterDto updatedCharacter)
    {
        var response = await _characterService.UpdateAsync(updatedCharacter);
        if (response.Data is null)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpDelete(nameof(DeleteCharacterByIdAsync) + "/{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacterByIdAsync(int id)
    {
        var response = await _characterService.DeleteByIdAsync(id);
        if (response.Data is null)
        {
            return NotFound(response);
        }

        return Accepted(response);
    }
}