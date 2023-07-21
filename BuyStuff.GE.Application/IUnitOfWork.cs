using BuyStuff.GE.Application.Images.Repositories;
using BuyStuff.GE.Application.Items.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Application
{
    public interface IUnitOfWork
    {
        Task Save(CancellationToken cancellationToken);
        IItemRepository Item { get; }
        IImageRepository Image { get; }
    }
}
