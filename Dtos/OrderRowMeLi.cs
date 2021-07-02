using System;
using System.Collections.Generic;

namespace CeluGamaSystem.Dtos
{
    public class OrderRowMeLi
    {
        public long OrderIDML { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public decimal TotalAmount { get; set; }
        public string Observations { get; set; }
        public List<ItemRowMeLi> Items { get; set; }
    }
}
