using ApiToDatabase.Data;
using ApiToDatabase.Models;
using ApiToDatabase.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.Configure<UserDatabaseSettings>(
//    builder.Configuration.GetSection("UserDatabase"));
builder.Services.AddScoped<IUserService, UserServicePostgres>();

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<UserDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));


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

app.UseAuthorization();

app.MapControllers();

app.Run();
