using BuyStuff.GE.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Application.Items.Repositories
{
    public interface IItemRepository
    {
        Task<int> Create(Item item, CancellationToken cancellationToken);
        Task<Item> GetItemById(int id, CancellationToken cancellationToken);
        Task<List<Item>> GetAllItems(CancellationToken cancellationToken);
        Task<List<Item>> GetItemsByName(string name, CancellationToken cancellationToken);
        Task Update(Item item, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<Item> GetItemByIdUntracked(int id, CancellationToken cancellationToken);
    }
}
