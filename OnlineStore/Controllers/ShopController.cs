using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Exceptions;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {
        public IProductService _productService;
        public IPurchaseService _purchaseService;

        public ShopController(IProductService productService, IPurchaseService purchaseService)
        {
            _productService = productService;
            _purchaseService = purchaseService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Products>>>> GetAsync()
        {
            try
            {
                var products = await _productService.GetProductsAsync();
                var response = new ApiResponse<IEnumerable<Products>>
                {
                    Status = "success",
                    Data = products,
                    Count = products.Count(),
                    Error = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<Products>>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<ApiResponse<Products>>> GetByIdAsync(string id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product != null)
                {

                    var response = new ApiResponse<Products>
                    {
                        Status = "success",
                        Data = product,
                        Error = null
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<Products>
                    {
                        Status = "fail",
                        Data = null,
                        Error = $"Product with ID {id} not found."
                    };
                    return NotFound(response);
                }

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<Products>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        //[Authorize]
        [HttpPost(Name = "AddProduct")]
        public IActionResult AddProduct(Products product)
        {
            try
            {
                _productService.AddProducts(product);
                var response = new ApiResponse<Products>
                {
                    Status = "success",
                    Data = product,
                    Error = null
                };
                return CreatedAtAction(nameof(GetByIdAsync), new { id = product.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpPut("BuyProduct")]
        public async Task<ActionResult<ApiResponse<string>>> BuyProductByIdAsync(string ProductId, int Quantity)
        {
            try
            {
                await _productService.BuyProductAsync(ProductId, Quantity);

                var response = new ApiResponse<string>
                    {
                        Status = "success",
                        Data = "Product Purchased succesfully",
                        Count = null,
                        Error = null
                    };
                    return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ApiResponse<string>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(404, response);
            }
            catch (NotEnoughStock ex)
            {
                var response = new ApiResponse<string>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(409, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Status = "fail",
                    Data = null,
                    Count = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        //[Authorize]
        [HttpDelete("DeleteProduct")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteProductAsync(string productId)
        {
            try
            {
                    await _productService.DeleteProductAsync(productId);
                    var response = new ApiResponse<string>
                    {
                        Status = "success",
                        Data = null,
                        Error = null
                    };
                    return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ApiResponse<string>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(404, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        //[Authorize]
        [HttpGet("GetPurchaseRecord")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Purchase>>>> GetSalesAsync()
        {
            try
            {
                var sales = await _purchaseService.GetPurchaseRecord();
                var response = new ApiResponse<IEnumerable<Purchase>>
                {
                    Status = "success",
                    Data = sales,
                    Count = sales.Count(),
                    Error = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<Purchase>>
                {
                    Status = "fail",
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }


    }
}
