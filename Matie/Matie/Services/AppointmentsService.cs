using Matie.Data;
using Microsoft.EntityFrameworkCore;

namespace Matie.Services;

public sealed class AppointmentsService : IAppointmentsService
{
    private readonly IDbContextFactory<DBCon> dbContextFactory;

    public AppointmentsService(IDbContextFactory<DBCon> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<(bool ok, int? queueNumber, string? error)> BookAsync(
        int userId,
        int serviceId,
        int masterId,
        DateTime appointmentDateUtc,
        CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var day = appointmentDateUtc.Date;
        var dayStart = PostgresDateTime.ForNaiveTimestamp(day);
        var dayEnd = PostgresDateTime.ForNaiveTimestamp(day.AddDays(1));

        var maxQueueNullable = await db.Appointments
            .Where(a =>
                a.MasterId == masterId &&
                a.Date.HasValue &&
                a.Date.Value >= dayStart &&
                a.Date.Value < dayEnd)
            .MaxAsync(a => (int?)a.QueueNumber, cancellationToken);

        var next = (maxQueueNullable ?? 0) + 1;

        var appointment = new Appointment
        {
            UserId = userId,
            ServiceId = serviceId,
            MasterId = masterId,
            Date = PostgresDateTime.ForNaiveTimestamp(appointmentDateUtc),
            QueueNumber = next
        };

        db.Appointments.Add(appointment);
        await db.SaveChangesAsync(cancellationToken);

        return (true, next, null);
    }

    public async Task<IReadOnlyList<Appointment>> GetForMasterAsync(int masterId, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Appointments
            .AsNoTracking()
            .Include(a => a.User)
            .Include(a => a.Service)
            .Where(a => a.MasterId == masterId)
            .OrderBy(a => a.Date)
            .ToListAsync(cancellationToken);
    }
}
