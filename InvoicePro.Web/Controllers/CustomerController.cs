using InvoicePro.Application.Helper;
using InvoicePro.Application.Interfaces;
using InvoicePro.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicePro.Web.Controllers
{
    [InvoiceProAuth]
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var list = _service.GetAll();
            return Ok(new { data = list });
        }

        [HttpGet("getCustomer/{id}")]
        public IActionResult GetById(string id)
        {
            var list = _service.GetById(id);
            return Ok(new { data = list });
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CustomerVm model)
        {
            var data = _service.Create(model);
            return Ok(new { data });
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(string id, [FromBody] CustomerVm model)
        {
            var data = _service.Update(id, model);
            return Ok(new { data });
        }
    }
}
