using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MulliganDeck.Infrastructure.Scryfall;

public class ScryfallSyncWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ScryfallSyncWorker> _logger;

    public ScryfallSyncWorker(IServiceScopeFactory scopeFactory, ILogger<ScryfallSyncWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);

            try
            {
                _logger.LogInformation("Iniciando sincronización con Scryfall...");

                using var scope = _scopeFactory.CreateScope();
                var importer = scope.ServiceProvider.GetRequiredService<ScryfallImporter>();
                var count = await importer.ImportBulkAsync();

                _logger.LogInformation("Sincronización completada. {Count} cartas importadas.", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la sincronización con Scryfall.");
            }
        }
    }
}