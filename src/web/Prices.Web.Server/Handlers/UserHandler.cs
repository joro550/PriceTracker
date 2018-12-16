using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Handlers.Requests;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Handlers
{
    public class UserHandler
        : IRequestHandler<GetUserByUsernameRequest, UserEntity>
    {
        private readonly IUserRepository _userRepository;

        public UserHandler(IUserRepository userRepository) 
            => _userRepository = userRepository;

        public async Task<UserEntity> Handle(GetUserByUsernameRequest byUsernameRequest, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUsername(byUsernameRequest.Username);
            return userEntity ?? new NullUserEntity();
        }
    }
}
