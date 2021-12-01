using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using DAW_MP2.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace DAW_MP2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {

        //Hosted web API REST Service base url
        string Baseurl = "https://localhost:44339";
        string key = "hL4bA4nB4yI0vI0fC8fH7eT6";

        // GET: ProductsController
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {

                List<Product> ProductsInfo = new List<Product>();

                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("ApiKey", key);

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/products/");

                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var ProductResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list
                    ProductsInfo = JsonConvert.DeserializeObject<List<Product>>(ProductResponse);
                }

                return View(ProductsInfo);
            }
        }

        // GET: ProductsController/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DetailsAsync(Guid id)
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
                HttpResponseMessage Res = await client.GetAsync("api/products/" + id.ToString());

                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var ProductResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list
                    ProductInfo = JsonConvert.DeserializeObject<Product>(ProductResponse);
                }

                return View(ProductInfo);
            }
        }

        // GET: ProductsController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(Product product)
        {



            if (product.ImageFile == null)
            {
                product.Image = "/images/produto-sem-imagem.png";
            }
            else
            {
                string FileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string FileExtension = Path.GetExtension(product.ImageFile.FileName);
                FileName = product.Id.ToString() + FileExtension;
                var filePath = Path.Combine("wwwroot/images/uploaded/", FileName);
                product.Image = "/images/uploaded/" + FileName.ToString();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }
            }

            try
            {

                using (var client = new HttpClient())
                {

                    List<Product> ProductsInfo = new List<Product>();

                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("ApiKey", key);

                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    var postTask = await client.PostAsJsonAsync<Product>("api/products/", product);

                                        return RedirectToAction(nameof(Index));
                }
               
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Guid id)
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
                HttpResponseMessage Res = await client.GetAsync("api/products/" + id.ToString());

                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var ProductResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list
                    ProductInfo = JsonConvert.DeserializeObject<Product>(ProductResponse);
                }

                return View(ProductInfo);
            }
        }

        // POST: ProductsController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( Guid id, Product product)
        {
            

            if(product.ImageFile == null && (product.Image == null || product.Image == ""))
            {
                product.Image = "/images/produto-sem-imagem.png";
            }
            else if(product.ImageFile != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string FileExtension = Path.GetExtension(product.ImageFile.FileName);
                FileName = product.Id.ToString() + FileExtension;
                var filePath = Path.Combine("wwwroot/images/uploaded/", FileName);
                product.Image = "/images/uploaded/" + FileName.ToString();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }
            }

            try
            {
                using (var client = new HttpClient())
                {

                    List<Product> ProductsInfo = new List<Product>();

                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("ApiKey", key);

                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient

                    var postTask = await client.PutAsJsonAsync<Product>("api/products/" + id.ToString(), product);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View("Edit");
            }
        }

        // GET: ProductsController/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
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
                HttpResponseMessage Res = await client.GetAsync("api/products/" + id.ToString());

                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var ProductResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list
                    ProductInfo = JsonConvert.DeserializeObject<Product>(ProductResponse);
                }

                return View(ProductInfo);
            }
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id, Product product)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    List<Product> ProductsInfo = new List<Product>();

                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("ApiKey", key);

                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient

                    var postTask = await client.DeleteAsync("api/products/" + id.ToString());

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }
    }


}
