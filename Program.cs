using Backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var MisOrigenesPermitidos = "_misOrigenesPermitidos";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MisOrigenesPermitidos,
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000",              // Tu Next.js en desarrollo
                    "https://electricidad-mattos.vercel.app" // Tu futuro dominio en producción
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MisOrigenesPermitidos);

app.UseAuthorization();

app.MapControllers();

app.Run();