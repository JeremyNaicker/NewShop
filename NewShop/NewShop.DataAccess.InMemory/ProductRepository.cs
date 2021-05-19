using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using NewShop.Core.Models;

namespace NewShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;


        public ProductRepository(){

            products = cache["products"] as List<Product>;


            if (products == null)
            {
                products = new List<Product>();
            }

            }


        public void Commit()
        {
            cache["products"] = products;
        }


        public void Insert(Product P)
        {
            products.Add(P);
        }

        public void Update (Product product)
        {
            Product Pupdate = products.Find(p => p.Id == product.Id);


            if (Pupdate != null)
            {
                Pupdate = product;
            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }


        public Product Find(string Id)
        {
            Product product = products.Find(p => p.Id == Id);


            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product Not Found");
            }


        }


        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }


        public void Delete (string Id)
        {
            Product PDelete = products.Find(p => p.Id == Id);


            if (PDelete != null)
            {
                products.Remove(PDelete);
            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }

    }
}
