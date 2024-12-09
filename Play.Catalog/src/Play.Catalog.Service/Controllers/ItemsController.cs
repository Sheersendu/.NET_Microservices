using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;

namespace Play.Catalog.Service.Controllers
{
    [Route("items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDTO> items = new()
        {
            new ItemDTO(Guid.NewGuid(), "Health Potion", "Restores small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDTO(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
            new ItemDTO(Guid.NewGuid(), "Bronze sword", "Deals small amount of damage", 20, DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDTO> GetItems()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ItemDTO GetItemById([FromRoute] Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDTO> AddItem([FromBody] CreateItemDTO itemDTO)
        {
            var item = new ItemDTO(Guid.NewGuid(), itemDTO.name, itemDTO.description, itemDTO.price, DateTimeOffset.UtcNow);
            items.Add(item);
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

    }
}