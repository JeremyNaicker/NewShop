using NewShop.Core.Contracts;
using NewShop.Core.Models;
using NewShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NewShop.Services
{
   public class BasketService : IBasketService
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

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            if (basket!= null)
            {
                var results = (from b in basket.BasketItems
                              join p in productContext.Collection() on b.ProductId equals p.Id
                              select new BasketItemViewModel
                              {
                                  Id = b.Id,
                                  Quantity = b.Qunatity,
                                  ProductName = p.Name,
                                  Price = p.Price,
                                  Image = p.Image

                              }).ToList();

                return results;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);
            if (basket != null)
            {
                int? basketCount = (from items in basket.BasketItems
                                    select items.Qunatity).Sum();

                decimal? basketTotal = (from items in basket.BasketItems
                                        join p in productContext.Collection() on items.ProductId equals p.Id
                                    select items.Qunatity * p.Price).Sum();

                model.BasketCount = basketCount ?? 0;
                model.BasketTotal = basketTotal ?? decimal.Zero;
            }
            else
            {
                return model;
            }

            return model;
        }

    }
}
