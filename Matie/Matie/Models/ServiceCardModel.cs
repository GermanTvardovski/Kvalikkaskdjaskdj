namespace Matie.Models;

public sealed class ServiceCardModel
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public string MastersSummary { get; init; } = "";
    public string CategoryName { get; init; } = "";
    public byte[]? ImageData { get; init; }
}
