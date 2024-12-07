using SLC;
using Entities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NWindProxyService
{
    public class ProxyProducts : IProductService
    {
        string BaseAddress = "http://localhost:56104";

        // Método para enviar una solicitud POST
        public async Task<T> SendPost<T, PostData>(string requestURI, PostData data)
        {
            T Result = default(T);
            using (var Client = new HttpClient())
            {
                try
                {
                    // URL Absoluto
                    requestURI = BaseAddress + requestURI;
                    Client.DefaultRequestHeaders.Accept.Clear();
                    Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var JSONData = JsonConvert.SerializeObject(data);
                    HttpResponseMessage Response = await Client.PostAsync(requestURI,
                        new StringContent(JSONData, Encoding.UTF8, "application/json"));

                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<T>(ResultWebAPI);
                }
                catch (Exception ex)
                {
                    // Manejar la excepción adecuadamente
                }
            }
            return Result;
        }

        // Método para enviar una solicitud GET
        public async Task<T> SendGet<T>(string requestURI)
        {
            T Result = default(T);
            using (var Client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI; // URL Absoluto

                    Client.DefaultRequestHeaders.Accept.Clear();
                    Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ResultJSON = await Client.GetStringAsync(requestURI);
                    Result = JsonConvert.DeserializeObject<T>(ResultJSON);
                }
                catch (Exception ex)
                {
                    // Manejar la excepción adecuadamente
                }
            }
            return Result;
        }

        // Métodos asíncronos para interactuar con el servicio
        public async Task<Products> CreateProductAsync(Products newProduct)
        {
            return await SendPost<Products, Products>("/api/Products", newProduct); // Ruta actualizada
        }

        public Products CreateProduct(Products newProduct)
        {
            Products Result = null;
            Task.Run(async () => Result = await CreateProductAsync(newProduct)).Wait();
            return Result;
        }

        public async Task<Products> RetrieveProductByIDAsync(int ID)
        {
            return await SendGet<Products>($"/api/Products/{ID}"); // Ruta actualizada
        }

        public Products RetrieveProductByID(int ID)
        {
            Products Result = null;
            Task.Run(async () => Result = await RetrieveProductByIDAsync(ID)).Wait();
            return Result;
        }

        public async Task<bool> UpdateProductAsync(Products productToUpdate)
        {
            return await SendPost<bool, Products>($"/api/Products/{productToUpdate.ProductID}", productToUpdate); // Ruta actualizada
        }

        public bool UpdateProduct(Products productToUpdate)
        {
            bool Result = false;
            Task.Run(async () => Result = await UpdateProductAsync(productToUpdate)).Wait();
            return Result;
        }

        // Implementación del método Delete(int id)
        public async Task<bool> DeleteProductAsync(int id)
        {
            return await SendGet<bool>($"/api/Products/{id}"); // Ruta actualizada
        }

        public bool Delete(int id)
        {
            bool Result = false;
            Task.Run(async () => Result = await DeleteProductAsync(id)).Wait();
            return Result;
        }

        // Implementación del método GetById(int id)
        public async Task<Products> GetByIdAsync(int id)
        {
            return await SendGet<Products>($"/api/Products/{id}"); // Ruta actualizada
        }

        public Products GetById(int id)
        {
            Products Result = null;
            Task.Run(async () => Result = await GetByIdAsync(id)).Wait();
            return Result;
        }

        // Implementación del método GetAll() de la interfaz IProductService
        public async Task<List<Products>> GetAllAsync()
        {
            return await SendGet<List<Products>>("/api/Products"); // Ruta actualizada
        }

        public List<Products> GetAll()
        {
            List<Products> Result = null;
            Task.Run(async () => Result = await GetAllAsync()).Wait();
            return Result;
        }
    }
}
