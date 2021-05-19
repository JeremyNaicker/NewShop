using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewShop.Core.Models;
using NewShop.DataAccess.InMemory;

namespace NewShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {


        ProductRepository Context;


        public ProductManagerController()
        {
            Context = new ProductRepository();
        }



        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = Context.Collection().ToList();
            return View(products);
        }



        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
         if (!ModelState.IsValid)
            {
                return View(product);
            }
         else
            {
                Context.Insert(product);
                Context.Commit();

                return RedirectToAction("Index");
            }
        }


        public ActionResult Edit(string Id)
        {
            Product product = Context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }


        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product ProducttoEdit = Context.Find(Id);
            if (ProducttoEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                ProducttoEdit.Category = product.Category;
                ProducttoEdit.Description = product.Description;
                ProducttoEdit.Image = product.Image;
                ProducttoEdit.Price = product.Price;
                ProducttoEdit.Name = product.Name;

                Context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Product ProducttoDelete = Context.Find(Id);
            if (ProducttoDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(ProducttoDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product ProducttoDelete = Context.Find(Id);
            if (ProducttoDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                Context.Delete(Id);
                Context.Commit();
                return RedirectToAction("Index");
            }

        }
    }
    }
