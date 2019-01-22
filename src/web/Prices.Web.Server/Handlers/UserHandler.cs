using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Handlers.Data.Entities;
using Prices.Web.Server.Handlers.Requests;

namespace Prices.Web.Server.Handlers
{
    public class UserHandler
        : IRequestHandler<GetUserByUsernameRequest, UserEntity>,
            IRequestHandler<CreateUserRequest, bool>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            return await _userRepository.Add(_mapper.Map<UserEntity>(request));
        }

        public async Task<UserEntity> Handle(GetUserByUsernameRequest byUsernameRequest,
            CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUsername(byUsernameRequest.Username);
            return userEntity ?? new NullUserEntity();
        }
    }
}