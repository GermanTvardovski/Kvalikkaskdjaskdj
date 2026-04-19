using System;
using System.Collections.Generic;

namespace Matie.Data;

public partial class Service
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[]? ImageData { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Servicemaster> Servicemasters { get; set; } = new List<Servicemaster>();
}
