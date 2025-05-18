using MeDesenvolveIA.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();
app.Configure();

app.Run();
