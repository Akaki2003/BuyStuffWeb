using BuyStuff.GE.Domain.Images;
using BuyStuff.GE.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Domain.Items
{
    public class Item
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public List<Image> Images { get; set; }
        public User User { get; set; }
    }
}
