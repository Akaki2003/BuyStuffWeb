using BuyStuff.GE.Domain.Images;
using BuyStuff.GE.Domain.Items;
using BuyStuff.GE.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Persistence.Context
{
    public class BuyStuffContext : IdentityDbContext<User>
    {
        public BuyStuffContext(DbContextOptions<BuyStuffContext> options) : base(options)
        {

        }

        //DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Image> Images{ get; set; }

    }
}
