using NewShop.Core.Contracts;
using NewShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NewShop.Services
{
   public class BasketService
    {

        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }

        private Basket GetBasket(HttpContextBase httpContext,bool createifNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketid = cookie.Value;
                if(!string.IsNullOrEmpty(basketid))
                {
                    basket = basketContext.Find(basketid);
                }
                else
                {
                    if(createifNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createifNull)
                {
                    basket = CreateNewBasket(httpContext);
                }

            }

            return basket;

        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddtoBasket(HttpContextBase httpContext,string ProductId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem Item = basket.BasketItems.FirstOrDefault(i => i.ProductId == ProductId);

            if (Item == null)
            {
                Item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = ProductId,
                    Qunatity = 1
                };

                basket.BasketItems.Add(Item);
            }
            else
            {
                Item.Qunatity = Item.Qunatity + 1;
            }

            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string ItemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem Item = basket.BasketItems.FirstOrDefault(i => i.Id == ItemId);
            if (Item != null)
            {
                basket.BasketItems.Remove(Item);
                basketContext.Commit();
            }

        }

    }
}
