namespace EntityFramework_DotNet7_SQLServer.Services.WeaponService;

public interface IWeaponService
{
    public Task<ServiceResponse<GetCharacterDto>> AddWeaponAsync(AddWeaponDto newWeapon);
}