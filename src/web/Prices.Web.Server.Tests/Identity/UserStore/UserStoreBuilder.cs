using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Prices.Web.Server.Data;
using Prices.Web.Server.Identity;
using Prices.Web.Server.Tests.Fakes;

namespace Prices.Web.Server.Tests.Identity.UserStore
{
    public class UserStoreBuilder
    {
        private IUserRepository _userRepository;
        
        public IUserStore<PriceWebUser> Build()
        {
            var userRepository = _userRepository ?? FakeUserRepository.WithNoRecords(); 
            
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            return new CustomUserStore(userRepository, config.CreateMapper());
        }

        public UserStoreBuilder WithUserRepository(IUserRepository repository)
        {
            _userRepository = repository;
            return this;
        }
    }
}