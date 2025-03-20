using Image_manupulation.Data.Models;
using Image_manupulation.Data.Models.Dtos;
using Image_manupulation.Data.Repository;
using Image_manupulation.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Image_Manupulation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly FileService _fileService;
        private readonly ProductRepository _productRepo;
        private readonly ILogger<ProductController> _logger;

        public ProductController(FileService fileService, ProductRepository productRepo, ILogger<ProductController> logger)
        {
            _fileService = fileService;
            _productRepo = productRepo;
            _logger = logger;
        }



        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = _productRepo.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] Insert_ProductDto productToAdd)
        {
            try
            {
                if (productToAdd.ImageFile?.Length > 1 * 1024 * 1024)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
                }



                string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
                string createdImageName = await _fileService.SaveFileAsync(productToAdd.ImageFile, allowedFileExtentions);


                string imageUrl = $"{Request.Scheme}://{Request.Host}/Uploads/{createdImageName}";




                // mapping `ProductDTO` to `Product` manually. You can use automapper.
                var product = new Product
                {
                    ProductName = productToAdd.ProductName,
                    ProductImage = imageUrl
                };
                var createdProduct = await _productRepo.AddProduct(product);
                return CreatedAtAction(nameof(CreateProduct), new
                {
                    createdProduct.Id,
                    createdProduct.ProductName,
                    ImageUrl = imageUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




    }
}
