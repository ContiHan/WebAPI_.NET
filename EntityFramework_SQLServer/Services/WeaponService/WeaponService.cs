using System.Security.Claims;

namespace EntityFramework_DotNet7_SQLServer.Services.WeaponService;

public class WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    : IWeaponService
{
    public async Task<ServiceResponse<GetCharacterDto>> AddWeaponAsync(AddWeaponDto newWeapon)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await context.Characters
                .FirstOrDefaultAsync(
                    c => c.Id == newWeapon.CharacterId
                         && c.User!.Id == int.Parse(httpContextAccessor.HttpContext!.User
                             .FindFirstValue(ClaimTypes.NameIdentifier)!));

            if (character is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Character not found.";
                return serviceResponse;
            }

            var weapon = mapper.Map<Weapon>(newWeapon);
            context.Weapons.Add(weapon);
            await context.SaveChangesAsync();

            serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}