using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    public interface IItemsRepository
    {
        Task CreateAsync(Item entity);
        Task<Item> GetAsync(Guid id);
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task RemoveAsync(Guid id);
        Task UpdateAsync(Item entity);
    }
}