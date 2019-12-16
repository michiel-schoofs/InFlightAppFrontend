using InFlightAppBACKEND.Models.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace InFlightAppBACKEND.Models.DTO
{
    public class ProductDTO{
        public int ProductID { get; set; }
        public int? ImageID { get; set; }

        [Range(0.0, Double.PositiveInfinity, ErrorMessage = "You can't enter a negative price")]
        public decimal ProductPrice { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for the name")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The name needs to be between 2 - 100 characters in length")]
        public string Name { get; set; }

        public ProductDTO(){}

        public ProductDTO(Product prod){
            ProductID = prod.ProductId;
            Name = prod.Name;
            ProductPrice = prod.UnitPrice;
            ImageID = prod.ImageID;
        }
    }
}
