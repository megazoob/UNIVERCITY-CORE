using Authorization.Api.Hubs.SignalR;
using Authorization.Api.Middlewares;
using Authorization.Data.DataContext.SQLServer;
using Authorization.Data.Models;
using Authorization.Services.Interfaces;
using Authorization.Services.Services;
using Filter.AuthOperation;
using Localization.Services;
using Localization.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Api
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

            services.AddControllers();

            services.AddCors();

            //-------------------------����������---------------------------------------------------------------------------------------------------------------------------
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("de"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            //string connectionLoc = Configuration.GetConnectionString("LocalizeConnection");
            //services.AddDbContext<LocalizationContext>(options => options.UseSqlServer(connectionLoc, p => p.MigrationsAssembly("WebApplication")));
            services.AddTransient<IStringLocalizer, EFStringLocalizer>();//����������� ����� (����������� ������������ � �������������)
            services.AddSingleton<IStringLocalizerFactory>(new EFStringLocalizerFactory());
            services.AddControllersWithViews().AddDataAnnotationsLocalization(options =>
            { //��������� ������
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                factory.Create(null);
            })
            .AddViewLocalization();
            //END �����������-----------------------------------------------------------------------------------------------------------------------------------------------

            //identity
            string connectionIdentity = Configuration.GetConnectionString("IdentityConnection");
            services.AddDbContext<ASPUserContext>(options => options.UseSqlServer(connectionIdentity, p => p.MigrationsAssembly("Authorization.Api")));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ASPUserContext>()
                .AddSignInManager<SignInManager<User>>();
            services.AddTransient<ILogin, LoginUser>(); //�����
            services.AddTransient<IRegister, RegisterUser>(); //�����������
            services.AddTransient<IUserInfo, UserInfo>(); //���������� � ������������
            services.AddTransient<IUserRoleManage, UserRoleManage>(); //���������� ������ ������������
            services.AddTransient<IUserProfileManage, UserProfileManage>(); //���������� �������� ������������
            services.AddScoped<ITokenValidation, TokenValidation>(); //�������� ������

            //----------------------JWT-------------------------------------------------------------------------------------------------------------------------------------
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            var tk = Configuration.GetValue(typeof(string), "TokenKey");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tk.ToString().ToCharArray()));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    opt =>
                    {
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = key,
                            ValidateAudience = false,
                            ValidateIssuer = false,

                        };

                    });
            //authorization politics
            //� ����������� API ���������� �������� �������� ����������� [Authorize(Policy = "ValidAccessToken")]
            services.AddAuthorization(options =>
                       options.AddPolicy("ValidAccessToken", policy =>
                       {
                           policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                           policy.RequireAuthenticatedUser();
                       }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "�����������. .NET CORE 5.0",
                    Description = "��� ���������� ���������� Identity � ��������� �������������� � ������� JWT.",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Andrey Trezubov",
                        Email = "a.trezubov@gmail.com",
                        Url = new Uri("https://megazoob.com"),
                    }
                });
                //BEARER
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http, //���� ��������� ����� ApiKey ��� �������� � ���������, �� ��������� ����� Authorization: token, � ��� ������� ����� Authorization: Bearer token - �.�. ���, ��� ��� �����
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });
                //////Add Operation Specific Authorization///////��������� ������ ������ � API � ��������� Auhtorize...
                c.OperationFilter<AuthOperationFilter>();
                //end bearer

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(Configuration["ApplicationName"] + "/swagger/v1/swagger.json", "Authorization.Api v1"));
            // }

            //---------------------------�����������-----------------------------------------------------------
            var supportedCultures = new[] //������ �������������� �����
            {
                new CultureInfo("en"),
                new CultureInfo("ru"),
                new CultureInfo("de")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"), //���� �� ���������
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            //END �����������----------------------------------------------------------------------------------

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseMiddleware<AuthMiddleware>(); //�������������� �������� ���������� �����������.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TokenValidationSignalRHub>("/TokenValidation"); //SignalR
            });
        }
    }
}
