using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{
	[Route("items")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		private readonly IRepository<InventoryItem> itemsRepository;

		public ItemsController(IRepository<InventoryItem> itemsRepository)
		{
			this.itemsRepository = itemsRepository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetAsync([FromQuery] Guid userId)
		{
			if (userId == Guid.Empty)
			{
				return BadRequest($"No user found with Id: {userId}");
			}
			var items = (await itemsRepository.GetAllAsync(item => item.UserId == userId))
						.Select(item => item.AsDTO());
			return Ok(items);
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