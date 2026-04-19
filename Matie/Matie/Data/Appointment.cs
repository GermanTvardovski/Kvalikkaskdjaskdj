using System;
using System.Collections.Generic;

namespace Matie.Data;

public partial class Appointment
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ServiceId { get; set; }

    public int? MasterId { get; set; }

    public DateTime? Date { get; set; }

    public int? QueueNumber { get; set; }

    public virtual Master? Master { get; set; }

    public virtual Service? Service { get; set; }

    public virtual User? User { get; set; }
}
