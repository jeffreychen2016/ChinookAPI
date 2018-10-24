using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChinookAPI.DataAccess;
using ChinookAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChinookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChinookController : ControllerBase
    {
        private Chinook _chinook;

        public ChinookController()
        {
            _chinook = new Chinook();
        }

        [HttpGet("GetInvoicesBySalesAgentID/{id}")]
        public IActionResult GetInvoicesBySalesAgentID(int id)
        {
            return Ok(_chinook.GetInvoicesBySalesAgentID(id));
        }

        [HttpGet("GetAllInvoices")]
        public IActionResult GetAllInvoices()
        {
            return Ok(_chinook.GetAllInvoices());
        }

        [HttpGet("GetCountOfItemsByInvoiceID/{id}")]
        public IActionResult GetCountOfItemsByInvoiceID(int id)
        {
            return Ok(_chinook.GetCountOfItemsByInvoiceID(id));
        }

        [HttpPost("AddNewInvoice")]
        public IActionResult AddNewInvoice(Invoice invoice)
        {
            return Ok(_chinook.AddNewInvoice(invoice));
        }

        [HttpPut("UpdateEmployeeName")]
        public IActionResult UpdateEmployeeName()
        {
            return Ok();
        }
    }
}