using System;
using System.IO;
using System.Net;
using System.Linq;
using RestSharp;
using CeluGamaSystem.Models;
using CeluGamaSystem.Platform.Clients;

namespace CeluGamaSystem.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly CeluGamaDbContext _context;

        public ShipmentService(CeluGamaDbContext context)
        {
            _context = context;
        }

        public MemoryStream GetLabels(string shipment_ids)
        {
            MercadoLibreClient client = new MercadoLibreClient();
            Token meliEntity = _context.Tokens.First();

            IRestResponse Response = client.GetShippingLabel(shipment_ids.Split(','), meliEntity.AccessToken);

            if (Response.StatusCode.Equals(HttpStatusCode.OK))
            {
                MemoryStream ms = new MemoryStream(Response.RawBytes);
                return ms;
            }
            else
            {
                _context.Logs.Add(new Log()
                {
                    StatusCode = (int)Response.StatusCode,
                    Error = Response.StatusDescription,
                    Message = Response.Content,
                    StackTrace = "",
                    Resource = $"/shipment_labels?shipment_ids={shipment_ids}&response_type=pdf",
                    Level = "Warning",
                    CreatedAt = DateTime.UtcNow
                });
                _context.SaveChanges();
            }

            return null;
        }
    }
}
