using System;
using System.Collections.Generic;

namespace Matie.Data;

public partial class Master
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Level { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Servicemaster> Servicemasters { get; set; } = new List<Servicemaster>();
}
