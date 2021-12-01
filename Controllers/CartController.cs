using DAW_MP2.Data;
using DAW_MP2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAW_MP2.Controllers
{
 

    public class CartController : Controller
    {

        private readonly AuthDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private ICartData _cartData;

        public CartController(AuthDbContext context, UserManager<IdentityUser> userManager, ICartData cartData)
        {
            _context = context;
            _userManager = userManager;
            _cartData = cartData;


        }

        public IActionResult Cart()
        {
            if(User.Identity.IsAuthenticated)
            {
                Guid id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                _cartData.CreateCart(id);

               List<CartRow> productList = _cartData.GetCart(id).Result;

                return View(productList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        public IActionResult AddToCart(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                _cartData.AddToCart(id, userId);

                return RedirectToAction("Cart", "Cart");
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }
        }

        public IActionResult Add(Guid id)
        {

            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _cartData.AddToCart(id, userId);
                return RedirectToAction("Cart", "Cart");
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }

          
        }

        public IActionResult Remove(Guid id)
        {

            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _cartData.RemoveFromCart(id, userId);
                return RedirectToAction("Cart", "Cart");
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }

          
        }

        public IActionResult Delete(Guid id)
        {

            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _cartData.RemoveAllFromCart(id, userId);
                return RedirectToAction("Cart", "Cart");
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            
        }

        public IActionResult Payment()
        {

            if (User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                Cart cart = _cartData.Payment(userId);
                return View(cart);
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }

           
        }

        [HttpPost]
        public IActionResult Payment(Cart cart)
        {

            if (User.Identity.IsAuthenticated)
            {

                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _cartData.FinishTransaction(userId, cart);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return LocalRedirect("/Identity/Account/Login");
            }

        }
    }
}
