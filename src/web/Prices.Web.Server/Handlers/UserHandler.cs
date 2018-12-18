using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Handlers.Requests;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Handlers
{
    public class UserHandler
        : IRequestHandler<GetUserByUsernameRequest, UserEntity>,
          IRequestHandler<CreateUserRequest, bool>
    {
        private readonly IUserRepository _userRepository;

        public UserHandler(IUserRepository userRepository) 
            => _userRepository = userRepository;

        public async Task<UserEntity> Handle(GetUserByUsernameRequest byUsernameRequest, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUsername(byUsernameRequest.Username);
            return userEntity ?? new NullUserEntity();
        }

        public async Task<bool> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            return await _userRepository.Create(new UserEntity
            {
                PartitionKey = "",
                RowKey = Guid.NewGuid().ToString("N"),
                Username = request.Username,
                PasswordSalt = request.PasswordSalt
            });
        }
    }
}
