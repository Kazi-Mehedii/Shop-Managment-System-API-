using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopManagment.Core.DTOs;
using ShopManagment.Services;

namespace ShopManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _supplierService;

        public SupplierController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]

        public async Task<ActionResult> GetSupplier()
        {
            var suppliers = await _supplierService.GetAllSupplier();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task <ActionResult> GetSupplierByID(int id)
        {
            var supplier = await _supplierService.GetByIDSupplier(id);
            if (supplier is null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }


        [HttpPost]
        public async Task<ActionResult> CreateSupplier([FromBody] SupplierDTO supplierDTO)
        {
            await _supplierService.AddSupplier(supplierDTO);
            return CreatedAtAction(nameof(GetSupplierByID), new { id = supplierDTO.Id }, supplierDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSupplier(int id, [FromBody] SupplierDTO supplierDTO)
        {
            if (id != supplierDTO.Id)
            {
                return BadRequest();
            }

            await _supplierService.UpdateSupplier(supplierDTO);
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            await _supplierService.DeleteSupplier(id);
            return NoContent();
        }


    }
}
