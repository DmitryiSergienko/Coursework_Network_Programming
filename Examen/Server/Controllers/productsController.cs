using Microsoft.AspNetCore.Mvc;
using DataLayer.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class productsController : ControllerBase
    {
        private readonly Service _service;

        public productsController(Service service)
        {
            _service = service;
        }

        // GET api/products/top3
        [HttpGet("top3")]
        public async Task<IActionResult> GetTop3Products()
        {
            try
            {
                var result = await _service.GetTop3ProductsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET api/products/portion
        [HttpGet("portion")]
        public async Task<IActionResult> GetProductsInPortions(
            [FromQuery] int skipRows = 0,
            [FromQuery] int showRows = 10)
        { 
            try
            {
                var result = await _service.GetShowProductsInPortionsAsync(skipRows, showRows);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}