using GameDay_Sync.Model;
using GameDay_Sync.Models;
using GameDay_Sync.Repositories;

namespace GameDay_Sync.Services;

public interface ISiteVisitsService
{
    Task RecordVisitAsync(RecordVisitRequest request, string? ipAddress, string? userAgent);
    Task<VisitStatsDto> GetStatsAsync();
}

public class SiteVisitsService(ISiteVisitsRepository siteVisitsRepository) : ISiteVisitsService
{
    public Task RecordVisitAsync(RecordVisitRequest request, string? ipAddress, string? userAgent)
    {
        var siteVisit = new SiteVisit
        {
            VisitorId = request.VisitorId,
            PagePath = request.PagePath,
            Referrer = request.Referrer,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            VisitedAt = DateTime.UtcNow,
        };

        return siteVisitsRepository.AddAsync(siteVisit);
    }

    public async Task<VisitStatsDto> GetStatsAsync()
    {
        var totalVisits = await siteVisitsRepository.GetTotalCountAsync();
        var uniqueVisitors = await siteVisitsRepository.GetUniqueVisitorCountAsync();
        var visitsToday = await siteVisitsRepository.GetTodayCountAsync();
        var recentVisits = await siteVisitsRepository.GetRecentAsync(50);

        return new VisitStatsDto(
            totalVisits,
            uniqueVisitors,
            visitsToday,
            recentVisits.Select(ToDto).ToList());
    }

    private static SiteVisitDto ToDto(SiteVisit v) => new(
        v.Id,
        v.VisitorId,
        v.PagePath,
        v.IpAddress,
        v.Referrer,
        v.VisitedAt);
}
