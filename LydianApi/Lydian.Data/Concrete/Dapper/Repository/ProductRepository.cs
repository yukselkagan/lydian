using Dapper;
using Lydian.Data.Abstract;
using Lydian.Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Data.Concrete.Dapper.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MsSQL");
        }

        public Product AddProduct(Product newProduct)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO products (ProductName, Price, Image, Description, CategoryId) VALUES (@productName, @price, @image, @description, @categoryId)";
                connection.Execute(sql, newProduct);
                return newProduct;
            }
        }

        public List<Product> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "select * from products";
                var products = connection.Query<Product>(sql);
                return products.ToList();
            }
        }

        public Product Get(int productId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Products WHERE ProductId = @productId ";
                var product = connection.QueryFirstOrDefault<Product>(sql, new { productId = productId });
                return product;
            }
        }


        public List<Product> GetByCategoryId(int categoryId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "select * from products where CategoryId = @categoryId";
                var products = connection.Query<Product>(sql, new {categoryId = categoryId});
                return products.ToList();
            }
        }

        public List<Category> GetCategories()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Categories";
                var categories = connection.Query<Category>(sql);
                return categories.ToList();
            }
        }


    }
}
