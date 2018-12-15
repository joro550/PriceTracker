using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.WindowsAzure.Storage;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Identity;

namespace Prices.Web.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) 
            => _configuration = configuration;


        public void ConfigureServices(IServiceCollection services)
        {
            var webTokenConfig = JsonWebTokenConfiguration.FromConfiguration(_configuration);
            var cipherConfig = CipherServiceConfig.FromConfiguration(_configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = webTokenConfig.Issuer,
                        ValidAudience = webTokenConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webTokenConfig.Key))
                    };
                });

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet, 
                    WasmMediaTypeNames.Application.Wasm
                });
            });

            services.AddMediatR(typeof(Startup).Assembly);

            var storageAccount =
                CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            services.AddTransient(sp => webTokenConfig);
            services.AddTransient(sp => cipherConfig);
            services.AddTransient<SecurityTokenHandler, JwtSecurityTokenHandler>();
            services.AddTransient<ITokenService, JsonWebTokenService>();
            services.AddTransient(sp => _configuration);
            services.AddTransient(s => config.CreateMapper());
            services.AddTransient(s => storageAccount.CreateCloudTableClient());
            services.AddTransient<ICipherService, CipherService>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemPriceRepository, ItemPriceRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseAuthentication();
            app.UseMvc(routes => routes.MapRoute("default", "api/{controller}/{action}/{id?}"));
            app.UseBlazor<Client.Startup>();
        }
    }
}