using NewShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShop.Core.ViewModels
{
    public class ProductManagerViewModel
    {

        public Product product { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }

    }
}
