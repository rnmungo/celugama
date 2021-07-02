using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using CeluGamaSystem.Exceptions;

namespace CeluGamaSystem.Dtos
{
    public class SearchOrder
    {
        public int LIMIT = 10;

        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "shipping_type")]
        public string ShippingType { get; set; }
    }
}
