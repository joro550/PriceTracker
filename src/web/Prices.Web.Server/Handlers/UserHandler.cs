using MediatR;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserEntity> Handle(GetUserByUsernameRequest byUsernameRequest, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUsername(byUsernameRequest.Username);
            return userEntity ?? new NullUserEntity();
        }

        public async Task<bool> Handle(CreateUserRequest request, CancellationToken cancellationToken) 
            => await _userRepository.Add(_mapper.Map<UserEntity>(request));
    }
}
