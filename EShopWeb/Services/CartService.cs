using EShopWeb.Models;
using EShopWeb.Services.Contracts;
using System.Text.Json;

namespace EShopWeb.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory _factory;
        private readonly JsonSerializerOptions _options;
        private const string apiEndpoint = "/api/Cart";
        private CartViewModel cartVM = new();

        public CartService(IHttpClientFactory factory)
        {
            _factory = factory;
            _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }

        public async Task<CartViewModel> GetCartByUserIdAsync(string userId, string token)
        {
            var client = _factory.CreateClient("CartApi");
            PutTokenInHeaderAuthorization(token, client);

            using (var response = await client.GetAsync($"{apiEndpoint}/getcart/{userId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    cartVM = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return cartVM;
        }

        public async Task<CartViewModel> AddItemToCartAsync(CartViewModel cartVM, string token)
        {
            var client = _factory.CreateClient("CartApi");
            PutTokenInHeaderAuthorization(token, client);
            StringContent content = new StringContent(JsonSerializer.Serialize(cartVM), System.Text.Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync($"{apiEndpoint}/addcart/", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    cartVM = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return cartVM;
        }
        public async Task<CartViewModel> UpdateCartAsync(CartViewModel cartVM, string token)
        {
            var client = _factory.CreateClient("CartApi");
            PutTokenInHeaderAuthorization(token, client);
            CartViewModel cartUpdate = new();

            using (var response = await client.PutAsJsonAsync($"{apiEndpoint}/updatecart/", cartVM))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    cartUpdate = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return cartUpdate;
        }
        public async Task<bool> RemoveItemFromCartAsync(int cartId, string token)
        {
            var client = _factory.CreateClient("CartApi");
            PutTokenInHeaderAuthorization(token, client);

            using (var response = await client.DeleteAsync($"{apiEndpoint}/deletecart/" + cartId))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
        public Task<bool> ApplyCouponAsync(CartViewModel cartVM, string couponCode, string token)
        {
            throw new NotImplementedException();
        }
        public Task<bool> RemoveCouponAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }
        public Task<bool> ClearCartAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }
        public Task<CartViewModel> CheckoutAsync(CartHeaderViewModel cartHeader, string token)
        {
            throw new NotImplementedException();
        }
        private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
