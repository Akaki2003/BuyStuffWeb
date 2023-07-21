using BuyStuff.GE.Application.Items.Repositories;
using BuyStuff.GE.Domain.Items;
using BuyStuff.GE.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BuyStuff.GE.Infrastructure.Items
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {

        public ItemRepository(BuyStuffContext context) : base(context)
        {
        }

        public async Task<int> Create(Item item, CancellationToken cancellationToken)
        {
            await AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return item.Id;
        }


        public async Task<List<Item>> GetAllItems(CancellationToken cancellationToken)
        {
            return await _dbSet.Where(it=>!it.IsDeleted).Include(it=>it.Images).ToListAsync();
        }

        public async Task<Item> GetItemById(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.Include(it=>it.Images).SingleOrDefaultAsync(x=>x.Id==id && !x.IsDeleted,cancellationToken);
        } 
        public async Task<Item> GetItemByIdUntracked(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().SingleOrDefaultAsync(x=>x.Id==id && !x.IsDeleted,cancellationToken);
        }

        public async Task<List<Item>> GetItemsByName(string name, CancellationToken cancellationToken)
        {
            var filteredItems = await _dbSet.Where(item => item.Title == name).ToListAsync();
            return filteredItems;
        }

        public async Task Update(Item item, CancellationToken cancellationToken)
        {
            if (item != null)
            {
                await UpdateAsync(item, cancellationToken);
            }
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var itemToDelete = await GetItemById(id, cancellationToken);
            itemToDelete.IsDeleted = true;
            await UpdateAsync(itemToDelete, cancellationToken );
        }

    }
}
