using System.Collections.Generic;
using System.Linq;
using InFlightAppBACKEND.Data.Repositories;
using InFlightAppBACKEND.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using InFlightAppBACKEND.Models.Domain;

namespace InFlightAppBACKEND.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase{
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepo){
            _productRepository = productRepo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDetailDTO>> GetAllProducts()
        {
            return _productRepository.GetAll().Select(p => new ProductDetailDTO(p)).ToList();
        }

        [Authorize(Policy = "Crew")]
        [HttpPost]
        public ActionResult AddProduct(ProductDetailDTO dto)
        {
            if (!dto.CategoryIsValid())
                return BadRequest("The category you specified doesn't exist");

            Product prod = new Product();
            prod.ChangeProduct(dto);

            _productRepository.Add(prod);
            _productRepository.SaveChanges();

            return Created($"api/Products/{prod.ProductId}", prod);
        }


        [Route("categories")]
        [HttpGet]
        public ActionResult<string[]> GetCategories() {
            return Enum.GetNames(typeof(ProductType));
        }

        [Route("categories/{category}")]
        [HttpGet]
        public ActionResult<IEnumerable<ProductDetailDTO>> GetAllProductsByCategory(string category) {
            if (!Enum.GetNames(typeof(ProductType)).Contains(category.ToUpper()))
                return BadRequest($"{category} isn't a known category");

            return _productRepository.GetAllByCategory(Enum.Parse<ProductType>(category.ToUpper()))
                                    .Select(p => new ProductDetailDTO(p)).ToList();                    
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult<ProductDetailDTO> GetProduct(int id) {
            Product prod = _productRepository.GetById(id);
            
            if (prod == null)
                return NotFound("We couldn't find the product you're looking for");

            return new ProductDetailDTO(prod);
        }

        [Authorize(Policy = "Crew")]
        [Route("{id}")]
        [HttpPut]
        public ActionResult UpdateProduct(int id, ProductDetailDTO pd)
        {
            if (!pd.CategoryIsValid())
                return BadRequest("The category you specified doesn't exist");

            Product prod = _productRepository.GetById(id);
            if (prod == null)
                return NotFound("This product doesn't exist");

            prod.ChangeProduct(pd);
            _productRepository.SaveChanges();

            return Ok();
        }

        [Route("{id}/image")]
        [HttpGet]
        public ActionResult<Image> GetImage(int id) {
            Product prod = _productRepository.GetById(id);

            if (prod == null)
                return BadRequest("The product you specified doesn't exist");

            Image img = _productRepository.GetImageFromId(id);

            if (img == null)
                img = _productRepository.GetDefaultImage();

            return img;
        }

        [Authorize(Policy = "Crew")]
        [HttpPut("{id}/restock/{amount}")]
        public ActionResult RestockProduct(int id,int amount) {
            Product prod = _productRepository.GetById(id);

            if (prod == null)
                return NotFound("We couldn't find the product you're looking for");

            if (amount <= 0)
                return BadRequest("Please provide a strict positive amount for restock");

            prod.Amount += amount;
            _productRepository.SaveChanges();

            return Ok();
        }
    }
}