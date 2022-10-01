using InvoicePro.Application.Helper;
using InvoicePro.Application.Interfaces;
using InvoicePro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicePro.Web.Controllers
{
    [InvoiceProAuth]
    [Route("producttype")]
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeService _service;

        public ProductTypeController(IProductTypeService service)
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

        [HttpGet("getProductType/{id}")]
        public IActionResult GetById(string id)
        {
            var product = _service.GetById(id);
            return Ok(new { data = product });
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] ProductType model)
        {
            var data = _service.Create(model);
            return Ok(new { data });
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(string id, [FromBody] ProductType model)
        {
            var data = _service.Update(id, model);
            return Ok(new { data });
        }
    }
}
