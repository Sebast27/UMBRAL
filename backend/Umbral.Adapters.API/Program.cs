using Microsoft.EntityFrameworkCore;
using Umbral.Adapters.API.Hubs; 
using Umbral.Adapters.API.Middleware;
using Umbral.Application.TriviaModule.Handlers;
using Umbral.Application.Common.Interfaces;
using Umbral.Domain.TriviaModule.Repositories;
using Umbral.Domain.Common.Interfaces;
using Umbral.Infrastructure.Persistence;
using Umbral.Infrastructure.Persistence.Repositories.TriviaModule;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR
builder.Services.AddSignalR();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTriviaCommandHandler).Assembly));

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repositories
builder.Services.AddScoped<ITriviaRepository, TriviaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")  // URLs del frontend
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();  // ← Importante para SignalR
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.MapControllers();

// Map SignalR hubs
app.MapHub<GameHub>("/hubs/game");

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.Run();