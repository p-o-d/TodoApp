using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using todoapi.AppOptions;
using todoapi.Contracts;
using todoapi.Services;

namespace todoapi
{
    public class Startup
    {
        private IConfiguration _config;

        private IHostingEnvironment _hostEnv;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _config = configuration;
            _hostEnv = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<HashOptions>(_config.GetSection("Hashing"));
            services.Configure<JWTOptions>(_config.GetSection("JWT"));

            IDataDecodingKey dataKey;
            ISignatureDecodingKey signatureKey;
            if (_hostEnv.IsDevelopment())
            {
                var devkeyOptions = Options.Create<DevKeysOptions>(new DevKeysOptions());
                _config.GetSection("DevKeys").Bind(devkeyOptions.Value);
                var devDataKey = new FromOptionsSymmetricDataKey(devkeyOptions);
                var devSignatureKey = new FromOptionsSymmetricSignatureKey(devkeyOptions);
                services.AddSingleton<IDataEncodingKey>(devDataKey);
                services.AddSingleton<ISignatureEncodingKey>(devSignatureKey);
                dataKey = devDataKey;
                signatureKey = devSignatureKey;
            }
            else
            {
                var prodDataKey = new FromEnvironmentSymmetricDataKey();
                var prodSignatureKey = new FromEnvironmentSymmetricSignatureKey();
                services.AddSingleton<IDataEncodingKey>(prodDataKey);
                services.AddSingleton<ISignatureEncodingKey>(prodSignatureKey);
                dataKey = prodDataKey;
                signatureKey = prodSignatureKey;
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var jwtSchemeName = _config.GetSection("JWT").GetSection("SchemeName").Value;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = jwtSchemeName;
                options.DefaultChallengeScheme = jwtSchemeName;
            }).AddJwtBearer(jwtSchemeName, jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signatureKey.Key,
                    TokenDecryptionKey = dataKey.Key,

                    ValidateIssuer = true,
                    ValidIssuer = _config.GetSection("JWT").GetSection("Issuer").Value,

                    ValidateAudience = true,
                    ValidAudience = _config.GetSection("JWT").GetSection("Audience").Value,

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.FromSeconds(5)
                };
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient(typeof(IPasswordHasher), typeof(PasswordHasher));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseMvc();
        }
    }
}
