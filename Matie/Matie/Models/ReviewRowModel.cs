namespace Matie.Models;

public sealed class ReviewRowModel
{
    public int Id { get; init; }
    public string Author { get; init; } = "";
    public string ServiceName { get; init; } = "";
    public int Rating { get; init; }
    public string Comment { get; init; } = "";
}
