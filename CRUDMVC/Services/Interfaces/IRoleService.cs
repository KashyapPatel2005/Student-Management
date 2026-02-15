public interface IRoleService
{
    Task CreateRolesAsync();
    Task AssignUserToRoleAsync(string email, string role);
}