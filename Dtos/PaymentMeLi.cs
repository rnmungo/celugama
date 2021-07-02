using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class PaymentMeLi
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("payment_type")]
        public string PaymentType { get; set; }

        [JsonProperty("payment_method_id")]
        public string PaymentMethodId { get; set; }

        [JsonProperty("operation_type")]
        public string OperationType { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("card_id")]
        public long? CardId { get; set; }

        [JsonProperty("payer_id")]
        public long PayerId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("status_detail")]
        public string StatusDetail { get; set; }

        [JsonProperty("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [JsonProperty("taxes_amount")]
        public decimal? TaxesAmount { get; set; }

        [JsonProperty("shipping_cost")]
        public decimal? ShippingCost { get; set; }

        [JsonProperty("overpaid_amount")]
        public decimal? OverpaidAmount { get; set; }

        [JsonProperty("total_paid_amount")]
        public decimal TotalPaidAmount { get; set; }
    }
}
