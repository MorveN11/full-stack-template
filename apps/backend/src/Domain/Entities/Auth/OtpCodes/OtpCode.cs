using System.Security.Cryptography;
using Domain.Entities.Auth.Users;
using Domain.Enums;
using SharedKernel.Domain;

namespace Domain.Entities.Auth.OtpCodes;

public sealed class OtpCode : Entity
{
    private const int CodeLength = 6;

    public required Guid UserId { get; init; }
    public required string Code { get; init; }
    public required DateTime ExpiredOnUtc { get; init; }
    public required OtpCodeType Type { get; init; }
    public bool Verified { get; set; }
    public bool Used { get; set; }
    public User User { get; init; } = null!;

    public static string GenerateCode()
    {
        using var rng = RandomNumberGenerator.Create();

        byte[] bytes = new byte[CodeLength / 2];
        rng.GetBytes(bytes);

        return Convert.ToHexString(bytes);
    }
}
