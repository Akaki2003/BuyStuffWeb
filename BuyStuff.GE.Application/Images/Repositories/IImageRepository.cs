using BuyStuff.GE.Domain.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Application.Images.Repositories
{
    public interface IImageRepository
    {
        Task<Image> AddImage(Image image, CancellationToken cancellationToken);
        Task<Image> GetImageById(int id, CancellationToken cancellationToken);
        Task<List<Image>> GetImagesByItemId(int itemId, CancellationToken cancellationToken);
        Task RemoveImagesByItemId(int itemId, CancellationToken cancellationToken);
    }
}
