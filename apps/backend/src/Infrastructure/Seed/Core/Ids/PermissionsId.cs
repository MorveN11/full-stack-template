namespace Infrastructure.Seed.Core.Ids;

internal static class PermissionsId
{
    public static readonly Guid CreateUsers = Guid.Parse("0194d7d1-be76-73b5-907b-6f837d5b551b");
    public static readonly Guid ReadUsers = Guid.Parse("0194d7d1-6b7e-7b6e-b241-64f35e827c3a");
    public static readonly Guid UpdateUsers = Guid.Parse("0194d7d1-7f46-7933-bd04-fe8dca4b6d93");
    public static readonly Guid DeleteUsers = Guid.Parse("0194d7d1-8ba4-710b-88f4-b39def8c5ab4");
    public static readonly Guid LogoutUsers = Guid.Parse("0194d7d1-ab95-7096-8a78-8f7c76fff164");
    public static readonly Guid FinishSessions = Guid.Parse("01953b87-b0d6-7226-b32b-37857d222540");
    public static readonly Guid GetSessions = Guid.Parse("01953b87-d590-7b43-88b8-e1f6144ec5f8");

    public static readonly Guid ReadSelfUser = Guid.Parse("0194d7d1-cca3-782f-84aa-7ec580ddc7b8");
    public static readonly Guid UpdateSelfUser = Guid.Parse("0194d7d1-d61d-7987-9dcb-181562241a50");
    public static readonly Guid DeleteSelfUser = Guid.Parse("0194d7d1-e469-736f-8a1d-a1d2f8acd7d5");
    public static readonly Guid LogoutSelfUser = Guid.Parse("0194d7d1-ede9-75d4-933d-4d8f90ef13c3");
    public static readonly Guid FinishSelfSessions = Guid.Parse(
        "01953b87-eb14-75e7-a161-28a55074bed0"
    );
    public static readonly Guid GetSelfSessions = Guid.Parse(
        "01953b88-026f-751b-83d0-ff8e678f5f98"
    );
}
