using System.Security.Cryptography;
using System.Text;

namespace EntityFramework_DotNet7_SQLServer.Data;

public class AuthRepository : IAuthRepository
{
    private readonly DataContext _context;

    public AuthRepository(DataContext context)
    {
        _context = context;
    }

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

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        response.Data = user.Id;
        return response;
    }

    public async Task<ServiceResponse<string>> LoginAsync(string username, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        if (user is null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            response.Success = false;
            response.Message = $"Wrong username or password";
            return response;
        }

        response.Data = user.Id.ToString();
        return response;
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
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
}