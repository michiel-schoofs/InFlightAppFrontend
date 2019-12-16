using InFlightAppBACKEND.Models.Domain;
using System.Collections.Generic;

namespace InFlightAppBACKEND.Data.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAllByCategory(ProductType type);
        Product GetById(int id);

        Image GetImageFromId(int id);
        Image GetDefaultImage();
        
        void Add(Product product);
        void Remove(Product product);
        void SaveChanges();
    }
}
