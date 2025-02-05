using Domain.Tokens;

namespace Application.Abstractions.Factories;

public interface IEmailVerificationLinkFactory
{
    string Create(EmailVerificationToken emailVerificationToken);
}
