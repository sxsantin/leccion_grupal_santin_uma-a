using System;
using System.Collections.Generic;
using System.Web.Services;
using BLL; // Importamos la capa BLL
using Entities; // Importamos la capa Entities

namespace SOAP
{
    /// <summary>
    /// Servicio SOAP para la gestión de productos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ProductsService : WebService
    {
        private readonly ProductsLogic _productsLogic = new ProductsLogic(); // Creamos la instancia de la capa BLL

        // Método para agregar un producto
        [WebMethod]
        public string AgregarProducto(string productName, int categoryId, decimal unitPrice, int unitsInStock)
        {
            try
            {
                // Creamos el objeto product usando los parámetros recibidos
                Products product = new Products
                {
                    ProductName = productName,
                    CategoryID = categoryId,
                    UnitPrice = unitPrice,
                    UnitsInStock = unitsInStock
                };

                // Llamamos al método de la capa BLL para crear el producto
                var result = _productsLogic.Create(product);
                return "Producto agregado correctamente.";
            }
            catch (Exception ex)
            {
                return $"Error al agregar el producto: {ex.Message}";
            }
        }

        // Método para obtener todos los productos
        [WebMethod]
        public List<Product> ObtenerProductos()
        {
            try
            {
                // Llamamos al método de la capa BLL para obtener los productos
                var productsList = _productsLogic.RetrieveAll();

                // Convertimos los objetos de la clase Products a Product para la respuesta SOAP
                var result = new List<Product>();
                foreach (var item in productsList)
                {
                    // Creamos un objeto Product con la información
                    var product = new Product
                    {
                        ProductID = item.ProductID,
                        ProductName = item.ProductName,
                        CategoryID = item.CategoryID,
                        UnitPrice = item.UnitPrice,
                        UnitsInStock = item.UnitsInStock
                    };
                    result.Add(product);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los productos: {ex.Message}");
            }
        }

        // Método para actualizar un producto
        [WebMethod]
        public string ActualizarProducto(int productId, string productName, int categoryId, decimal unitPrice, int unitsInStock)
        {
            try
            {
                // Creamos el objeto product usando los parámetros recibidos
                Products product = new Products
                {
                    ProductID = productId,
                    ProductName = productName,
                    CategoryID = categoryId,
                    UnitPrice = unitPrice,
                    UnitsInStock = unitsInStock
                };

                // Llamamos al método de la capa BLL para actualizar el producto
                var result = _productsLogic.Update(product);
                return result ? "Producto actualizado correctamente." : "Producto no encontrado.";
            }
            catch (Exception ex)
            {
                return $"Error al actualizar el producto: {ex.Message}";
            }
        }

        // Método para eliminar un producto
        [WebMethod]
        public string EliminarProducto(int productId)
        {
            try
            {
                // Llamamos al método de la capa BLL para eliminar el producto
                var result = _productsLogic.Delete(productId);
                return result ? "Producto eliminado correctamente." : "Producto no encontrado.";
            }
            catch (Exception ex)
            {
                return $"Error al eliminar el producto: {ex.Message}";
            }
        }

        // Clase interna para representar un producto en el servicio SOAP
        public class Product
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int CategoryID { get; set; }
            public decimal UnitPrice { get; set; }
            public int UnitsInStock { get; set; }
        }
    }
}
