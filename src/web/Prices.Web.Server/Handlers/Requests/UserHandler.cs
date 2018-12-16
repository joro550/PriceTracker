using MediatR;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Handlers.Requests
{
    public class GetUserByUsernameRequest : IRequest<UserEntity>
    {
        public string Username { get; set; }
    }
}