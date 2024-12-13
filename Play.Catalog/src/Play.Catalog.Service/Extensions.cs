using Play.Catalog.Service.DTOs;
using Play.Catalog.Service.Entities;

namespace Catalog.Service
{
	public static class Extensions
	{
		public static ItemDTO AsDto(this Item item)
		{
			return new ItemDTO(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
		}
	}
}