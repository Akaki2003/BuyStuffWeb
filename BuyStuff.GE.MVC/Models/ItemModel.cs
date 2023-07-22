using BuyStuff.GE.Domain.Images;

namespace BuyStuff.GE.MVC.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public List<Image> Images { get; set; }
    }
}
