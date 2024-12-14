using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Service
{
	public record GrantItemsDTO([Required] Guid UserID, [Required] Guid CatalogItemId, [Range(1, 10)] int Quantity);
	public record InventoryItemDTO([Required] Guid CatalogItemId, int Quantity, DateTimeOffset AcquiredDate);
}