using BuyStuff.GE.Application.Items.Requests;
using BuyStuff.GE.Application.Items.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Application.Items
{
    public interface IItemService
    {
        Task<int> CreateItem(ItemRequestModel item, string UserId, CancellationToken cancellationToken);
        Task DeleteItem(int id, CancellationToken cancellationToken);
        Task<List<ItemResponseModel>> GetAllItems(CancellationToken cancellationToken);
        Task<ItemResponseModel> GetItemById(int id, CancellationToken cancellationToken);
        Task UpdateItem( ItemRequestPutModel item, CancellationToken cancellationToken);
    }
}
