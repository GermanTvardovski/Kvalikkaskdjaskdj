using Matie.Data;
using Matie.Models;

namespace Matie.Services;

public interface IServiceRepository
{
    Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Master>> GetMastersAsync(CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<ServiceCardModel> Items, int TotalCount)> GetServicesPageAsync(
        string? searchText,
        int? categoryId,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<Service?> GetServiceByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<int>> GetMasterIdsForServiceAsync(int serviceId, CancellationToken cancellationToken = default);
    Task<int> AddServiceAsync(Service service, IReadOnlyList<int> masterIds, CancellationToken cancellationToken = default);
    Task UpdateServiceAsync(Service service, IReadOnlyList<int> masterIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Service>> GetAllServicesAsync(CancellationToken cancellationToken = default);
}
