using BuyStuff.GE.Application;
using BuyStuff.GE.Application.Images.Repositories;
using BuyStuff.GE.Application.Items.Repositories;
using BuyStuff.GE.Infrastructure.Images;
using BuyStuff.GE.Infrastructure.Items;
using BuyStuff.GE.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BuyStuff.GE.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {

        private BuyStuffContext context;
        private ItemRepository itemRepository;
        private ImageRepository imageRepository;

        public UnitOfWork(DbContextOptions<BuyStuffContext> options)
        {
            context = new BuyStuffContext(options);
        }

        public IItemRepository Item
        {
            get
            {

                if (this.itemRepository == null)
                {
                    this.itemRepository = new ItemRepository(context);
                }
                return itemRepository;
            }
        }


        public IImageRepository Image
        {
            get
            {
                if (this.imageRepository == null)
                {
                    this.imageRepository = new ImageRepository(context);
                }
                return imageRepository;
            }
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        public void Rollback()
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
