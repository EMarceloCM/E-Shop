using AutoMapper;
using EShop.CartApi.Context;
using EShop.CartApi.DTOs;
using EShop.CartApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.CartApi.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private IMapper _mapper;

        public CartRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {
            Cart cart = new Cart
            {
                CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId)
            };
            cart.CartItems = _context.CartItems.Where(x => x.CartHeaderId == cart.CartHeader.Id).Include(x => x.Product);

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<bool> DeleteItemCartAsync(int cartItemId)
        {
            try
            {
                CartItem cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.Id == cartItemId);
                int total = _context.CartItems.Where(_x => _x.CartHeaderId == cartItem.CartHeaderId).Count();

                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                if (total == 1)
                {
                    var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.Id == cartItem.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeader);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> CleanCartAsync(string userId)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
            if (cartHeader != null)
            {
                _context.CartItems.RemoveRange(_context.CartItems.Where(x => x.CartHeaderId == cartHeader.Id));
                _context.CartHeaders.Remove(cartHeader);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<CartDTO> UpdateCartAsync(CartDTO cartDTO)
        {
            Cart cart = _mapper.Map<Cart>(cartDTO);

            //salvar produto no banco de nao existir
            await SaveProductInBataBase(cartDTO, cart);

            //verifica se o header é null
            var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cart.CartHeader.UserId);
            if (cartHeader != null)
            {
                await CreateHeaderAndItems(cart);
            }
            else
            {
                await UpdateQuantityAndItems(cartDTO, cart, cartHeader);
            }
            return _mapper.Map<CartDTO>(cart);
        }
        public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
        {
            var cartHeaderApplyCoupon = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cartHeaderApplyCoupon != null)
            {
                cartHeaderApplyCoupon.CouponCode = couponCode;

                _context.CartHeaders.Update(cartHeaderApplyCoupon);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
        public async Task<bool> DeleteCouponAsync(string userId)
        {
            var cartHeaderApplyCoupon = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cartHeaderApplyCoupon != null)
            {
                cartHeaderApplyCoupon.CouponCode = "";

                _context.CartHeaders.Update(cartHeaderApplyCoupon);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        private async Task SaveProductInBataBase(CartDTO cartDTO, Cart cart)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == cartDTO.CartItems.FirstOrDefault().ProductId);
            if (product != null)
            {
                _context.Products.Add(cart.CartItems.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }
        }
        private async Task CreateHeaderAndItems(Cart cart)
        {
            _context.CartHeaders.Add(cart.CartHeader);
            await _context.SaveChangesAsync();

            cart.CartItems.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
            cart.CartItems.FirstOrDefault().Product = null;

            _context.CartItems.Add(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
        private async Task UpdateQuantityAndItems(CartDTO cartDTO, Cart cart, CartHeader? cartHeader)
        {
            var cartDetail = await _context.CartItems.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == cartDTO.CartItems.FirstOrDefault().ProductId && x.CartHeaderId == cartHeader.Id);
            if (cartDetail == null)
            {
                //cria o cartItems
                cart.CartItems.FirstOrDefault().CartHeaderId = cartHeader.Id;
                cart.CartItems.FirstOrDefault().Product = null;
                _context.CartItems.Add(cart.CartItems.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                //atualiza a quantidade de itens
                cart.CartItems.FirstOrDefault().Product = null;
                cart.CartItems.FirstOrDefault().Quantity += cartDetail.Quantity;
                cart.CartItems.FirstOrDefault().Id = cartDetail.Id;
                cart.CartItems.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;
                _context.CartItems.Update(cart.CartItems.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
        }
    }
}
