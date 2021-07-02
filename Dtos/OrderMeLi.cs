using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class OrderMeLi
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("order_items")]
        public OrderItemMeLi[] OrderItems { get; set; }

        [JsonProperty("date_created")]
        public DateTimeOffset DateCreated { get; set; }

        [JsonProperty("date_closed")]
        public DateTimeOffset DateClosed { get; set; }

        [JsonProperty("total_amount")]
        public decimal TotalAmount { get; set; }

        [JsonProperty("paid_amount")]
        public decimal PaidAmount { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("payments")]
        public PaymentMeLi[] Payments { get; set; }

        [JsonProperty("buyer")]
        public UserMeLi Buyer { get; set; }

        [JsonProperty("pack_id")]
        public long? PackId { get; set; }

        [JsonProperty("shipping")]
        public ShippingMeLi Shipping { get; set; }

        [JsonProperty("taxes")]
        public TaxesMeLi Taxes { get; set; }

        public decimal TotalAmountWithShipping()
        {
            decimal TotalWithShipping = TotalAmount;
            if (Taxes != null && Taxes.Amount.HasValue)
            {
                TotalWithShipping += (decimal)Taxes.Amount;
            }

            if (Shipping != null && Shipping.ShippingOption != null && Shipping.ShippingOption.Cost.HasValue)
            {
                TotalWithShipping += (decimal)Shipping.ShippingOption.Cost;
            }

            return TotalWithShipping;
        }
    }
}
