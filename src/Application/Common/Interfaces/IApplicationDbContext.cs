using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {

        DbSet<Domain.Entities.User> Users { get; set; }

        DbSet<Domain.Entities.Company> Companies { get; set; }

        DbSet<Domain.Entities.Kiosk> Kiosks { get; set; }
        DbSet<Domain.Entities.Wallet> Wallets { get; set; }
        DbSet<Domain.Entities.Transaction> Transactions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
