using MediatR;

namespace Prices.Web.Server.Handlers.Requests
{
    public class CreateUserRequest : IRequest<bool>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
    }
}