using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CeluGamaSystem.Services;
using CeluGamaSystem.Exceptions;

namespace CeluGamaSystem.Controllers
{
    [Authorize]
    [Route("api/shipments")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _service;
        const string MIMETYPE = "application/pdf";

        public ShipmentController(IShipmentService service)
        {
            _service = service;
        }

        [HttpGet("{shipment_ids}/labels")]
        public FileResult GetLabels([FromRoute] string shipment_ids)
        {
            MemoryStream ms = _service.GetLabels(shipment_ids);

            if (ms != null)
            {       
                string filename = string.Format("{0}.pdf", Guid.NewGuid().ToString());
                return File(ms, MIMETYPE, filename);
            }

            throw new ShippmentLabelUnauthorized("El estado del envío no permite esta acción");
        }
    }
}