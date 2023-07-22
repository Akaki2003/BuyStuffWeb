using BuyStuff.GE.Application.Images.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Application.Items.Responses
{
    public class ItemResponseModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public List<ImageResponseModel> Images{ get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
    }
}
