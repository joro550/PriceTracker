using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Prices.Web.Server.Data;

namespace Prices.Web.Server.Identity
{
    public class CustomUserStore : IUserStore<PriceWebUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CustomUserStore(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public void Dispose()
        {
        }

        public async Task<string> GetUserIdAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUsername(user.UserName);
            return userEntity?.Id;
        }

        public async Task<string> GetUserNameAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUsername(user.UserName);
            return userEntity?.Username;
        }

        public async Task<string> GetNormalizedUserNameAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUsername(user.UserName);
            return userEntity?.Username;
        }

        public Task SetUserNameAsync(PriceWebUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(PriceWebUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PriceWebUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetById(userId);
            return _mapper.Map<PriceWebUser>(userEntity);
        }

        public async Task<PriceWebUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUsername(normalizedUserName);
            return _mapper.Map<PriceWebUser>(userEntity);
        }
    }
}