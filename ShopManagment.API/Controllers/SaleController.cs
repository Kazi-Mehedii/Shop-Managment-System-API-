using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopManagment.Core.DTOs;
using ShopManagment.Services;

namespace ShopManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly SaleService _saleService;

        public SaleController(SaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDTO>>> GetAllSale()
        {
            try
            {
                var sale = await _saleService.GettAllSale();
                return Ok(sale);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving sales: {ex.Message}");
            }            
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetSaleByID(int id)
        {

            try
            {
                var sale = await _saleService.GetSaleById(id);
                if (sale == null)
                {
                    return NotFound($"Sale With Id {id} not found");
                }
                return Ok(sale);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retriving sale: {ex.Message}");
            }

           
        }

        [HttpPost]
        public async Task<ActionResult> CrateSale([FromBody] SaleDTO saleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var sale = await _saleService.CreateSale(saleDto);
                return CreatedAtAction(nameof(GetSaleByID), new { id = sale.Id }, sale);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating sale: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSale(int id, [FromBody] SaleDTO sale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sale.Id)
            {
                return BadRequest("Sale Id Mismatch");
            }

            try
            {
                await _saleService.UpdateSale(sale);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating sale: {ex.Message}");
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSale(int id)
        {
            try
            {
                var sale = await _saleService.GetSaleById(id);
                if (sale== null)
                {
                    return NotFound($"Sale with ID {id} not found.");
                }
                await _saleService.DeleteSale(id);
                return NoContent();
            }
            catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting sale: {ex.Message}"); }
        }
    }
}
