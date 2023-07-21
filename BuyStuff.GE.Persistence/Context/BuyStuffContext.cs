using BuyStuff.GE.Domain.Images;
using BuyStuff.GE.Domain.Items;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Persistence.Context
{
    public class BuyStuffContext : DbContext
    {
        public BuyStuffContext(DbContextOptions<BuyStuffContext> options) : base(options)
        {

        }

        //DbSets
        public DbSet<Item> Items { get; set; }
        public DbSet<Image> Images{ get; set; }

    }
}
