using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewShop.Core.Contracts;
using NewShop.Core.Models;
using NewShop.Core.ViewModels;
using NewShop.WebUI;
using NewShop.WebUI.Controllers;

namespace NewShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {

    [TestMethod]
    public void IndexPageDoesReturnProducts()
        {
            IRepository<Product> ProductContext = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> ProductCategoryContext = new Mocks.MockContext<ProductCategory>();
            HomeController controller = new HomeController(ProductContext, ProductCategoryContext);

            ProductContext.Insert(new Product());


            var result = controller.Index() as ViewResult;
            var viewmodel = (ProductListViewModel)result.ViewData.Model;
            Assert.AreEqual(1, viewmodel.product.Count());
        }


    }
}
