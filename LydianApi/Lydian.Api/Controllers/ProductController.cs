using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Lydian.Business.Abstract;
using Lydian.Entities.Dto;
using Lydian.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Lydian.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            var responseList = _service.GetAll();
            var mappedList = responseList.Select(x => _mapper.Map<ProductDto>(x)).ToList();
            return Ok(mappedList);
        }

        [HttpGet("Get/{productId}")]
        public ActionResult Get(int productId)
        {
            try
            {
                var responseProduct = _service.Get(productId);
                var productDto = _mapper.Map<ProductDto>(responseProduct);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }
        



        [HttpPost("AddProduct")]
        public ActionResult AddProduct([FromServices] IValidator<ProductAddDto> validator)
        {
            try
            {
                var productName = Request.Form["productName"];
                var price = Request.Form["price"];
                var description = Request.Form["description"];
                var categoryId = Request.Form["categoryId"];
                IFormFile productImage = Request.Form.Files["productImage"];

                var productAddDto = new ProductAddDto()
                {
                    ProductName = productName,
                    Price = Convert.ToDouble(price),
                    Description = description,
                    ImagePath = "Failed upload",
                    CategoryId = Convert.ToInt32(categoryId)
                };

                ValidationResult validationResult = validator.Validate(productAddDto);
                if (!validationResult.IsValid)
                {
                    var modelStateDictionary = new ModelStateDictionary();

                    foreach (ValidationFailure failure in validationResult.Errors)
                    {
                        modelStateDictionary.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }

                    return ValidationProblem(modelStateDictionary);
                }

                var response = _service.AddProduct(productAddDto, productImage);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }


        [HttpGet("GetByCategoryId")]
        public ActionResult GetProductsByCategoryId(int categoryId)
        {
            var responseList = _service.GetByCategory(categoryId);
            var mappedList = responseList.Select(x => _mapper.Map<ProductDto>(x)).ToList();
            return Ok(mappedList);
        }

        [HttpGet("GetByCategoryName")]
        public ActionResult GetProductsByCategoryName(string categoryName)
        {
            try
            {
                var responseList = _service.GetByCategory(categoryName);
                var mappedList = responseList.Select(x => _mapper.Map<ProductDto>(x)).ToList();
                return Ok(mappedList);
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonResponse() { Data = ex.Message });
            }        
        }





    }
}
