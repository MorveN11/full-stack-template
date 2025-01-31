using Domain.Tokens;

namespace Application.Abstractions.Utilities;

public interface IEmailVerificationLinkFactory
{
    string Create(EmailVerificationToken emailVerificationToken);
}
