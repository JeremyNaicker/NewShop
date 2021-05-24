using NewShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace NewShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {

        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;


        public ProductCategoryRepository()
        {

            productCategories = cache["productCategories"] as List<ProductCategory>;


            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }

        }


        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }


        public void Insert(ProductCategory P)
        {
            productCategories.Add(P);
        }

        public void Update(ProductCategory productcategory)
        {
            ProductCategory PCupdate = productCategories.Find(p => p.Id == productcategory.Id);


            if (PCupdate != null)
            {
                PCupdate = productcategory;
            }
            else
            {
                throw new Exception("Product Category Not Found");
            }
        }


        public ProductCategory Find(string Id)
        {
            ProductCategory productcategory = productCategories.Find(p => p.Id == Id);


            if (productcategory != null)
            {
                return productcategory;
            }
            else
            {
                throw new Exception("Product Category Not Found");
            }


        }


        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }


        public void Delete(string Id)
        {
            ProductCategory PCDelete = productCategories.Find(p => p.Id == Id);


            if (PCDelete != null)
            {
                productCategories.Remove(PCDelete);
            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }

    }
}
