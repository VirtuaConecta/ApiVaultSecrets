
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecretVault.Application.Interfaces;
using SecretVault.Application.Services;
using SecretVault.Domain.Interfaces;
using SecretVault.Infrastucture.Configuration;
using SecretVault.Infrastucture.Data.Context;
using SecretVault.Infrastucture.Data.Inicialization;
using SecretVault.Infrastucture.Repositories;
using System.Text;

namespace SecretVault.Infrastucture.Services
{
    public static class ServiceConfiguration
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IEncryptionService encryptionService)
        {

            ConfigureDatabase(services, configuration);
            ConfigureServices( services,  encryptionService);
            ConfigureAuthentication(services, configuration);



            return services;
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var conn = Settings.ConnectionString;
            services.AddDbContext<SecretVaultContext>(options =>
                options.UseSqlite(conn));
        }

        private static void ConfigureServices(IServiceCollection services, IEncryptionService encryptionService)
        {
            services.AddSingleton(encryptionService);
            services.AddScoped<ISecretRepository, SecretRepository>();
            services.AddScoped<ISecretService, SecretService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
          


        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {

            var key = Encoding.UTF8.GetBytes(Settings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };


            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("AdminOrOperatorPolicy", policy => policy.RequireRole("Admin", "Operator"));
            });

    
        }

    }
}
