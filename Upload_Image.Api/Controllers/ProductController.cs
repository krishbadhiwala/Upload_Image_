using Azure.Core;
using Image_manupulation.Data.Models;
using Image_manupulation.Data.Models.Dtos;
using Image_manupulation.Data.Repository;
using Image_manupulation.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Image_Manupulation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly FileService _fileService;
        private readonly ProductRepository _productRepo;


        public ProductController(FileService fileService, ProductRepository productRepo)
        {
            _fileService = fileService;
            _productRepo = productRepo;
            
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = _productRepo.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> getproductsbyid(int id)
        {
            try
            {
                var product = await _productRepo.GetProductById(id);
                if (product == null)
                {
                    return NotFound("Product not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromForm] Insert_ProductDto productToAdd)
        {
            try
            {
                if (productToAdd.ImageFile?.Length > 2 * 1024 * 1024)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
                }

                //var product = new Product { ProductName = productToAdd.ProductName };
                //_context.Products.Add(product);
                //await _context.SaveChangesAsync();
                string[] allowedFileExtentions = new[] { ".jpg", ".jpeg", ".png" };
                string createdImageName = await _fileService.SaveFileAsync(productToAdd.ImageFile, allowedFileExtentions);

                string imageUrl = $"{Request.Scheme}://{Request.Host}/Uploads/{createdImageName}";


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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productRepo.GetProductById(id);
                var imageUrl = product?.ProductImage;

                if (product == null)
                {
                    return NotFound("Product not found");
                }

                var deletedProduct = await _productRepo.DeleteProduct(product);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    string fileName = Path.GetFileName(new Uri(imageUrl).LocalPath);

                    _fileService.DeleteFile(fileName);
                }

                return Ok(deletedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] Insert_ProductDto productToUpdate)
        {
            try
            {
                var existingProduct = await _productRepo.GetProductById(id);
                if (existingProduct == null)
                {
                    return NotFound("Product not found");
                }

                // If a new image file is uploaded, delete the old file and save the new one
                string imageUrl = existingProduct.ProductImage;
                if (productToUpdate.ImageFile != null)
                {
                    if (productToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        return BadRequest("File size should not exceed 1 MB");
                    }

                    string[] allowedFileExtensions = new[] { ".jpg", ".jpeg", ".png" };

                    // Delete old file if exists
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        string oldFileName = Path.GetFileName(new Uri(imageUrl).LocalPath);
                        _fileService.DeleteFile(oldFileName);
                    }

                    // Save new file
                    string newImageName = await _fileService.SaveFileAsync(productToUpdate.ImageFile, allowedFileExtensions);
                    imageUrl = $"{Request.Scheme}://{Request.Host}/Uploads/{newImageName}";
                }

                // Update product properties
                existingProduct.ProductName = productToUpdate.ProductName;
                existingProduct.ProductImage = imageUrl;

                // Save changes
                var updatedProduct = await _productRepo.UpdateProduct(existingProduct);

                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}
