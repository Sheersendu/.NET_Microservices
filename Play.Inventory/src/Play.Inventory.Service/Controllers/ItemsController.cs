using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{
	[Route("items")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		private readonly IRepository<InventoryItem> itemsRepository;
		private readonly CatalogClient catalogClient;

		public ItemsController(IRepository<InventoryItem> itemsRepository, CatalogClient catalogClient)
		{
			this.itemsRepository = itemsRepository;
			this.catalogClient = catalogClient;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetAsync([FromQuery] Guid userId)
		{
			if (userId == Guid.Empty)
			{
				return BadRequest($"No user found with Id: {userId}");
			}
			var catalogItems = await catalogClient.GetCatalogItemsAsync();
			var inventoryItemEntities = await itemsRepository.GetAllAsync(item => item.UserId == userId);

			var inventoryItemDTOs = inventoryItemEntities.Select(inventoryItem =>
			{
				var catalogItem = catalogItems.SingleOrDefault(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
				return inventoryItem.AsDTO(catalogItem.Name, catalogItem.Description);
			});
			return Ok(inventoryItemDTOs);
		}

		[HttpPost]
		public async Task<ActionResult> CreateAsync([FromBody] GrantItemsDTO item)
		{
			var inventoryItem = await itemsRepository.GetAsync(existingItem => existingItem.UserId == item.UserID && existingItem.CatalogItemId == item.CatalogItemId);
			if (inventoryItem == null)
			{
				inventoryItem = new InventoryItem
				{
					UserId = item.UserID,
					CatalogItemId = item.CatalogItemId,
					Quantity = item.Quantity,
					AcquiredDate = DateTimeOffset.UtcNow
				};
				await itemsRepository.CreateAsync(inventoryItem);
			}
			else
			{
				inventoryItem.Quantity += item.Quantity;
				await itemsRepository.UpdateAsync(inventoryItem);
			}

			return Ok();
		}
	}
}