using chess.Components;
using chess.Interfaces;
using chess.Model;
using chess.Service;
using chess.Validators;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IAiService = chess.Interfaces.IAiService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();



builder.Services.AddSingleton<IMoveValidator, MoveValidator>();
builder.Services.AddSingleton<ICheckValidator, CheckValidator>();
builder.Services.AddScoped<GameLogicService>();
builder.Services.AddScoped<IBoard, Board>(); 
builder.Services.AddScoped<ICastlingService, CastlingService>();
builder.Services.AddTransient<IGameStateSerice, GameStateSerice>();
builder.Services.AddTransient<ITimeService, TimeService>();
builder.Services.AddTransient<IAiService, AiService>();
builder.Services.AddTransient<IMoveService, MoveService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();