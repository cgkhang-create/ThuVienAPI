using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;
using web2labthuchanh.Data;
using web2labthuchanh.Repositories;

namespace web2labthuchanh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(
                    "Logs/Book_log.txt",
                    rollingInterval: RollingInterval.Minute)
                .MinimumLevel.Information()
                .CreateLogger();

            builder.Services.AddSerilog();

           
            builder.Services.AddControllers();

            
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Book API",
                        Version = "v1"
                    });

              
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Nhập JWT token"
                    });

                options.AddSecurityRequirement(document =>
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference(
                            "Bearer",
                            document)] = []
                    });
            });

           
            var connectionString =
                builder.Configuration.GetConnectionString(
                    "DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(
                options =>
                    options.UseSqlServer(connectionString));
            
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<
                IBookRepository,
                SQLBookRepository>();

            builder.Services.AddScoped<
                IAuthorRepository,
                SQLAuthorRepository>();

            builder.Services.AddScoped<
                IPublisherRepository,
                SQLPublisherRepository>();

            builder.Services.AddScoped<
                ITokenRepository,
                TokenRepository>();
            builder.Services.AddScoped<
                IImageRepository,
                LocalImageRepository>();

            var bookAuthConnection =
                builder.Configuration.GetConnectionString(
                    "BookAuthConnection");

            builder.Services.AddDbContext<BookAuthDbContext>(
                options =>
                    options.UseSqlServer(bookAuthConnection));

           
            builder.Services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Book")
                .AddEntityFrameworkStores<BookAuthDbContext>()
                .AddDefaultTokenProviders();

            
            builder.Services.Configure<IdentityOptions>(
                option =>
                {
                    option.Password.RequireDigit = false;
                    option.Password.RequireLowercase = false;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                    option.Password.RequiredLength = 6;
                    option.Password.RequiredUniqueChars = 1;
                });

           

            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            // Kiểm tra cấu hình JWT
            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new Exception(
                    "Không tìm thấy Jwt:Key trong appsettings.json");
            }

            if (string.IsNullOrWhiteSpace(jwtIssuer))
            {
                throw new Exception(
                    "Không tìm thấy Jwt:Issuer trong appsettings.json");
            }

            if (string.IsNullOrWhiteSpace(jwtAudience))
            {
                throw new Exception(
                    "Không tìm thấy Jwt:Audience trong appsettings.json");
            }

            builder.Services
                .AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtIssuer,
                            ValidAudience = jwtAudience,

                            ClockSkew = TimeSpan.Zero,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(jwtKey))
                        };
                });

            

            var app = builder.Build();

            

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}