using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Api.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Api.Auth;
using Autofac;
using System.Reflection;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Autofac.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            ConfigValue.Configuration = Configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                      .AllowAnyMethod()
                                                                       .AllowAnyHeader()));

            // For Authorization
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(ConfigValue.TokenAuthOption.TokenType, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            // For AutoMapper
            services.AddSingleton(AutoMapperConfiguration());
            services.AddMvc();

            var builder = DependencyInjectionAutowired();

            builder.Populate(services);
            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(ConfigValue.Logging);
            loggerFactory.AddDebug();

            app.UseCors("AllowAll");

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            #region UseJwtBearerAuthentication
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = TokenAuthOption.Key,
                    ValidAudience = TokenAuthOption.Audience,
                    ValidIssuer = TokenAuthOption.Issuer,
                    // When receiving a token, check that we've signed it.
                    ValidateIssuerSigningKey = true,
                    // When receiving a token, check that it is still valid.
                    ValidateLifetime = true,
                    // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                    // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                    // machines which should have synchronised time, this can be set to zero. Where external tokens are
                    // used, some leeway here could be useful.
                    ClockSkew = TimeSpan.FromMinutes(0)
                }
            });
            #endregion

            app.UseMvc();
        }

        private IMapper AutoMapperConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CoreToDto>();
                cfg.AddProfile<DtoToCore>();
            });

            return config.CreateMapper();
        }

        private ContainerBuilder DependencyInjectionAutowired()
        {
            var builder = new ContainerBuilder();

            // For Repository
            //services.AddTransient<IUserRepository>(provider=> new SqlRepository(Configuration.GetSection("ConnectionString").Value));
            var assembly = Assembly.Load(new AssemblyName(ConfigValue.RepositoryAssembly));

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Repository", StringComparison.Ordinal))
                .As<IUserRepository>()
                .WithParameter("connectionString", ConfigValue.ConnectionString);

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Repository", StringComparison.Ordinal))
                .As<ITaxSlabRepository>()
                .WithParameter("connectionString", ConfigValue.ConnectionString);

            assembly = Assembly.Load(new AssemblyName(ConfigValue.BusinessLogicAssembly));

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("BL", StringComparison.Ordinal))
                .As<IUserBL>();

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("BL", StringComparison.Ordinal))
                .As<ITaxSlabBL>();

            return builder;
        }
    }
}
