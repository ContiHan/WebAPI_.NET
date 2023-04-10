namespace EntityFramework_DotNet7_SQLServer.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;

    private readonly List<Character> _characters = new()
    {
        new Character(),
        new Character
        {
            Id = 1,
            Name = "Sam"
        }
    };

    public CharacterService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllAsync()
    {
        return new ServiceResponse<List<GetCharacterDto>> { Data = _mapper.Map<List<GetCharacterDto>>(_characters) };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetByIdAsync(int id)
    {
        return new ServiceResponse<GetCharacterDto>
            { Data = _mapper.Map<GetCharacterDto>(_characters.FirstOrDefault(c => c.Id == id)) };
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacterAsync(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        character.Id = _characters.Max(c => c.Id) + 1;
        _characters.Add(character);
        return new ServiceResponse<List<GetCharacterDto>> { Data = _mapper.Map<List<GetCharacterDto>>(_characters) };
    }
}