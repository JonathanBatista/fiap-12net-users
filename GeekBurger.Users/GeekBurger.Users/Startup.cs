﻿using AutoMapper;
using GeekBurger.Users.Application.AzureServices;
using GeekBurger.Users.Application.AzureServices.AzureConnections;
using GeekBurger.Users.Application.AzureServices.Services;
using GeekBurger.Users.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace GeekBurger.Users
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcCoreBuilder = services.AddMvcCore()
                                            .AddFormatterMappings()
                                            .AddJsonFormatters()
                                            .AddCors();

            services.AddAutoMapper();

            services.AddTransient<IFaceService, FaceService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSingleton<IServiceBus, AzureServiceBus>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
