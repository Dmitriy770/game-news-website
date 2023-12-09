using GameNews.MediaStorage.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseCors(cors =>
{
    cors.AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowAnyHeader();
});

app.MapControllers();

app.Run();