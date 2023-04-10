global using EntityFramework_DotNet7_SQLServer.Models;
global using EntityFramework_DotNet7_SQLServer.Services.CharacterService;
global using EntityFramework_DotNet7_SQLServer.Dtos.Character;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using EntityFramework_DotNet7_SQLServer.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ICharacterService, CharacterService>();

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