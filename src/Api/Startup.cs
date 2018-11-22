using System;
using System.Linq;
using System.Runtime.Loader;
using Api.Common;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Core.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(AutoMapperConfiguration());

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                      .AllowAnyMethod()
                                                                       .AllowAnyHeader()));
            services.AddMvc();

            var builder = DependencyInjectionAutowired();
            builder.Populate(services);
            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAll");
            app.UseMvc();
        }

        private ContainerBuilder DependencyInjectionAutowired()
        {
            var builder = new ContainerBuilder();

            // For Repository
            //services.AddTransient<IUserRepository>(provider=> new SqlRepository(Configuration.GetSection("ConnectionString").Value));

            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath($@"{AppDomain.CurrentDomain.BaseDirectory}\{Configuration.GetSection("RepositoryAssembly").Value}.dll");

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Repository", StringComparison.Ordinal))
                .As<IUserRepository>()
                .WithParameter("connectionString", Configuration.GetSection("ConnectionString").Value);

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Repository", StringComparison.Ordinal))
                .As<ITaxSlabRepository>()
                .WithParameter("connectionString", Configuration.GetSection("ConnectionString").Value);

            assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath($@"{AppDomain.CurrentDomain.BaseDirectory}\{Configuration.GetSection("BusinessLogicAssembly").Value}.dll");
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("BL", StringComparison.Ordinal))
                .As<IUserBL>();

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("BL", StringComparison.Ordinal))
                .As<ITaxSlabBL>();

            return builder;
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
    }
}
