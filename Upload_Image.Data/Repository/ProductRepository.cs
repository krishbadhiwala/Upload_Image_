using Image_manupulation.Data.Data;
using Image_manupulation.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_manupulation.Data.Repository
{
     public class ProductRepository
    {
        private readonly ApplicationDbcontext dbcontext;

        public ProductRepository(ApplicationDbcontext _dbcontext)
        {
            this.dbcontext = _dbcontext;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return dbcontext.Products.ToList();
        }

        //public async Task<Product> GetProductById(int id)
        //{
        //    return dbcontext.Products.Find(id);
        //}

        public async Task<Product> AddProduct(Product product)
        {
            dbcontext.Products.Add(product);
            await dbcontext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            dbcontext.Products.Update(product);
            await dbcontext.SaveChangesAsync();
            return product;
        }
















    }
}
