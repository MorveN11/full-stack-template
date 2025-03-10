using SharedKernel.Errors;

namespace Domain.Entities.Auth.Roles;

public static class RoleErrors
{
    public static Error NotFound(string roleName) =>
        Error.NotFound("Roles.NotFound", $"The role with the Name = '{roleName}' was not found");
}
