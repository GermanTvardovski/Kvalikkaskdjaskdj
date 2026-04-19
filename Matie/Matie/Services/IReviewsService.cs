using Matie.Data;
using Matie.Models;

namespace Matie.Services;

public interface IReviewsService
{
    Task<IReadOnlyList<ReviewRowModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(bool ok, string? error)> AddAsync(int userId, int serviceId, int rating, string? comment, CancellationToken cancellationToken = default);
}
