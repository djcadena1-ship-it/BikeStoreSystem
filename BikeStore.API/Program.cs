using Microsoft.EntityFrameworkCore;
using BikeStore.Datos;

var builder = WebApplication.CreateBuilder(args);

// agregar servicios al contenedor

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


// configuramos HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
