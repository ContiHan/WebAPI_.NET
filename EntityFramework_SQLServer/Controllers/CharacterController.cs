﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFramework_DotNet7_SQLServer.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CharacterController(ICharacterService characterService) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet(nameof(GetAllCharactersAsync))]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAllCharactersAsync()
    {
        return Ok(await characterService.GetAllCharactersAsync());
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet(nameof(GetCharacterByIdAsync) + "/{id}", Name = nameof(GetCharacterByIdAsync))]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetCharacterByIdAsync(int id)
    {
        return Ok(await characterService.GetCharacterByIdAsync(id));
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> CreateCharacterAsync(
        AddCharacterDto newCharacter)
    {
        var response = await characterService.AddCharacterAsync(newCharacter);
        if (response.Data is null)
        {
            return NotFound(response.Message);
        }

        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacterAsync(
        UpdateCharacterDto updatedCharacter)
    {
        var response = await characterService.UpdateCharacterAsync(updatedCharacter);
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
        var response = await characterService.DeleteCharacterByIdAsync(id);
        if (response.Data is null)
        {
            return NotFound(response);
        }

        return Accepted(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost(nameof(AddCharacterSkillAsync))]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkillAsync(
        AddCharacterSkillDto newCharacterSkill)
    {
        var response = await characterService.AddCharacterSkillAsync(newCharacterSkill);
        if (response.Message != "Character not found." || response.Message != "Skill not found.")
        {
            return BadRequest(response);
        }

        if (response.Data is null)
        {
            return NotFound(response.Message);
        }

        return Ok(response);
    }
}