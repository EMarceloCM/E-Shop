using EShopWeb.Models;
using EShopWeb.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EShopWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProducts(string.Empty);
            if (products == null) return View ("Error");
            
            return View(products);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ProductViewModel>> ProductDetails(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var result = await _productService.FindProductById(id, token);

            if (result == null) return View("Error");

            return View(result);
        }

        [HttpPost]
        [Authorize]
        [ActionName("ProductDetails")]
        public async Task<ActionResult<ProductViewModel>> ProductDetailsPost(ProductViewModel productVM)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            CartViewModel cart = new()
            {
                CartHeader = new CartHeaderViewModel()
                {
                    UserId = User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartItemViewModel cartItem = new()
            {
                Quantity = productVM.Quantity,
                ProductId = productVM.Id,
                Product = await _productService.FindProductById(productVM.Id, token)
            };

            List<CartItemViewModel> cartItemsVM = new List<CartItemViewModel>();
            cartItemsVM.Add(cartItem);
            cart.CartItems = cartItemsVM;

            var result = await _cartService.AddItemToCartAsync(cart, token);

            if (result is not null)
                return RedirectToAction(nameof(Index));

            return View(productVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}