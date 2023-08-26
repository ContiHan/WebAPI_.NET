using EntityFramework_DotNet7_SQLServer.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFramework_DotNet7_SQLServer.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WeaponController : ControllerBase
{
    private readonly IWeaponService _weaponService;

    public WeaponController(IWeaponService weaponService)
    {
        _weaponService = weaponService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> CreateWeaponAsync(AddWeaponDto newWeapon)
    {
        var response = await _weaponService.AddAsync(newWeapon);
        if (response.Data is null)
        {
            return NotFound(response.Message);
        }

        return Ok(response);
    }
}