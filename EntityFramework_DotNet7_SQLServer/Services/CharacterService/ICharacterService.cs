namespace EntityFramework_DotNet7_SQLServer.Services.CharacterService;

public interface ICharacterService
{
    public Task<ServiceResponse<List<Character>>> GetAllAsync();
    public Task<ServiceResponse<Character>> GetByIdAsync(int id);
    public Task<ServiceResponse<List<Character>>> AddCharacterAsync(Character newCharacter);
}