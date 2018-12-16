using FluentValidation;

namespace Prices.Web.Shared.Models.Users
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(model => model.Username).NotEmpty();
            RuleFor(model => model.Password).NotEmpty();
        }
    }
}