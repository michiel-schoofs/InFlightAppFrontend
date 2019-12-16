using InFlightAppBACKEND.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.Domain
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductType Type { get; set; }
        public int Amount { get; set; }

        public Image Image { get; set; }
        public int? ImageID { get; set; }

        //EF constructor
        public Product() { }

        public Product(string name, string description, decimal unitPrice, ProductType type,int amount=100){
            Name = name;
            Description = description;
            UnitPrice = unitPrice;
            Type = type;
            Amount = amount;
        }

        public void ChangeProduct(ProductDetailDTO prod) {
            Name = prod.Name;
            Description = prod.Description;
            UnitPrice = prod.ProductPrice;
            Type = Enum.Parse<ProductType>(prod.Category.ToUpper());

            if (prod.Amount > 0) {
                Amount = prod.Amount;
            }

        }

        public void Restock(int amount) { 
            if (amount < 0)
                throw new ArgumentException("Please provide a positive value");

            Amount = amount;
        }

        public void Order(int amount) { 
            if(Amount-amount<0)
                throw new ArgumentException("You can't order that much");

            Amount -= amount;
        }
    }
}
