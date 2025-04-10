using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Guid userId, string message, CancellationToken cancellationToken);
    }
}
