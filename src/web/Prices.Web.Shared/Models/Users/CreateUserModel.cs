using FluentValidation;

namespace Prices.Web.Shared.Models.Users
{
    public class CreateUserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
    }

    public class CreateUserValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserValidator()
        {
            RuleFor(prop => prop.Username).NotEmpty().NotNull();
            RuleFor(prop => prop.Password).NotEmpty().NotNull();
            RuleFor(prop => prop.VerifyPassword).NotEmpty().NotNull().Equal(m => m.Password);
        }
    }
}