using System;
using System.Collections.Generic;

namespace Matie.Data;

public partial class Servicemaster
{
    public int Id { get; set; }

    public int? ServiceId { get; set; }

    public int? MasterId { get; set; }

    public virtual Master? Master { get; set; }

    public virtual Service? Service { get; set; }
}
