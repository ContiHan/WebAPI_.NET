using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EntityFramework_DotNet7_SQLServer.Data;

public class AuthRepository(DataContext context, IConfiguration configuration) : IAuthRepository
{
    public async Task<ServiceResponse<int>> RegisterAsync(User user, string password)
    {
        var response = new ServiceResponse<int>();
        if (await UserExistsAsync(user.Username))
        {
            response.Success = false;
            response.Message = $"User with username '{user.Username}' already exists";
            return response;
        }

        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        context.Users.Add(user);
        await context.SaveChangesAsync();
        response.Data = user.Id;
        return response;
    }

    public async Task<ServiceResponse<string>> LoginAsync(string username, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await context.Users.FirstOrDefaultAsync(
            u => string.Equals(u.Username.ToLower(), username.ToLower()));
        if (user is null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            response.Success = false;
            response.Message = $"Wrong username or password";
            return response;
        }

        response.Data = CreateToken(user);
        return response;
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await context.Users.AnyAsync(u => string.Equals(u.Username.ToLower(), username.ToLower()));
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        };

        var appSettingsToken = configuration.GetSection("AppSettings:Token").Value ??
                               throw new Exception("AppSettings token is null");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}