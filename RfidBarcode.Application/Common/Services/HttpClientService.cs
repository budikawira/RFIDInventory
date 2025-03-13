namespace RfidBarcode.Application.Services
{
    using Newtonsoft.Json;
    using RfidBarcode.Application.Common.BaseObjects;
    using RfidBarcode.Application.Operationals.Requests;
    using RfidBarcode.Application.Operationals.ViewModels;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class HttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> PostAsync(string url, string jsonPayload, string token)
        {
            // Set the Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Set the Content-Type header to application / json
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await _httpClient.PostAsync(url, content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Read and return the response content
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<BaseObjectResponse<List<TrackingItemVM>>> RequestTrackingItemAsync(
            String url, SyncTrackingItemRequest param, String token)
        {
            var response = new BaseObjectResponse<List<TrackingItemVM>>();
            try
            {
                var jsonPayload = JsonConvert.SerializeObject(param);
                var result = await PostAsync(url + "/api/Sync/StockOut", jsonPayload, token);
                var res = JsonConvert.DeserializeObject<BaseObjectResponse<List<TrackingItemVM>>>(result);
                if (res != null)
                {
                    response = res;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;

        }

    }
}
