using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using ToDo.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ToDo.Api.Services;
using ToDo.Api.UtilModels;
using ToDo.Api.Scopes;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace ToDo.Api
{
    public class Startup
    {

        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
            _logger.LogInformation("TODO app started!");
        }

        public IConfiguration Configuration { get; }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddCors(o => o.AddPolicy("ToDoPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            //adding authentication services
            string domain = Configuration["Auth0:Domain"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            //adding authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:to-do-list", policy => policy.Requirements.Add(new HasScopeRequirement("read:to-do-list", domain)));
                options.AddPolicy("write:to-do-list", policy => policy.Requirements.Add(new HasScopeRequirement("write:to-do-list", domain)));
                options.AddPolicy("remove:to-do-list", policy => policy.Requirements.Add(new HasScopeRequirement("remove:to-do-list", domain)));
                options.AddPolicy("read:to-do-item", policy => policy.Requirements.Add(new HasScopeRequirement("read:to-do-item", domain)));
                options.AddPolicy("write:to-do-item", policy => policy.Requirements.Add(new HasScopeRequirement("write:to-do-item", domain)));
                options.AddPolicy("remove:to-do-item", policy => policy.Requirements.Add(new HasScopeRequirement("remove:to-do-item", domain)));
            });

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "ToDo API",
                    Version = "v1",
                    Description = "A ASP.NET Core Web API for managing todo lists",
                    Contact = new OpenApiContact
                    {
                        Name = "Bojan Vlasonjic",
                        Email = "bojanvlasonjic@yahoo.com"
                    }

                });
            });

            services.AddDbContext<ToDoDbContext>(options => options.UseSqlServer(Configuration["ConnectionString"]));

            //adding custom services
            services.AddTransient<IToDoListService, ToDoListService>();
            services.AddTransient<IToDoItemService, ToDoItemService>();

            //init my todoOptions by loading the values from appsettings.json
            services.Configure<ToDoOptions>(Configuration.GetSection("ToDoOptions"));

            services.AddHostedService<ReminderService>();
            services.AddHostedService<SharedListsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            UpdateDatabase(app); //execute migrations on startup if there are any
        }


        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ToDoDbContext>())
                {
                    context.Database.Migrate();
                }
            }

        }
    }
}
