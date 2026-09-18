using GameDay_Sync.Data;
using GameDay_Sync.Extensions;
using GameDay_Sync.Extensions.DependencyInjection;
using GameDay_Sync.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDataLayer(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddAdminApi();
builder.Services.AddControllers();

const string adminFrontendCorsPolicy = "AdminFrontendCors";
string[] defaultAllowedFrontendOrigins =
[
    "http://localhost:3000",
    "https://gamedaysync.vercel.app"
];
var allowedFrontendOrigins = builder.Configuration
    .GetSection("AdminFrontend:AllowedOrigins")
    .Get<string[]>() ?? defaultAllowedFrontendOrigins;

builder.Services.AddCors(options =>
{
    options.AddPolicy(adminFrontendCorsPolicy, policy =>
    {
        policy.WithOrigins(allowedFrontendOrigins)
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
    });
});

var app = builder.Build();

if (args.Length > 0)
{
    using var scope = app.Services.CreateScope();
    var pipeline = scope.ServiceProvider.GetRequiredService<Pipeline>();
    await pipeline.RunPipeline(args[0]);
    return;
}

app.UseHttpsRedirection();

app.UseCors(adminFrontendCorsPolicy);

app.MapControllers();

app.Run();