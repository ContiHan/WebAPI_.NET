using System.Security.Claims;

namespace EntityFramework_DotNet7_SQLServer.Services.WeaponService;

public class WeaponService : IWeaponService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddWeaponAsync(AddWeaponDto newWeapon)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        
        try
        {
            var character = await _context.Characters
                .FirstOrDefaultAsync(
                    c => c.Id == newWeapon.CharacterId
                         && c.User!.Id == int.Parse(_httpContextAccessor.HttpContext!.User
                             .FindFirstValue(ClaimTypes.NameIdentifier)!));

            if (character is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Character not found.";
                return serviceResponse;
            }
            
            var weapon = _mapper.Map<Weapon>(newWeapon);
            _context.Weapons.Add(weapon);
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
}