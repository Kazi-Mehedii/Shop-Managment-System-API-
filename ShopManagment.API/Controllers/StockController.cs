using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopManagment.Core.DTOs;
using ShopManagment.Services;

namespace ShopManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockService _stockService;

        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockDTO>>> GetAllStock()
        {
            var stocks = await _stockService.GetAllStocks();
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockDTO>> GetStockById(int id)
        {
            var stock = await _stockService.GetStockByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock);
        }
    }
}
