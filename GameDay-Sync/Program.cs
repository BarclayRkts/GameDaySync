using GameDay_Sync.Data;
using GameDay_Sync.Extensions;
using GameDay_Sync.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDataLayer(builder.Configuration);
builder.Services.AddServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var pipeline = scope.ServiceProvider.GetRequiredService<Pipeline>();
    string alertChoice = args.Length > 0 ? args[0] : "--weekly";
    
    await pipeline.RunPipeline(alertChoice);
};

app.UseHttpsRedirection();


app.Run();