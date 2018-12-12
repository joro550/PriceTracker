using System;
using System.Linq;
using System.Net.Mime;
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
using Prices.Web.Server.Identity;

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

            services.AddIdentity<PriceWebUser, IdentityRole>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => { });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = true;
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
            services.AddScoped(s => config.CreateMapper());
            services.AddScoped(s => storageAccount.CreateCloudTableClient());
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemPriceRepository, ItemPriceRepository>();
            services.AddScoped(s => storageAccount.CreateCloudTableClient());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();
            app.UseAuthentication();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
            app.UseBlazor<Client.Startup>();
        }
    }
}