using BookAPI.Application;
using BookAPI.Persistance;
using BookAPI.Application.Validators.Customer;
using FluentValidation.AspNetCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using System.Collections.ObjectModel;
using System.Security.Claims;

namespace BookAPI.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Cors
            builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            //IoC
            builder.Services.AddHttpContextAccessor();//clienten gelen requestlerikatmanlarda ula�mama�z� sa�lar IHttpContextAccessor
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistanceServices();
            builder.Services.AddInfrastructureServices();

            
            // Add services to the container.
            builder.Services.AddControllers().AddFluentValidation(conf => conf.RegisterValidatorsFromAssemblyContaining<CreateCustomer>());
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("ADM�N",opt =>
                {
                    opt.TokenValidationParameters = new()
                    {
                        ValidateAudience = false, //hagni originlerin ula�abilece�i
                        ValidateIssuer = false,//token kim da��t�yor
                        ValidateLifetime = true,//token ge�erlilik s�resi
                        ValidateIssuerSigningKey = true,

                        LifetimeValidator = ( notBefore, expires, securityToken, validationParameters) => expires != null ? expires>DateTime.UtcNow:false,
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SigningKey"])),

                        NameClaimType =ClaimTypes.Name
                        
                    };
                });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}