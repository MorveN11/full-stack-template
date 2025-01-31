using SharedKernel.Errors;

namespace Domain.Roles;

public static class RoleErrors
{
    public static Error NotFound(Guid roleId) =>
        Error.NotFound("Roles.NotFound", $"The role with the Id = '{roleId}' was not found");
}
