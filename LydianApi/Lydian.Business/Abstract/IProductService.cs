using Lydian.Entities.Dto;
using Lydian.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Business.Abstract
{
    public interface IProductService
    {
        public List<Product> GetAll();
        public Product Get(int productId);
        public List<Product> GetByCategory(int categoryId);
        public List<Product> GetByCategory(string categoryName);
        public Product AddProduct(ProductAddDto productAddDto, IFormFile productImage);
    }
}
