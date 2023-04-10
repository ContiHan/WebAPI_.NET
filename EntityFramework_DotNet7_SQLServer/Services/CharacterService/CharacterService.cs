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

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllAsync()
    {
        return new ServiceResponse<List<GetCharacterDto>> { Data = _characters };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetByIdAsync(int id)
    {
        return new ServiceResponse<GetCharacterDto> { Data = _characters.FirstOrDefault(c => c.Id == id) };
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacterAsync(AddCharacterDto newCharacter)
    {
        _characters.Add(newCharacter);
        return new ServiceResponse<List<GetCharacterDto>> { Data = _characters };
    }
}