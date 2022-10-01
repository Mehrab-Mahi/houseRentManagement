﻿using InvoicePro.Application.Helper;
using InvoicePro.Application.Interfaces;
using InvoicePro.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InvoicePro.Web.Controllers
{
    [InvoiceProAuth]
    [Route("assetstatus")]
    public class AssetStatusController : Controller
    {
        private readonly IAssetStatusService _service;
        public AssetStatusController(IAssetStatusService service)
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

        [HttpGet("get/{id}")]
        public IActionResult GetById(string id)
        {
            var list = _service.GetById(id);
            return Ok(new { data = list });
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] AssetStatusVm model)
        {
            var data = _service.Create(model);
            return Ok(new { data });
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(string id, [FromBody] AssetStatusVm model)
        {
            var data = _service.Update(id, model);
            return Ok(new { data });
        }
    }
}
