using InvoicePro.Application.Helper;
using InvoicePro.Application.Interfaces;
using InvoicePro.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicePro.Web.Controllers
{
    [InvoiceProAuth]
    [Route("order")]
    public class OrderController : Controller
    {
        private readonly IOrderService _service;
        private readonly IHostEnvironment _env;

        public OrderController(IOrderService service, IHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("createview")]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpGet("editview/{id}")]
        public IActionResult Edit(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var list = _service.GetAll();
            return Ok(new { data = list });
        }
        
        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] Order model)
        {
            var data = _service.CreateOrder(model);
            return Ok(new { data });
        }
        
        [HttpGet("getallorderstatus")]
        public IActionResult GetAllOrderStatus()
        {
            var list = _service.GetAllOrderStatus();
            return Ok(new { data = list });
        }

        [HttpGet("getById/{id}")]
        public IActionResult GetById(string id)
        {
            var data = _service.GetById(id);
            return Ok(new { data });
        }

        [HttpPost("update/{id}")]
        public IActionResult Update(string id, [FromBody] Order model)
        {
            var data = _service.Update(id, model);
            return Ok(new { data });
        }

        [HttpPost("downloadInvoice/{id}")]
        public IActionResult DownloadInvoice(string id)
        {
            var data = _service.GetInvoiceDataById(id);
            var fileName = "Invoice-" + data.InvoiceNumber + ".pdf";

            var pdf = new ViewAsPdf("Invoice", data)
            {
                FileName = fileName,
                IsLowQuality = false,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                IsJavaScriptDisabled = false
            };

            var pdfData = pdf.BuildFile(ControllerContext).Result;

            System.IO.File.WriteAllBytes(Path.Combine(GetInvoicePdfFolder(), fileName), pdfData);

            return Ok(new { fileName });
        }

        private string GetInvoicePdfFolder()
        {
            var path = Path.Combine(_env.ContentRootPath, "wwwroot/invoice/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
