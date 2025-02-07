namespace Domain.Authorization;

public sealed class PermissionAccess(string resource, string operation)
{
    private string Resource { get; init; } = resource;
    private string Operation { get; init; } = operation;

    public string Key => $"{Resource}:{Operation}";
    public string SelfAccessKey => $"{Resource}:{Operation}:self";
}
