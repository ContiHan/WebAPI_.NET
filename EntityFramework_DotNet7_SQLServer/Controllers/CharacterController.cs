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

    [HttpGet(nameof(GetAllAsync))]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAllAsync()
    {
        return Ok(await _characterService.GetAllAsync());
    }

    [HttpGet(nameof(GetByIdAsync) + "/{id}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetByIdAsync(int id)
    {
        return Ok(await _characterService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacterAsync(AddCharacterDto newCharacter)
    {
        return Ok(await _characterService.AddCharacterAsync(newCharacter));
    }
}