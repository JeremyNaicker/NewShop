using NewShop.Core.Contracts;
using NewShop.Core.Models;
using NewShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewShop.WebUI.Controllers
{
    public class ProductCategoryController : Controller
    {
        // GET: ProductCategory
        IRepository<ProductCategory> Context;


        public ProductCategoryController(IRepository<ProductCategory> ProductCategoryContext)
        {
            this.Context = ProductCategoryContext;
        }



        // GET: ProductManager
        public ActionResult Index()
        {
            List<ProductCategory> productcategories = Context.Collection().ToList();
            return View(productcategories);
        }



        public ActionResult Create()
        {
            ProductCategory productcategory = new ProductCategory();
            return View(productcategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productcategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productcategory);
            }
            else
            {
                Context.Insert(productcategory);
                Context.Commit();

                return RedirectToAction("Index");
            }
        }


        public ActionResult Edit(string Id)
        {
            ProductCategory productcategory = Context.Find(Id);
            if (productcategory == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productcategory);
            }
        }


        [HttpPost]
        public ActionResult Edit(ProductCategory productcategory, string Id)
        {
            ProductCategory ProductCategorytoEdit = Context.Find(Id);
            if (ProductCategorytoEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productcategory);
                }

                ProductCategorytoEdit.Category = productcategory.Category;
                //ProducttoEdit.Description = product.Description;
                //ProducttoEdit.Image = product.Image;
                //ProducttoEdit.Price = product.Price;
                //ProducttoEdit.Name = product.Name;

                Context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            ProductCategory ProductCategorytoDelete = Context.Find(Id);
            if (ProductCategorytoDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(ProductCategorytoDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory ProductCategorytoDelete = Context.Find(Id);
            if (ProductCategorytoDelete == null)
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