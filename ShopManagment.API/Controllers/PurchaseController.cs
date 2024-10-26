using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopManagment.Core.DTOs;
using ShopManagment.Services;

namespace ShopManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;

        public PurchaseController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDTO>>> GetAllPurchase()
        {
            var purchase = await _purchaseService.GetAllPurchase();
            return Ok(purchase);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetpurchaseById(int id)
        {
            var purchase = await _purchaseService.GetPurchaseById(id);
            return Ok(purchase);
        }

        [HttpPost]
        public async Task <ActionResult> CreatePurchase([FromBody] PurchaseDTO purchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdPurchase = await _purchaseService.CreatePurchase(purchase);
                return CreatedAtAction(nameof(GetpurchaseById), new { id = createdPurchase.Id }, createdPurchase);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePurchase(int id, [FromBody] PurchaseDTO purchaseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != purchaseDTO.Id)
            {
                return Ok("Purchase Id is mismatch");
            }

            try
            {
                await _purchaseService.UpdatePurchase(purchaseDTO);
                return NoContent();

            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task <ActionResult> Deletepurchase(int id)
        {
            try
            {
                await _purchaseService.DeletePurchase(id);
                return NoContent();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

    }
}
