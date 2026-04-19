using System;
using System.Collections.Generic;

namespace Matie.Data;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
