using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CeluGamaSystem.Services
{
    public interface IShipmentService
    {
        MemoryStream GetLabels(string shipment_ids);
    }
}
