using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStuff.GE.Application.Items.Requests
{
    public class ItemRequestModel
    {
        [Required]
        public string Title { get; set; }
        public List<IFormFile?> Images { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Please enter a 9-digit number.")]

        public string PhoneNumber { get; set; }
    }
}
