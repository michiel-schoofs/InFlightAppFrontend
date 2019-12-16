using InFlightAppBACKEND.Data.Mappers;
using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private DbSet<Product> _products;
        private DBContext _dbContext;

        public ProductRepository(DBContext dbContext)
        {
            _products = dbContext.Products;
            _dbContext = dbContext;
        }

        //You don't have to include enum  values, gives error (enums are mapped and retrieved automatically)
        public IEnumerable<Product> GetAll(){
            return _products.ToList();
        }

        public Product GetById(int id){
            return _products.SingleOrDefault(p => p.ProductId == id);
        }

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Remove(Product product)
        {
            _products.Remove(product);
        }

        public IEnumerable<Product> GetAllByCategory(ProductType type){
            return _products.Where(p => p.Type == type).ToList();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public Image GetImageFromId(int id) {
            return _products.Include(p => p.Image).FirstOrDefault(p => p.ProductId == id)?.Image;
        }

        public Image GetDefaultImage(){
            byte[] bytes = File.ReadAllBytes(@"./Data/Seeding/default.jpg");
            return new Image() { Data = Convert.ToBase64String(bytes), Extension = ".jpeg", ID = -1000 };
        }
    }
}
