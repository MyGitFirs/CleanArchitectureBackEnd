using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Services
{
    public class WalletResetService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WalletResetService> _logger;

        public WalletResetService(IServiceScopeFactory scopeFactory, ILogger<WalletResetService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                    var today = DateTime.UtcNow;
                    var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

                    if (today.Day == 1) 
                    {
                        _logger.LogInformation("Resetting wallet balances for the new month...");

                        var wallets = await context.Wallets.ToListAsync(stoppingToken);

                        foreach (var wallet in wallets)
                        {
                            wallet.Balance = wallet.MonthlyLimit; 
                            wallet.LastResetDate = firstDayOfMonth;
                            wallet.UpdatedDate = DateTime.UtcNow;
                        }

                        await context.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Wallet balances reset successfully.");
                    }
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Check once a day
            }
        }
    }
}
