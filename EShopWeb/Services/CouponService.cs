using EShopWeb.Models;
using EShopWeb.Services.Contracts;
using System.Text.Json;

namespace EShopWeb.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _factory;
        private readonly JsonSerializerOptions _options;
        private const string apiEndpoint = "/api/coupon";
        private CouponViewModel couponVM = new();

        public CouponService(IHttpClientFactory factory)
        {
            _factory = factory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
        }

        public async Task<CouponViewModel> GetDiscountCoupon(string couponCode, string token)
        {
            var client = _factory.CreateClient("DiscountApi");
            PutTokenInHeaderAuthorization(token, client);

            using (var response = await client.GetAsync($"{apiEndpoint}/{couponCode}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    couponVM = await JsonSerializer.DeserializeAsync<CouponViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return couponVM;
        }

        private void PutTokenInHeaderAuthorization(string token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
