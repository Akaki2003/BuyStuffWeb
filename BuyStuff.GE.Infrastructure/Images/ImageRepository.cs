using BuyStuff.GE.Application.Images.Repositories;
using BuyStuff.GE.Domain.Images;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Infrastructure.Images
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        public ImageRepository(DbContext context) : base(context)
        {
        }

        public async Task<Image> AddImage(Image image, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(image, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return image;
        }

        public async Task<List<Image>> GetImagesByItemId(int itemId, CancellationToken cancellationToken)
        {
            var images = await _dbSet
                .Where(x => x.Items.Any(x => x.Id == itemId)).ToListAsync(cancellationToken);
            return images;
        }

        public async Task<Image> GetImageById(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task RemoveImagesByItemId(int itemId, CancellationToken cancellationToken)
        {
            var images = await _dbSet
                 .Where(x => x.Items.Any(x => x.Id == itemId)).ToListAsync(cancellationToken);

            _dbSet.RemoveRange(images);
        }
    }
}
