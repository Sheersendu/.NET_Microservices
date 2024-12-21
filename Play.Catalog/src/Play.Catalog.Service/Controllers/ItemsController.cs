using Catalog.Service;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers
{
	[Route("items")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		private readonly IRepository<Item> itemsRepository;

		public ItemsController(IRepository<Item> itemsRepository)
		{
			this.itemsRepository = itemsRepository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItemsAsync()
		{
			var items = (await itemsRepository.GetAllAsync())
						.Select(item => item.AsDto());

			return Ok(items);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ItemDTO>> GetItemById([FromRoute] Guid id)
		{
			var item = await itemsRepository.GetAsync(id);
			if (item == null)
			{
				return NotFound();
			}
			return item.AsDto();
		}

		[HttpPost]
		public async Task<ActionResult<ItemDTO>> AddItemAsync([FromBody] CreateItemDTO itemDTO)
		{
			var item = new Item()
			{
				Name = itemDTO.name,
				Description = itemDTO.description,
				Price = itemDTO.price,
				CreatedDate = DateTimeOffset.UtcNow
			};
			await itemsRepository.CreateAsync(item);
			return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateItemAsync([FromRoute] Guid id, [FromBody] UpdateItemDTO itemDTO)
		{
			var existingItem = await itemsRepository.GetAsync(id);
			if (existingItem == null)
			{
				return NotFound();
			}
			existingItem.Name = itemDTO.name;
			existingItem.Description = itemDTO.description;
			existingItem.Price = itemDTO.price;

			await itemsRepository.UpdateAsync(existingItem);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteItemAsync([FromRoute] Guid id)
		{
			var existingItem = await itemsRepository.GetAsync(id);
			if (existingItem == null)
			{
				return NotFound();
			}
			await itemsRepository.RemoveAsync(id);
			return NoContent();
		}
	}
}