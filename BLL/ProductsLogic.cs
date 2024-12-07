using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;

namespace BLL
{
    public class ProductsLogic
    {
        public Products Create(Products products)
        {
            Products _products = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                Products _result = repository.Retrieve<Products>(p => p.ProductName == products.ProductName);

                if (_result == null)
                {
                    _products = repository.Create(products);
                }
                else
                {
                    throw new Exception("Producto ya existe");
                }
            }

            return _products; // Cambiado para retornar el producto creado.
        }


        public Products RetrieveById(int id){
            Products _products = null;
            using (var repository = RepositoryFactory.CreateRepository()){
                _products = repository.Retrieve<Products>(p => p.ProductID == id);
            }
            return _products;
        }
        public bool Update(Products products)
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Busca la entidad original en la base de datos
                var existingProduct = repository.Retrieve<Products>(p => p.ProductID == products.ProductID);

                if (existingProduct == null)
                {
                    throw new Exception("El producto no existe.");
                }

                // Actualiza manualmente las propiedades necesarias
                existingProduct.ProductName = products.ProductName;
                existingProduct.UnitPrice = products.UnitPrice;
                existingProduct.UnitsInStock = products.UnitsInStock;
                existingProduct.CategoryID = products.CategoryID;

                // Guarda los cambios
                return repository.Update(existingProduct);
            }
        }


        public bool Delete(int id)
        {
            bool _delete = false;
            var _product = RetrieveById(id);
            if (_product != null)
            {
                if (_product.UnitsInStock == 0)
                {
                    using (var repository = RepositoryFactory.CreateRepository())
                    {
                        _delete = repository.Delete(_product);
                    }
                }
            }
            return _delete;
        }
        public List<Products> RetrieveAll()
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Usar una expresión lambda
                var products = repository.Filter<Products>(p => p.ProductID > 0).ToList();

                return products; // Devuelve la lista de productos directamente
            }
        }
    }
}
