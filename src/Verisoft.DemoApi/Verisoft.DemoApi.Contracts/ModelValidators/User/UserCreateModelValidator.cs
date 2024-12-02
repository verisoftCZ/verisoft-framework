using FluentValidation;
using Verisoft.DemoApi.Contracts.Models.User;

namespace Verisoft.DemoApi.Contracts.ModelValidators;

public class UserCreateModelValidator : AbstractValidator<UserCreateModel>
{
    public UserCreateModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.FirstName);
        RuleFor(x => x.Surname).NotEmpty();
    }
}