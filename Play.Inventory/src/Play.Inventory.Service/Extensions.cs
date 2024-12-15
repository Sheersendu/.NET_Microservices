using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service
{
	public static class Extensions
	{
		public static InventoryItemDTO AsDTO(this InventoryItem inventoryItem, string Name, string Description)
		{
			return new InventoryItemDTO(inventoryItem.CatalogItemId, Name, Description, inventoryItem.Quantity, inventoryItem.AcquiredDate);
		}
	}
}