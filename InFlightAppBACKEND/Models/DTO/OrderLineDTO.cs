using InFlightAppBACKEND.Models.Domain;

namespace InFlightAppBACKEND.Models.DTO{
    public class OrderLineDTO{
        public ProductDTO Product { get; set; }
        public int Amount { get; set; }

        public OrderLineDTO(){}

        public OrderLineDTO(OrderLine ol){
            Amount = ol.Amount;
            Product = new ProductDTO(ol.Product);
        }
    }
}
