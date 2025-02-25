namespace Application.Commands.Auth.Register.Strategies;

internal interface IRegisterUser
{
    Guid RegisterUser(RegisterUserCommand command);
}
