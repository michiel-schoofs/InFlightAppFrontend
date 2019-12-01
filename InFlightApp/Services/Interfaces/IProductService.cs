using InFlightApp.Model;
using System.Threading.Tasks;

namespace InFlightApp.Services.Interfaces{
    public interface IProductService{
        string[] GetCategories();
        Task<Product[]> GetProducts(ProductType pt);
        bool AddToStock(int productID, int restock);
        Task<string> GetImage(int productID);
    }
}
