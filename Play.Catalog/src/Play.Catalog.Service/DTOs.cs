namespace Play.Catalog.Service.DTOs
{
    public record ItemDTO(Guid Id, string name, string description, decimal price, DateTimeOffset createdDate);
    public record CreateItemDTO(string name, string description, decimal price);

    public record UpdateItemDTO(string name, string description, decimal price);
}