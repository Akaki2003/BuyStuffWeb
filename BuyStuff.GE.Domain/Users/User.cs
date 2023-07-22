using BuyStuff.GE.Domain.Items;
using Microsoft.AspNetCore.Identity;

namespace BuyStuff.GE.Domain.Users
{
    public class User : IdentityUser
    {

        public List<Item> Items { get; set; }
    }

   
}
