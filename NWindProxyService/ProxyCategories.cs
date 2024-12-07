using SLC;  // Asegúrate de que este namespace se refiera al proyecto adecuado.
using Entities;  // Aquí debes tener las clases como Categories, que están definidas en el proyecto Entities.
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NWindProxyService
{
    public class ProxyCategories : ICategoriesService
    {
        string BaseAddress = "http://localhost:56104";  // Asegúrate de que la URL es la correcta para tu API de categorías.

        // Método para enviar una solicitud POST (crear categoría)
        public async Task<T> SendPost<T, PostData>(string requestURI, PostData data)
        {
            T Result = default(T);
            using (var Client = new HttpClient())
            {
                try
                {
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
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return Result;
        }

        // Método para enviar una solicitud GET (obtener categorías)
        public async Task<T> SendGet<T>(string requestURI)
        {
            T Result = default(T);
            using (var Client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI;

                    Client.DefaultRequestHeaders.Accept.Clear();
                    Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var ResultJSON = await Client.GetStringAsync(requestURI);
                    Result = JsonConvert.DeserializeObject<T>(ResultJSON);
                }
                catch (Exception ex)
                {
                    // Manejar la excepción adecuadamente
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return Result;
        }

        // Crear una categoría
        public async Task<Categories> CreateCategoryAsync(Categories newCategory)
        {
            return await SendPost<Categories, Categories>("/api/Categories", newCategory);
        }

        public Categories CreateCategory(Categories newCategory)
        {
            Categories Result = null;
            Task.Run(async () => Result = await CreateCategoryAsync(newCategory)).Wait();
            return Result;
        }

        // Obtener categoría por ID
        public async Task<Categories> RetrieveCategoryByIDAsync(int ID)
        {
            return await SendGet<Categories>($"/api/Categories/{ID}");
        }

        public Categories RetrieveCategoryByID(int ID)
        {
            Categories Result = null;
            Task.Run(async () => Result = await RetrieveCategoryByIDAsync(ID)).Wait();
            return Result;
        }

        // Obtener todas las categorías
        public async Task<List<Categories>> GetAllCategoriesAsync()
        {
            return await SendGet<List<Categories>>("/api/Categories");
        }

        public List<Categories> GetAllCategories()
        {
            List<Categories> Result = null;
            Task.Run(async () => Result = await GetAllCategoriesAsync()).Wait();
            return Result;
        }

        // Actualizar categoría
        public async Task<bool> UpdateCategoryAsync(Categories categoryToUpdate)
        {
            return await SendPost<bool, Categories>($"/api/Categories/{categoryToUpdate.CategoryID}", categoryToUpdate);
        }

        public bool UpdateCategory(Categories categoryToUpdate)
        {
            bool Result = false;
            Task.Run(async () => Result = await UpdateCategoryAsync(categoryToUpdate)).Wait();
            return Result;
        }

        // Eliminar categoría
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await SendGet<bool>($"/api/Categories/{id}");
        }

        // Implementación del método Delete de la interfaz ICategoriesService
        public bool Delete(int id)
        {
            bool Result = false;
            Task.Run(async () => Result = await DeleteCategoryAsync(id)).Wait();
            return Result;
        }
    }
}
