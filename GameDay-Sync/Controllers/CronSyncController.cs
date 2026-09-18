using System.Security.Cryptography;
using System.Text;
using GameDay_Sync.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameDay_Sync.Controllers;

[ApiController]
[Route("api/cron")]
public class CronSyncController(Pipeline pipeline, IConfiguration configuration) : ControllerBase
{
    private const string CronTokenHeader = "X-Cron-Token";
    private const string CronSecretTokenSetting = "CRON_SECRET_TOKEN";

    [HttpPost("daily")]
    public async Task<IActionResult> RunDaily()
    {
        if (!HasValidCronToken())
        {
            return Unauthorized();
        }

        await pipeline.RunPipeline("--daily");
        return Ok();
    }

    [HttpPost("weekly")]
    public async Task<IActionResult> RunWeekly()
    {
        if (!HasValidCronToken())
        {
            return Unauthorized();
        }

        await pipeline.RunPipeline("--weekly");
        return Ok();
    }

    private bool HasValidCronToken()
    {
        var expectedToken = configuration[CronSecretTokenSetting];

        if (string.IsNullOrEmpty(expectedToken) ||
            !Request.Headers.TryGetValue(CronTokenHeader, out var suppliedTokens) ||
            suppliedTokens.Count != 1)
        {
            return false;
        }

        var expectedTokenBytes = Encoding.UTF8.GetBytes(expectedToken);
        var suppliedTokenBytes = Encoding.UTF8.GetBytes(suppliedTokens[0]!);

        return expectedTokenBytes.Length == suppliedTokenBytes.Length &&
               CryptographicOperations.FixedTimeEquals(expectedTokenBytes, suppliedTokenBytes);
    }
}
