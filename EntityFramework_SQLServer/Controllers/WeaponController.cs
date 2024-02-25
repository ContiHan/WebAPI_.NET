using EntityFramework_DotNet7_SQLServer.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFramework_DotNet7_SQLServer.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WeaponController(IWeaponService weaponService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> CreateWeaponAsync(AddWeaponDto newWeapon)
    {
        var response = await weaponService.AddWeaponAsync(newWeapon);
        if (response.Message != "Character not found.")
        {
            return BadRequest(response);
        }

        if (response.Data is null)
        {
            return NotFound(response.Message);
        }


        return Ok(response);
    }
}