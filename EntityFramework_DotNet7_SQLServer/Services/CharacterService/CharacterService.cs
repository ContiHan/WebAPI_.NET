namespace EntityFramework_DotNet7_SQLServer.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly List<Character> _characters = new()
    {
        new Character(),
        new Character
        {
            Id = 1,
            Name = "Sam"
        }
    };

    public async Task<ServiceResponse<List<Character>>> GetAllAsync()
    {
        return new ServiceResponse<List<Character>> { Data = _characters };
    }

    public async Task<ServiceResponse<Character>> GetByIdAsync(int id)
    {
        return new ServiceResponse<Character> { Data = _characters.FirstOrDefault(c => c.Id == id) };
    }

    public async Task<ServiceResponse<List<Character>>> AddCharacterAsync(Character newCharacter)
    {
        _characters.Add(newCharacter);
        return new ServiceResponse<List<Character>> { Data = _characters };
    }
}