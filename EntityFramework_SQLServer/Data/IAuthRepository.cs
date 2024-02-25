namespace EntityFramework_DotNet7_SQLServer.Data;

public interface IAuthRepository
{
    public Task<ServiceResponse<int>> RegisterAsync(User user, string password);
    public Task<ServiceResponse<string>> LoginAsync(string username, string password);
    public Task<bool> UserExistsAsync(string username);
}