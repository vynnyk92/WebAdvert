using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAdvert.Api.Services
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAdvertStorageService _advertStorageService;

        public StorageHealthCheck(IAdvertStorageService advertStorageService)
        {
            _advertStorageService = advertStorageService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var isOk = await _advertStorageService.CheckTableExist();
            return isOk ? HealthCheckResult.Healthy("Table exist") : HealthCheckResult.Unhealthy("Table absent");
        }
    }
}
