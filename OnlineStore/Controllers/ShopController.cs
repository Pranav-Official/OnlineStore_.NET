using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<Products>>> Get()
        {
            try
            {
                var products = _productService.GetProducts();
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

        //[Authorize]
        [HttpGet("{id}", Name = "GetProductById")]
        public ActionResult<ApiResponse<Products>> GetById(string id)
        {
            try
            {
                var product = _productService.GetProductById(id);
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
                return CreatedAtAction(nameof(GetById), new { id = product.ID }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpPut("BuyProduct")]
        public ActionResult<ApiResponse<string>> BuyProductById(string ProductId, int Quantity)
        {
            try
            {
                var result = _productService.BuyProduct(ProductId, Quantity);

                if (result == "Purchase successful")
                {
                    var response = new ApiResponse<string>
                    {
                        Status = "success",
                        Data = result,
                        Count = null,
                        Error = null
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<string>
                    {
                        Status = "fail",
                        Data = null,
                        Count = null,
                        Error = result
                    };
                    return BadRequest(response);
                }
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
        public ActionResult<ApiResponse<string>> DeleteProduct(string productId)
        {
            try
            {
                var product = _productService.GetProductById(productId);

                if (product != null)
                {
                    var result = _productService.DeleteProduct(productId);
                    var response = new ApiResponse<string>
                    {
                        Status = "success",
                        Data = result,
                        Error = null
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<string>
                    {
                        Status = "fail",
                        Data = null,
                        Error = $"Product with ID {productId} not found."
                    };
                    return NotFound(response);
                }
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
        public ActionResult<ApiResponse<IEnumerable<Purchase>>> GetSales()
        {
            try
            {
                var sales = _purchaseService.GetPurchaseRecord();
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
