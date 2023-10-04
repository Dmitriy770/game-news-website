using GameNews.OAuth.Api.Middlewares;
using GameNews.OAuth.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDiscordApi();
builder.Services.AddServices();

builder.Services.AddTransient<CheckAuthorizationMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(cors =>
{
    cors.AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowAnyHeader();
});

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/v1/oauth2/me"),
    appBuilder => appBuilder.UseMiddleware<CheckAuthorizationMiddleware>());
app.MapControllers();

app.Run();