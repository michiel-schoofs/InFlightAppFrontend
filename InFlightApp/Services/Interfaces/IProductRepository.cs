using InFlightApp.Model;
using System.Threading.Tasks;

namespace InFlightApp.Services.Interfaces{
    public interface IProductRepository{
        string[] GetCategories();
        Task<Product[]> GetProducts(ProductType pt);
        Task<string> GetImage(int productID);
    }
}
