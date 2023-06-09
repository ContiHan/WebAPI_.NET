﻿namespace EntityFramework_DotNet7_SQLServer.Services.CharacterService;

public interface ICharacterService
{
    public Task<ServiceResponse<List<GetCharacterDto>>> GetAllAsync();
    public Task<ServiceResponse<GetCharacterDto>> GetByIdAsync(int id);
    public Task<ServiceResponse<GetCharacterDto>> AddAsync(AddCharacterDto newCharacter);
    public Task<ServiceResponse<GetCharacterDto>> UpdateAsync(UpdateCharacterDto updatedCharacter);
    public Task<ServiceResponse<GetCharacterDto>> DeleteByIdAsync(int id);
}