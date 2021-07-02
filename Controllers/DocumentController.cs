using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CeluGamaSystem.Dtos;
using CeluGamaSystem.Models;
using CeluGamaSystem.Services;

namespace CeluGamaSystem.Controllers
{
    [Authorize]
    [Route("api/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly OrderExcelReportService _excelService;

        const string MIMETYPE = "application/octet-stream";

        public DocumentController(IOrderService service)
        {
            _service = service;
            _excelService = new OrderExcelReportService();
        }

        [HttpGet("orders")]
        public FileResult GetExcelOrders([FromQuery] SearchOrder SearchOrder)
        {
            List<Order> Orders = _service.SearchOrders(SearchOrder);
            MemoryStream stream = _excelService.WriteExcelFile(Orders, SearchOrder.ShippingType);

            return File(stream, MIMETYPE, _excelService.GetRandomFileName());
        }
    }
}