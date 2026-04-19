namespace Matie.Services;

public interface IMasterProfileService
{
    Task<int?> TryResolveMasterIdByUserNameAsync(string? userName, CancellationToken cancellationToken = default);
    Task<(bool ok, string? message)> SubmitQualificationRequestAsync(int masterId, CancellationToken cancellationToken = default);
}
