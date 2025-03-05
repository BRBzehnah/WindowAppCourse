
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;

namespace TaskManagerCourse.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    /*НА СЛУЧАЙ ЕСЛИ БОЛЕЕ ЛАКОНИЧНОЕ РЕШЕНИЕ НЕ ПОДОЙДЕТ*/

                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // укзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        // строка, представляющая издателя
                        ValidIssuer = AuthOptions.ISSUER,

                        // будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        // установка потребителя токена
                        ValidAudience = AuthOptions.AUDIENCE,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,

                        // установка ключа безопасности
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                    /*БОЛЕЕ ЛАКОНИЧНОЕ РЕШЕНИЕ by Gemini*/
                    //options.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    ValidateIssuer = true, // Проверять, что издатель токена (issuer) соответствует ожидаемому значению.
                    //    ValidateAudience = true, // Проверять, что аудитория токена (audience) соответствует ожидаемому значению.
                    //    ValidateLifetime = true, // Проверять, что срок действия токена (expiration time) не истек.
                    //    ValidateIssuerSigningKey = true, // Проверять, что подпись токена (signature) действительна и соответствует ожидаемому ключу.
                    //    ValidIssuer = builder.Configuration["Jwt:Issuer"], // Получаем издателя из конфигурации
                    //    ValidAudience = builder.Configuration["Jwt:Audience"], // Получаем аудиторию из конфигурации
                    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Получаем ключ из конфигурации
                    //};
                });


            // Add services to the container.

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
