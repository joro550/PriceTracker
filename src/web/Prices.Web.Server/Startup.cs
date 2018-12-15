using System.Linq;
using System.Net.Mime;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.WindowsAzure.Storage;
using Prices.Web.Server.Data;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
            });

            services.AddIdentity<PriceWebUser, IdentityRole>()
                .AddDefaultTokenProviders();

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient(sp => _configuration);

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
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
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

            var storageAccount =
                CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            services.AddTransient<IUserStore<PriceWebUser>, CustomUserStore>();
            services.AddTransient<IRoleStore<IdentityRole>, CustomRoleStore>();
            services.AddTransient(s => config.CreateMapper());
            services.AddTransient(s => storageAccount.CreateCloudTableClient());
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemPriceRepository, ItemPriceRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient(s => storageAccount.CreateCloudTableClient());
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