using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Handlers.Data.Entities;
using Prices.Web.Server.Handlers.Requests;

namespace Prices.Web.Server.Handlers
{
    public class UserHandler
        : IRequestHandler<GetUserByUsernameRequest, UserEntity>
    {
        private readonly IUserRepository _userRepository;

        public UserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserEntity> Handle(GetUserByUsernameRequest byUsernameRequest, CancellationToken cancellationToken) 
            => await _userRepository.GetByUsername(byUsernameRequest.Username);
    }
}
