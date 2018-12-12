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

        public Task<string> GetUserNameAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(PriceWebUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(PriceWebUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(PriceWebUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PriceWebUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PriceWebUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}