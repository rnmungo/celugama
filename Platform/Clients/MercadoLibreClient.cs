using System;
using System.Collections.Generic;
using RestSharp;

namespace CeluGamaSystem.Platform.Clients
{
    public class MercadoLibreClient
    {
        const string BASE_URL = "https://api.mercadolibre.com";
        const string SDK_VERSION = "MELI-NET-SDK-1.0.2";
        const string REDIRECT_URI = "https://www.proyectomas.net/success";

        private readonly IRestClient _client;

        public string SecretKey { get; set; }
        public string ClientId { get; set; }

        public MercadoLibreClient()
        {
            _client = new RestClient(BASE_URL);
        }

        public MercadoLibreClient(string clientId, string secretKey)
        {
            this.ClientId = clientId;
            this.SecretKey = secretKey;
            _client = new RestClient(BASE_URL);
        }

        public IRestResponse Authorize(string code)
        {
            string URI = string.Format("/oauth/token?grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&redirect_uri={3}", ClientId, SecretKey, code, REDIRECT_URI);
            IRestRequest Request = new RestRequest(URI, Method.POST);
            Request.AddHeader("Accept", "application/json");
            return ExecuteRequest(Request);
        }

        public IRestResponse RefreshToken(string RefreshToken)
        {
            string URI = string.Format("/oauth/token?grant_type=refresh_token&client_id={0}&client_secret={1}&refresh_token={2}", ClientId, SecretKey, RefreshToken);
            IRestRequest Request = new RestRequest(URI, Method.POST);
            Request.AddHeader("Accept", "application/json");
            return ExecuteRequest(Request);
        }

        public IRestResponse GetOrder(string Resource, string AccessToken)
        {
            IRestRequest Request = new RestRequest(Method.GET);
            Request.Resource = Resource;
            Request.AddHeader("Accept", "application/json");
            Request.AddHeader("x-format-new", "true");
            Request.AddHeader("Authorization", $"Bearer {AccessToken}");
            return ExecuteRequest(Request);
        }

        public IRestResponse GetNotesFromOrder(string Resource, string AccessToken)
        {
            IRestRequest Request = new RestRequest(Method.GET);
            Request.Resource = $"{Resource}/notes";
            Request.AddHeader("Accept", "application/json");
            Request.AddHeader("Authorization", $"Bearer {AccessToken}");
            return ExecuteRequest(Request);
        }

        public IRestResponse GetShippingLabel(string[] ShippingIds, string AccessToken)
        {
            IRestRequest Request = new RestRequest(Method.GET);
            string ids = string.Join(",", 1);
            Request.Resource = $"/shipment_labels?shipment_ids={ids}&response_type=pdf";
            Request.AddHeader("Authorization", $"Bearer {AccessToken}");
            return ExecuteRequest(Request);
        }

        public IRestResponse GetShipping(string ShippingID, string AccessToken)
        {
            IRestRequest Request = new RestRequest(Method.GET);
            Request.Resource = $"/shipments/{ShippingID}";
            Request.AddHeader("Authorization", $"Bearer {AccessToken}");
            return ExecuteRequest(Request);
        }

        public IRestResponse GetItemVariations(string ItemID, long VariationID)
        {
            IRestRequest Request = new RestRequest(Method.GET);
            Request.Resource = $"/items/{ItemID}/variations/{VariationID}";
            return ExecuteRequest(Request);
        }

        public IRestResponse ExecuteRequest(IRestRequest request)
        {
            _client.UserAgent = SDK_VERSION;
            return _client.Execute(request);
        }
    }
}
