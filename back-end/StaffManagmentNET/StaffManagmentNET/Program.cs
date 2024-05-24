
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.Services;
using StaffManagmentNET.StaticServices;
using System.Text;

namespace StaffManagmentNET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectedDB") ?? "");
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<DataContext>()
                            .AddDefaultTokenProviders()
                            .AddRoles<IdentityRole>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;

                options.User.RequireUniqueEmail = true;
            });

            builder.Services.AddScoped<IAccountRepo, AccountService>();
            builder.Services.AddScoped<ITimeSheetRepo, TimeSheetService>();
            builder.Services.AddScoped<IDivisionRepo, DivisionService>();
            builder.Services.AddScoped<INotification, NotificationService>();
            builder.Services.AddScoped<IDeviceRepo, DeviceService>();

            builder.Services.AddSingleton<JWTManager>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opts =>
            {
                opts.SaveToken = true;
                opts.RequireHttpsMetadata = true;
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(builder.Configuration["JWT:SecretKey"] ?? "")),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
                opts.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200") 
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}