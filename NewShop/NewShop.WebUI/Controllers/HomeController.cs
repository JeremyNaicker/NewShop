using NewShop.Core.Contracts;
using NewShop.Core.Models;
using NewShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewShop.WebUI.Controllers
{
    public class HomeController : Controller
    {

        //ProductRepository Context;
        IRepository<Product> Context;
        IRepository<ProductCategory> ProductCategories;
        //ProductCategoryRepository ProductCategories; 


        public HomeController(IRepository<Product> ProductContext, IRepository<ProductCategory> ProductCategoryContext)
        {
            Context = ProductContext;
            ProductCategories = ProductCategoryContext;
        }


        public ActionResult Index(string Category=null)
        {


            List<Product> products;
            List<ProductCategory> category = ProductCategories.Collection().ToList();
            //List<Product> products = Context.Collection().ToList();
            if (Category == null)
            {
                products = Context.Collection().ToList();
            }
            else
            {
                products = Context.Collection().Where(p => p.Category == Category).ToList();
            }

            ProductListViewModel model = new ProductListViewModel();
            model.product = products;
            model.ProductCategories = category;

            return View(model);
        }

        public ActionResult Details(string Id)
        {
            Product product = Context.Find(Id);
            if(product==null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}