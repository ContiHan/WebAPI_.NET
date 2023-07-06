using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFramework_DotNet7_SQLServer.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet(nameof(GetAllCharactersAsync))]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAllCharactersAsync()
    {
        return Ok(await _characterService.GetAllAsync());
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet(nameof(GetCharacterByIdAsync) + "/{id}", Name = nameof(GetCharacterByIdAsync))]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetCharacterByIdAsync(int id)
    {
        return Ok(await _characterService.GetByIdAsync(id));
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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