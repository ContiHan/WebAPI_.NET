using System.Security.Claims;

namespace EntityFramework_DotNet7_SQLServer.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public CharacterService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    private int GetUserId() =>
        int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharactersAsync()
    {
        return new ServiceResponse<List<GetCharacterDto>>
        {
            Data = _mapper.Map<List<GetCharacterDto>>(
                await _context.Characters.Where(c => c.User!.Id == GetUserId()).ToListAsync())
        };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterByIdAsync(int id)
    {
        return new ServiceResponse<GetCharacterDto>
        {
            Data = _mapper.Map<GetCharacterDto>(
                await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId()))
        };
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacterAsync(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();

        return new ServiceResponse<List<GetCharacterDto>>
        {
            Data = await _context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToListAsync()
        };
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacterAsync(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await _context.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
            if (character is null || character.User!.Id != GetUserId())
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

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacterByIdAsync(int id)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            if (character is null)
            {
                throw new Exception($"Character with id '{id}' not found");
            }

            _context.Remove(character);
            await _context.SaveChangesAsync();
            serviceResponse.Message = $"Character '{character.Name}' has been deleted";
            serviceResponse.Data = await _context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToListAsync();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}