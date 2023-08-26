namespace EntityFramework_DotNet7_SQLServer;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Character, GetCharacterDto>();
        CreateMap<AddCharacterDto, Character>();
        CreateMap<UpdateCharacterDto, Character>();

        CreateMap<Weapon, GetWeaponDto>();
        CreateMap<AddWeaponDto, Weapon>();
    }
}