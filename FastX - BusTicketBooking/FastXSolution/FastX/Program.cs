using FastX.Contexts;
using FastX.Interfaces;
using FastX.Models;
using FastX.Repositories;
using FastX.Services;
using Microsoft.EntityFrameworkCore;

namespace FastX
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<FastXContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("fastx"));
            });

            builder.Services.AddScoped<IRouteeService, RouteeService>();
            builder.Services.AddScoped<IBusService, BusService>();

            builder.Services.AddScoped<IRepository<int, Routee>, RouteeRepository>();
            builder.Services.AddScoped<IRepository<int, Bus>, BusRepository>();




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