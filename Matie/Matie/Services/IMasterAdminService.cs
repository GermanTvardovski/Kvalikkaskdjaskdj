namespace Matie.Services;

public interface IMasterAdminService
{
    Task<(bool ok, string? error)> UpdateQualificationLevelAsync(int masterId, string? newLevel, CancellationToken cancellationToken = default);
}
