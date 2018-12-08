using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Prices.Web.Server.Data;

namespace Prices.Web.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDefaultIdentity<IdentityUser>()
                .AddUserStore<PriceUserStore>();
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet,
                    WasmMediaTypeNames.Application.Wasm
                });
            });

            var storageAccount =
                CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            services.AddScoped(s => config.CreateMapper());
            services.AddScoped(s => storageAccount.CreateCloudTableClient());
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemPriceRepository, ItemPriceRepository>();
            services.AddScoped(s => storageAccount.CreateCloudTableClient());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc(routes => { routes.MapRoute("default", "{controller}/{action}/{id?}"); });

            app.UseBlazor<Client.Startup>();
        }
    }

    public class PriceUserStore : IUserStore<IdentityUser>
    {
        public void Dispose()
        {
            
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken) 
            => Task.FromResult("userId");

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken) 
            => Task.FromResult("userName");

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken) 
            => Task.CompletedTask;

        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken) 
            => Task.FromResult("userName");

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken) 
            => Task.FromResult("userName");

        public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken) 
            => Task.FromResult(IdentityResult.Success);

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(IdentityResult.Success);

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
            => Task.FromResult(IdentityResult.Success);

        public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken) 
            => Task.FromResult(new IdentityUser());

        public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) 
            => Task.FromResult(new IdentityUser());
    }
}