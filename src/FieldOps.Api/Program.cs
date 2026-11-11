var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
    options.AddPolicy("web", policy => policy
        .WithOrigins(builder.Configuration["Web:Origin"] ?? "http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("web");

app.MapHealthChecks("/health");

app.Run();

public partial class Program;
