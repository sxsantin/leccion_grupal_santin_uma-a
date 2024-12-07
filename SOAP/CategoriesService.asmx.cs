using System;
using System.Collections.Generic;
using System.Web.Services;
using BLL; // Importamos la capa BLL
using Entities; // Importamos la capa Entities

namespace SOAP
{
    /// <summary>
    /// Servicio Web para manejar operaciones sobre categorías utilizando la lógica de negocio
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CategoriesService : WebService
    {
        private readonly CategoriesLogic _categoriesLogic = new CategoriesLogic(); // Instancia de la lógica de negocio

        // Método para obtener todas las categorías
        [WebMethod]
        public List<CategoryDTO> GetCategories()
        {
            try
            {
                var categories = _categoriesLogic.RetrieveAll();
                var result = new List<CategoryDTO>();

                foreach (var category in categories)
                {
                    result.Add(new CategoryDTO
                    {
                        CategoryID = (int)category.GetType().GetProperty("CategoryID")?.GetValue(category),
                        CategoryName = category.GetType().GetProperty("CategoryName")?.GetValue(category)?.ToString(),
                        Description = category.GetType().GetProperty("Description")?.GetValue(category)?.ToString()
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las categorías: {ex.Message}");
            }
        }

        // Método para agregar una categoría
        [WebMethod]
        public string AddCategory(string categoryName, string description)
        {
            try
            {
                var category = new Categories
                {
                    CategoryName = categoryName,
                    Description = description
                };

                _categoriesLogic.Create(category);
                return "Categoría agregada exitosamente.";
            }
            catch (Exception ex)
            {
                return $"Error al agregar la categoría: {ex.Message}";
            }
        }

        // Método para actualizar una categoría
        [WebMethod]
        public string UpdateCategory(int categoryID, string categoryName, string description)
        {
            try
            {
                var category = new Categories
                {
                    CategoryID = categoryID,
                    CategoryName = categoryName,
                    Description = description
                };

                var result = _categoriesLogic.Update(category);
                return result ? "Categoría actualizada exitosamente." : "Categoría no encontrada.";
            }
            catch (Exception ex)
            {
                return $"Error al actualizar la categoría: {ex.Message}";
            }
        }

        // Método para eliminar una categoría
        [WebMethod]
        public string DeleteCategory(int categoryID)
        {
            try
            {
                var result = _categoriesLogic.Delete(categoryID);
                return result ? "Categoría eliminada exitosamente." : "Categoría no encontrada.";
            }
            catch (Exception ex)
            {
                return $"Error al eliminar la categoría: {ex.Message}";
            }
        }

        // Clase para representar una categoría como DTO
        public class CategoryDTO
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public string Description { get; set; }
        }
    }
}
