namespace EntityFramework_DotNet7_SQLServer.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;

    private static List<Character> _characters = new()
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

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacterAsync(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = _characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
            if (character is null)
            {
                throw new Exception($"Character with Id '{updatedCharacter.Id}' not found");
            }

            character.Name = updatedCharacter.Name;
            character.HitPoints = updatedCharacter.HitPoints;
            character.Strength = updatedCharacter.Strength;
            character.Defense = updatedCharacter.Defense;
            character.Intelligence = updatedCharacter.Intelligence;
            character.Class = updatedCharacter.Class;
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteByIdAsync(int id)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            if (character is null)
            {
                throw new Exception($"Character with id '{id}' not found");
            }

            _characters.Remove(character);
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(_characters);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}