using System.Security.Claims;

namespace EntityFramework_DotNet7_SQLServer.Services.CharacterService;

public class CharacterService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    : ICharacterService
{
    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharactersAsync()
    {
        return new ServiceResponse<List<GetCharacterDto>>
        {
            Data = mapper.Map<List<GetCharacterDto>>(
                await context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => c.User!.Id == GetUserId()).ToListAsync())
        };
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterByIdAsync(int id)
    {
        return new ServiceResponse<GetCharacterDto>
        {
            Data = mapper.Map<GetCharacterDto>(
                await context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId()))
        };
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacterAsync(AddCharacterDto newCharacter)
    {
        var character = mapper.Map<Character>(newCharacter);
        character.User = await context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        context.Characters.Add(character);
        await context.SaveChangesAsync();

        return new ServiceResponse<List<GetCharacterDto>>
        {
            Data = await context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => mapper.Map<GetCharacterDto>(c))
                .ToListAsync()
        };
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacterAsync(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = await context.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
            if (character is null || character.User!.Id != GetUserId())
            {
                throw new Exception($"Character with Id '{updatedCharacter.Id}' not found");
            }

            mapper.Map(updatedCharacter, character);
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

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacterByIdAsync(int id)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            var character = await context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            if (character is null)
            {
                throw new Exception($"Character with id '{id}' not found");
            }

            context.Remove(character);
            await context.SaveChangesAsync();
            serviceResponse.Message = $"Character '{character.Name}' has been deleted";
            serviceResponse.Data = await context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => mapper.Map<GetCharacterDto>(c))
                .ToListAsync();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkillAsync(AddCharacterSkillDto newCharacterSkill)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c =>
                    c.Id == newCharacterSkill.CharacterId && c.User!.Id == GetUserId());
            if (character is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Character not found";
                return serviceResponse;
            }

            var skill = await context.Skills
                .FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
            if (skill is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Skill not found";
                return serviceResponse;
            }

            character.Skills!.Add(skill);
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

    private int GetUserId() =>
        int.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}