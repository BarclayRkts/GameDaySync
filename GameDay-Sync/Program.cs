using GameDay_Sync.Extensions;
using GameDay_Sync.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddServices();

var app = builder.Build();

var pipeline = app.Services.GetRequiredService<Pipeline>();
await pipeline.RunPipeline();

app.UseHttpsRedirection();


app.Run();