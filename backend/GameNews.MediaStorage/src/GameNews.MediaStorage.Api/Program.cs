using GameNews.MediaStorage.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapControllers();

app.Run();