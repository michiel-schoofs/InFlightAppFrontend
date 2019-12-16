using InFlightAppBACKEND.Data;
using InFlightAppBACKEND.Data.Repositories;
using InFlightAppBACKEND.Data.Repositories.Interfaces;
using InFlightAppBACKEND.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.SwaggerGeneration.Processors.Security;
using System;
using System.Security.Claims;
using System.Text;

namespace InFlightAppBACKEND
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
            #region MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #endregion

            #region DBContext
            services.AddDbContext<DBContext>(options =>
            {
                //options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]); 
                options.UseSqlServer(Configuration["ConnectionStrings:MichielAndLarsConnection"]);
            });
            #endregion

            #region Dependency Injections
            services
                .AddScoped<DBInitializer>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<IPassengerRepository, PassengerRepository>()
                .AddScoped<IFlightRepository, FlightRepository>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<ITravelGroupRepository, TravelGroupRepository>()
                .AddScoped<INotificationRepository,NotificationRepository>();
            #endregion

            #region NSwag
            //Make sure NSwag.AspNetCore is at 12.0.14 otherwise this doesn't wrk
            services.AddOpenApiDocument(d =>
            {
                d.Description = "Back-end api written for UWP application";
                d.DocumentName = "In-Flight-API";
                d.Version = "Development";
                d.Title = "In-Flight-API";
                d.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token", new SwaggerSecurityScheme
                {
                    Type = SwaggerSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = SwaggerSecurityApiKeyLocation.Header,
                    Description = "Copy 'Bearer' + valid JWT token into field"
                }));
                d.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
            });
            #endregion

            #region Default identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DBContext>();
            #endregion

            #region Authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion

            #region Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Passenger", policy => policy.RequireClaim(ClaimTypes.Role, "Passenger"));
                options.AddPolicy("Crew", policy => policy.RequireClaim(ClaimTypes.Role, "Crew"));
            });
            #endregion

            #region Identity Configuration
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 4;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
            #endregion

            #region CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin());
            });
            #endregion

            #region SignalR
            services.AddSignalR();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DBInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            //app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();

            app.UseSwaggerUi3();
            app.UseSwagger();

            app.UseCors("AllowAllOrigins");

            app.UseSignalR(routes => {
                routes.MapHub<ChatHub>("/chatHub");
            });

            dbInitializer.seedDatabase().Wait(TimeSpan.FromMinutes(10));
            dbInitializer.seedIdentityDatabase().Wait();
        }
    }
}
