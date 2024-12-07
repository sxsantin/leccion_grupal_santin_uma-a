using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BLL;
using Entities;

namespace YourNamespace.Controllers
{
    public class HomeController : Controller
    {
        private ProductsLogic _logic = new ProductsLogic();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int id)
        {
            try
            {
                var product = _logic.RetrieveById(id);
                if (product == null)
                {
                    ViewBag.ErrorMessage = "Producto no encontrado.";
                }
                return View("Details", product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public ActionResult List()
        {
            try
            {
                var products = _logic.RetrieveAll();
                return View("ProductList", products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("ProductList", new List<Products>());
            }
        }

        public ActionResult Details(int id)
        {
            var product = _logic.RetrieveById(id);
            if (product == null)
            {
                ViewBag.ErrorMessage = "Producto no encontrado.";
                return RedirectToAction("List");
            }
            return View(product);
        }

        public ActionResult CUDForm()
        {
            return View("CUD", new Products());
        }


        public ActionResult CUD(int? id = null)
        {
            Products model = id.HasValue ? _logic.RetrieveById(id.Value) : new Products();
            if (model == null && id.HasValue)
            {
                ViewBag.ErrorMessage = "Producto no encontrado.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CUD(Products model, string CreateBtn, string UpdateBtn, string DeleteBtn)
        {
            try
            {
                if (!string.IsNullOrEmpty(CreateBtn))
                {
                    _logic.Create(model);
                    ViewBag.SuccessMessage = "Producto creado exitosamente.";
                }
                else if (!string.IsNullOrEmpty(UpdateBtn))
                {
                    _logic.Update(model);
                    ViewBag.SuccessMessage = "Producto actualizado exitosamente.";
                }
                else if (!string.IsNullOrEmpty(DeleteBtn))
                {
                    if (_logic.Delete(model.ProductID))
                    {
                        ViewBag.SuccessMessage = "Producto eliminado exitosamente.";
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se puede eliminar el producto. Asegúrate de que las unidades en existencia sean 0.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(model);
        }
    }
}
