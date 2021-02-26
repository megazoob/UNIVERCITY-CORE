using Administration.Data.DataContext.SQL_SERVER;
using Administration.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Administration.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Filter.AuthOperation;
using Authorization.Gateway.Services.Models;
using Authorization.Gateway.Services.Inerfaces;
using Authorization.Gateway.Services.Services;


namespace Administration.API
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
           

            //Если не добавить опцию игнорирования рекурсии, то дерево отделов не сможет быть
            //сериализовано в методе WebApi. И вместо дерева Json будет ошибка.
            //Подробно здесь: https://www.thecodebuzz.com/jsonexception-possible-object-cycle-detected-object-depth/
            services.AddControllers().AddNewtonsoftJson(options =>
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Administration.API",
                    Version = "v1",
                    Description = "API управления административной структурой.",
                    Contact = new OpenApiContact
                    {
                        Name = "Andrey Trezubov",
                        Email = "a.trezubov@gmail.com",
                        Url = new Uri("https://megazoob.com"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                //JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", //c маленькой буквы
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                //////Add Operation Specific Authorization///////применяет фильтр только к API с атрибутом Auhtorize...
                c.OperationFilter<AuthOperationFilter>();
                //end bearer
            });

            //регистрация контекста базы данных.
            string connectionNews = Configuration.GetConnectionString("AdministrationConnection");
            services.AddDbContext<DBContextSQLServer>(options => options.UseSqlServer(connectionNews, p => p.MigrationsAssembly("Administration.API")));
            //регистрация сервиса управления отделами.
            services.AddScoped<IDepartmentsManagement, DepartmentsManagementSQLServer>();

            //авторизация
            //-------------настройки Url из appsettings.json скопом---------------------------------
            UrlSettings configuration = new UrlSettings();
            Configuration.GetSection("UrlSettings").Bind(configuration);
            services.AddSingleton(configuration);
            //--------------------------------------------------------------------------------------
            if (Configuration["UseSignalR"].ToLower().Equals("true"))
            {
                services.AddSingleton<IAuthorizationGateway, AuthorizationGatewaySignalR>(); //signalR. 
            } else
            {
                services.AddSingleton<IAuthorizationGateway, AuthorizationGatewayHttp>(); //Web Api.
            }
            //


            if (Configuration["ApplyMigration"].ToLower().Equals("true"))
            {
                Program.MigrateDatabase(services);
            }


            services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint(Configuration["ApplicationName"] + "/swagger/v1/swagger.json", "Administration.API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
