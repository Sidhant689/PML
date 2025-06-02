using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pml.Application.IServices;
using Pml.Application.Services;
using Pml.Domain.Authentication;
using Pml.Domain.IRepositories.Client;
using Pml.Domain.IRepositories.Master;
using Pml.Infrastructure.Authentication;
using Pml.Infrastructure.Client;
using Pml.Infrastructure.Client.Factory;
using Pml.Infrastructure.Client.ClientRepositories;
using Pml.Infrastructure.Master;
using Pml.Infrastructure.Master.Repositories;
using Pml.Shared.Entities.Settings;

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

            // Register SqliteConnection factory
            services.AddTransient<SqliteConnection>(sp => {
                var dbSettings = sp.GetRequiredService<DatabaseSettings>();
                return new SqliteConnection(dbSettings.MasterConnectionString);
            });

            // Add PostgreSQL master database context
            services.AddSingleton<MasterDbContext>();

            // Add the ClientRepositoryFactory
            services.AddScoped<IClientRepositoryFactory, ClientRepositoryFactory>();

            // Register the repository provider
            services.AddScoped<IClientRepositoryProvider, ClientRepositoryProvider>();

            // Register the dynamic client repository wrapper
            services.AddScoped<IClientRepository, DynamicClientRepository>();

            // Register Repositories
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            // Register Services
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

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
