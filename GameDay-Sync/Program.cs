using GameDay_Sync.Data;
using GameDay_Sync.Extensions;
using GameDay_Sync.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDataLayer(builder.Configuration);
builder.Services.AddServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var pipeline = scope.ServiceProvider.GetRequiredService<Pipeline>();
    await pipeline.RunPipeline();
};

app.UseHttpsRedirection();


app.Run();