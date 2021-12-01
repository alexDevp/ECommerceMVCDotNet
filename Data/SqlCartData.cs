using DAW_MP2.Data;
using DAW_MP2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DAW_MP2.Data
{
    public class SqlCartData : ICartData
    {
        private readonly AuthDbContext _context;

        string Baseurl = "https://localhost:44339";
        string key = "hL4bA4nB4yI0vI0fC8fH7eT6";
        public SqlCartData(AuthDbContext context)
        {
            _context = context;
           
        }

        public void CreateCart(Guid id)
        {
            Status status = _context.Status.Where(a => a.StatusName == "No Carrinho").SingleOrDefault();

            if (_context.Carts.Where(a => a.IdUser == id).Where(x => x.IdStatus == status.Id).SingleOrDefault() == null)
            {

                Guid newId = Guid.NewGuid();
                Cart cart = new Cart { Id = newId, IdUser = id, Address = "Sem Endereço", IdStatus = status.Id };
                _context.Add(cart);
                _context.SaveChanges();
            }
        }

        public void AddToCart(Guid itemId, Guid userId)
        {

            CreateCart(userId);


            Status status = _context.Status.Where(a => a.StatusName == "No Carrinho").SingleOrDefault();

            Cart cart = _context.Carts.Where(a => a.IdUser == userId).Where(x => x.IdStatus == status.Id).SingleOrDefault();

            Guid newId = Guid.NewGuid();

            CartItem cartItem = new CartItem { Id = newId, IdCart = cart.Id, IdItem = itemId };
            _context.Add(cartItem);
            _context.SaveChanges();
        }

        public void RemoveFromCart(Guid itemId, Guid userId)
        {
            Status status = _context.Status.Where(a => a.StatusName == "No Carrinho").SingleOrDefault();

            Cart cart = _context.Carts.Where(a => a.IdUser == userId).Where(x => x.IdStatus == status.Id).SingleOrDefault();

            CartItem cartItem = _context.CartItems.Where(x => x.IdCart == cart.Id).Where(x => x.IdItem == itemId).FirstOrDefault();

            _context.Remove(cartItem);
            _context.SaveChanges();

        }

        public void RemoveAllFromCart(Guid itemId, Guid userId)
        {
            Status status = _context.Status.Where(a => a.StatusName == "No Carrinho").SingleOrDefault();

            Cart cart = _context.Carts.Where(a => a.IdUser == userId).Where(x => x.IdStatus == status.Id).SingleOrDefault();

            List<CartItem> cartItems = _context.CartItems.Where(x => x.IdCart == cart.Id).Where(x => x.IdItem == itemId).ToList();

            foreach (CartItem cartItem in cartItems)
            {
                _context.Remove(cartItem);
                _context.SaveChanges();
            }

        }

        public async Task<List<CartRow>> GetCart(Guid id)
        {
            Status status = _context.Status.Where(a => a.StatusName == "No Carrinho").SingleOrDefault();
            Cart cart = _context.Carts.Where(a => a.IdUser == id).Where(x => x.IdStatus == status.Id).SingleOrDefault();
       
            List<CartItem> cartList = _context.CartItems.Where(x => x.IdCart == cart.Id).ToList();

            List<Product> productsList = new List<Product>();

            foreach (CartItem item in cartList)
            {
                using (var client = new HttpClient())
                {

                    Product ProductInfo = new Product();

                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("ApiKey", key);

                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    HttpResponseMessage Res = await client.GetAsync("api/products/" + item.IdItem.ToString());

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        var ProductResponse = Res.Content.ReadAsStringAsync().Result;
                        ProductInfo = JsonConvert.DeserializeObject<Product>(ProductResponse);
                    }

                    productsList.Add(ProductInfo);
                }
            }
       
            return productsList.GroupBy(k => new { k.Id, k.ProductName, k.Image, k.Price, k.Description}, (key, items) => new CartRow {  Id = key.Id, ProductName = key.ProductName,
                Image= key.Image, Price = key.Price, Description = key.Description, Amount = items.Sum(k => 1) }).ToList();
        }


        public Cart Payment(Guid userId)
        {
            Status status = _context.Status.Where(a => a.StatusName == "No Carrinho").SingleOrDefault();
            Cart cart = _context.Carts.Where(a => a.IdUser == userId).Where(x => x.IdStatus == status.Id).SingleOrDefault();
            return cart;
        }

        public void FinishTransaction(Guid userId, Cart cart)
        {
            Status status2 = _context.Status.Where(a => a.StatusName == "Concluida").SingleOrDefault();

            Cart cartUpdated = _context.Carts.Find(cart.Id);

            cartUpdated.Address = cart.Address;
            cartUpdated.IdPaymentMethod = cart.IdPaymentMethod;
            cartUpdated.IdStatus = status2.Id;
            _context.Update(cartUpdated);
            _context.SaveChanges();

        }
    }
}
