using Lydian.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Data.Abstract
{
    public interface IProductRepository
    {
        public List<Product> GetAll();
        public Product Get(int productId);
        public List<Product> GetByCategoryId(int categoryId);
        public Product AddProduct(Product newProduct);
        public List<Category> GetCategories();
    }
}
