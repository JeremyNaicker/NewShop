using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewShop.Core.Contracts;
using NewShop.Core.Models;
using NewShop.Core.ViewModels;
using NewShop.DataAccess.InMemory;

namespace NewShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {


        //ProductRepository Context;
        IRepository<Product> Context;
        IRepository<ProductCategory> ProductCategories;
        //ProductCategoryRepository ProductCategories; 


        public ProductManagerController(IRepository<Product> ProductContext, IRepository<ProductCategory> ProductCategoryContext)
        {
            Context = ProductContext;
            ProductCategories = ProductCategoryContext;
        }



        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = Context.Collection().ToList();
            return View(products);
        }



        public ActionResult Create()
        {
            ProductManagerViewModel viewmodel = new ProductManagerViewModel();
            viewmodel.product = new Product();
            viewmodel.ProductCategories = ProductCategories.Collection();
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Create(Product product,HttpPostedFileBase file)
        {
         if (!ModelState.IsValid)
            {
                return View(product);
            }
         else
            {

                if(file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }


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
                ProductManagerViewModel ViewModel = new ProductManagerViewModel();
                ViewModel.product = product;
                ViewModel.ProductCategories = ProductCategories.Collection();
                return View(ViewModel);
            }
        }


        [HttpPost]
        public ActionResult Edit(Product product, string Id,HttpPostedFileBase file)
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

                if(file!=null)
                {
                    ProducttoEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + ProducttoEdit.Image);
                }

                ProducttoEdit.Category = product.Category;
                ProducttoEdit.Description = product.Description;
                //ProducttoEdit.Image = product.Image;
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
