using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopManagment.Services.Interface;

namespace ShopManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportInterface _reportInterface;

        public ReportController(IReportInterface reportInterface)
        {
           _reportInterface = reportInterface;
        }

        [HttpGet("{type}")]
        public IActionResult GetReports(string type) 
        {
            var reports = _reportInterface.GenarateReport(type);
            if (reports == null || !reports.Any()) {
                return NotFound("Report Not Found");
            }

            return Ok(reports);
        }
    }
}
