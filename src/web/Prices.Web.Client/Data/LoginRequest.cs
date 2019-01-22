using MediatR;

namespace Prices.Web.Client.Data
{
    public class LoginRequest : IRequest<UserState>
    {
        public string UserToken { get; set; }
    }
}