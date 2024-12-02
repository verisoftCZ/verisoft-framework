using FluentValidation;
using Verisoft.DemoApi.Contracts.Models.User;

namespace Verisoft.DemoApi.Contracts.ModelValidators;

public class UserEditModelValidator : AbstractValidator<UserEditModel>
{
    public UserEditModelValidator()
    {
        RuleFor(x => x.FirstName);
        RuleFor(x => x.Surname).NotEmpty();
    }
}