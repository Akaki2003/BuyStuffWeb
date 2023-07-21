using BuyStuff.GE.Domain.Images;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BuyStuff.GE.MVC.Models
{
    public class ItemEditModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Please enter a 9-digit number.")]
        public string PhoneNumber { get; set; }
        public List<Image> CurrentImages { get; set; }
        public List<IFormFile?> Images { get; set; }
    }
}
