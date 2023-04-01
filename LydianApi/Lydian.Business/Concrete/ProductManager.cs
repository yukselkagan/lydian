using AutoMapper;
using Lydian.Business.Abstract;
using Lydian.Business.General;
using Lydian.Data.Abstract;
using Lydian.Entities.Dto;
using Lydian.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public ProductManager(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<Product> GetAll()
        {
            var responseList = _repository.GetAll();
            return responseList;
        }

        public Product Get(int productId)
        {
            var responseProduct = _repository.Get(productId);
            return responseProduct;
        }

        public List<Product> GetByCategory(int categoryId)
        {
            var responseList = _repository.GetByCategoryId(categoryId);
            return responseList;
        }

        public List<Product> GetByCategory(string categoryName)
        {
            var categories = _repository.GetCategories();
            var category = categories.Where(x => x.CategoryName.ToLower() == categoryName.ToLower()).FirstOrDefault();
            var categoryId = category.CategoryId;
            var responseList = _repository.GetByCategoryId(categoryId);
            return responseList;
        }


        public Product AddProduct(ProductAddDto productAddDto, IFormFile productImage)
        {
            var mappedProduct = _mapper.Map<Product>(productAddDto);           

            string targetFolder = "Stock/ProductImages";

            var fileName = productImage.FileName;
            var fileExtension = Path.GetExtension(fileName);

            if(productImage.Length <= 0)
            {
                throw new Exception("File is empty"); 
            }
            if(fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
            {
                throw new Exception("Wrong extension");
            }

            var createdUniqueName = GeneralManager.CreateUniqueName();
            var newFileName = createdUniqueName + fileExtension;
            var fullPath = Path.Combine(targetFolder, newFileName);

            using(var stream = new FileStream(fullPath, FileMode.Create))
            {
                productImage.CopyTo(stream);
                mappedProduct.Image = newFileName;
            }

            var responseProduct = _repository.AddProduct(mappedProduct);
            return responseProduct;
        }



    }
}
