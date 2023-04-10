namespace EntityFramework_DotNet7_SQLServer.Services.CharacterService;

public class CharacterServiceSqlDb : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public CharacterServiceSqlDb(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllAsync()
    {
        return new ServiceResponse<List<GetCharacterDto>>
            { Data = _mapper.Map<List<GetCharacterDto>>(await _context.Characters.ToListAsync()) };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetByIdAsync(int id)
    {
        return new ServiceResponse<GetCharacterDto>
            { Data = _mapper.Map<GetCharacterDto>(await _context.Characters.FirstOrDefaultAsync(c => c.Id == id)) };
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddAsync(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        return new ServiceResponse<GetCharacterDto>
            { Data = _mapper.Map<GetCharacterDto>(character) };
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateAsync(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
            if (character is null)
            {
                throw new Exception($"Character with Id '{updatedCharacter.Id}' not found");
            }

            _mapper.Map(updatedCharacter, character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> DeleteByIdAsync(int id)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            if (character is null)
            {
                throw new Exception($"Character with id '{id}' not found");
            }

            _context.Remove(character);
            await _context.SaveChangesAsync();
            serviceResponse.Message = $"Character '{character.Name}' has been deleted";
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}