using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using InFlightAppBACKEND.Models.Domain;

namespace InFlightAppBACKEND.Models.DTO
{
    public class ProductDetailDTO:ProductDTO{
        #region Simple Properties
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for the Description")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "The Description needs to be between 2 - 1000 characters in length")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for category")]
        public string Category { get; set; }

        [Range(0,int.MaxValue,ErrorMessage ="Please provide a positive value")]
        public int Amount { get; set; }
        #endregion

        #region Constructor
        public ProductDetailDTO(Product prod):base(prod){
                Description = prod.Description;
                Category = Enum.GetName(typeof(ProductType), prod.Type);
                Amount = prod.Amount;
        }

        public ProductDetailDTO():base(){}
        #endregion

        public bool CategoryIsValid() {
            return Enum.GetNames(typeof(ProductType)).Contains(Category.ToUpper());
        }
    }
}
