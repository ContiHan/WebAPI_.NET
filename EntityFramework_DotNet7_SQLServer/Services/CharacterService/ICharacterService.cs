namespace EntityFramework_DotNet7_SQLServer.Services.CharacterService;

public interface ICharacterService
{
    public Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharactersAsync();
    public Task<ServiceResponse<GetCharacterDto>> GetCharacterByIdAsync(int id);
    public Task<ServiceResponse<List<GetCharacterDto>>> AddCharacterAsync(AddCharacterDto newCharacter);
    public Task<ServiceResponse<GetCharacterDto>> UpdateCharacterAsync(UpdateCharacterDto updatedCharacter);
    public Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacterByIdAsync(int id);
}