using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Service
{
	public record GrantItemsDTO([Required] Guid UserID, [Required] Guid CatalogItemId, [Range(1, 10)] int Quantity);
	public record InventoryItemDTO([Required] Guid CatalogItemId, string Name, string Description, int Quantity, DateTimeOffset AcquiredDate);
	public record CatalogItemDTO(Guid Id, string Name, string Description);
}