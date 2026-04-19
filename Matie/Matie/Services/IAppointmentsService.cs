using Matie.Data;

namespace Matie.Services;

public interface IAppointmentsService
{
    Task<(bool ok, int? queueNumber, string? error)> BookAsync(
        int userId,
        int serviceId,
        int masterId,
        DateTime appointmentDateUtc,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Appointment>> GetForMasterAsync(int masterId, CancellationToken cancellationToken = default);
}
