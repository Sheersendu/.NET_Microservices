using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.DTOs
{
    public record ItemDTO(Guid Id, string name, string description, decimal price, DateTimeOffset createdDate);
    public record CreateItemDTO([Required] string name, string description, [Range(0, 1000)] decimal price);
    public record UpdateItemDTO([Required] string name, string description, [Range(0, 1000)] decimal price);
}