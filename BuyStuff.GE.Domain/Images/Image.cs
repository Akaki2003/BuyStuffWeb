using BuyStuff.GE.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Domain.Images
{
    public class Image
    {
        public int Id { get; set; }
        public string ImgName { get; set; }
        public string ImagePath { get; set; }
        public List<Item> Items { get; set; }
    }
}
