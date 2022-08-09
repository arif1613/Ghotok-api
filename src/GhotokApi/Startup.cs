using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ghotok.Data.Context;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using Ghotok.Data.Utils.Cache;
using GhotokApi.JwtTokenGenerator;
using GhotokApi.MediatR.Handlers;
using GhotokApi.MediatR.NotificationHandlers;
using GhotokApi.Models.SharedModels;
using GhotokApi.Services;
using GhotokApi.Utils.FilterBuilder;
using GhotokApi.Utils.Otp;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GhotokApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            //AddDbContext(services);


            //Register database
            services.AddScoped<IGhotokDbContext,GhotokDbContext>();


            services.AddMemoryCache();
            services.AddScoped<ICacheHelper, CacheHelper>();
            //Register Repos
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            //Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //register services
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ILoginService,LoginService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<ITokenService,TokenService>();

            //Register Utils
            services.AddScoped<IOtpSender, OtpSender>();
            services.AddScoped<IFilterBuilder,FilterBuilder>();

            //MediatR
            BuildMediator(services);
            
            AddSwagger(services);
            AddAuthentication(services);
            services.AddControllers();
        }

        private static IMediator BuildMediator(IServiceCollection services)
        {
            //Get
            services.AddMediatR(typeof(GetAppUsersRequest));
            services.AddMediatR(typeof(GetUserRequest));
            services.AddMediatR(typeof(GetUsersRequest));

            //add
            services.AddMediatR(typeof(AddAppUserRequest));
            services.AddMediatR(typeof(AddAppUsersRequest));
            services.AddMediatR(typeof(AddUserInfoRequest));
            services.AddMediatR(typeof(AddUserInfosRequest));
            //update
            services.AddMediatR(typeof(UpdateUserInfoRequest));
            services.AddMediatR(typeof(UpdateAppUserRequest));
            //delete
            services.AddMediatR(typeof(DeleteUserInfoRequest));
            services.AddMediatR(typeof(DeleteAppUserRequest));
            //Notification
            services.AddMediatR(typeof(ComitDatabaseNotification));
            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<IMediator>();
        }

        private void AddDbContext(IServiceCollection services)
        {
            //services.AddScoped<IGhotokDbContext>((options) =>
            //{
            //    return new GhotokDbContext(new DbContextOptionsBuilder<GhotokDbContext>()
            //        .UseSqlServer(Configuration["GhotokDbConnectionString"],
            //            x => x.EnableRetryOnFailure(5)).Options);
            //});


            //services.AddDbContext<GhotokDbContext>(options =>
            //    options.UseSqlServer(Configuration["GhotokDbConnectionString"], sqlServerOptionsAction: sqlOptions =>
            //    {
            //        sqlOptions.EnableRetryOnFailure(5);
            //    }));
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Ghotok api",
                    Version = "V1",
                    Description = $"Ghotok api endpoints"

                });

                c.AddSecurityDefinition("GhotokBearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer",
                        //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "GhotokBearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
                
            });
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "Ghotok.Security.Bearer",
                        ValidAudience = "Ghotok.App",
                        IssuerSigningKey = JwtSecurityKey.Create("Ghotok-Secret-Key")
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim(AppRole.Admin.ToString()));
                options.AddPolicy("PremiumUser", policy => policy.RequireClaim(AppRole.PremiumUser.ToString()));
                options.AddPolicy("User", policy => policy.RequireClaim(AppRole.User.ToString()));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAllOrigins");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsProduction())
            {
                app.UseHsts();
            }

            if (env.IsEnvironment("Test"))
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
                c.DisplayRequestDuration();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
