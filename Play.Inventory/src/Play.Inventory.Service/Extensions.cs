using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service
{
	public static class Extensions
	{
		public static InventoryItemDTO AsDTO(this InventoryItem inventoryItem)
		{
			return new InventoryItemDTO(inventoryItem.CatalogItemId, inventoryItem.Quantity, inventoryItem.AcquiredDate);
		}
	}
}