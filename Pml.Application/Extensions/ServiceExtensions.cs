using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Pml.Application.IServices;
using Pml.Application.Services;
using Pml.Domain.Authentication;
using Pml.Domain.Entities.Settings;
using Pml.Domain.IRepositories.Master;
using Pml.Infrastructure.Authentication;
using Pml.Infrastructure.Master;
using Pml.Infrastructure.Master.Repositories;

namespace Pml.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddPMLServices(this IServiceCollection services, IConfiguration configuration)
        {
           /// Database Configuration
            services.Configure<DatabaseSettings>(options =>
                configuration.GetSection("DatabaseSettings").Bind(options));

            // Make DatabaseSettings available for direct injection
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            // Register NpgsqlConnection factory
            services.AddTransient<NpgsqlConnection>(sp => {
                var dbSettings = sp.GetRequiredService<DatabaseSettings>();
                return new NpgsqlConnection(dbSettings.MasterConnectionString);
            });

            // Add PostgreSQL master database context
            services.AddSingleton<MasterDbContext>();

            // Add the DatabaseFactory
            //services.AddScoped<DatabaseFactory>();

            // Register Repositories
            services.AddScoped<ISystemAdminRepository, SystemAdminRepository>();
            services.AddScoped<ISystemAdminRoleRepository, SystemAdminRoleRepository>();

            // Register Services
            services.AddScoped<ISystemAdminService, SystemAdminService>();

            // Add HTTP context accessor
            services.AddHttpContextAccessor();

            // JWT Authentication
            services.Configure<JwtSettings>(options =>
                    configuration.GetSection("JwtSettings").Bind(options));
            services.AddSingleton<ITokenRepository, TokenRepository>();
            services.AddScoped<IAuthService, AuthService>();

            // Configure JWT authentication
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    //RoleClaimType = ClaimTypes.Role
                };
            });
        }
    }
}
