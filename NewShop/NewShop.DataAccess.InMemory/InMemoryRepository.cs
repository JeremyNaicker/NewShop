using NewShop.Core.Contracts;
using NewShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace NewShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T: BaseEntity

    {

        ObjectCache cache =  MemoryCache.Default;
        List<T> items;
        string ClassName;

        public InMemoryRepository()
        {
            ClassName = typeof(T).Name;
            items = cache[ClassName] as List<T>;
            if(items == null)
            {
                items = new List<T>();
            }
        }


        public void Commit()
        {
            cache[ClassName] = items;
        }

 

        public void Insert (T t)
        {
            items.Add(t);
        }

        public void Update (T t)
        {
            T ttoupdate = items.Find(i => i.Id == t.Id);

            if (ttoupdate != null)
            {
                ttoupdate = t;
            }
            else
            {
                throw new Exception(ClassName + " Not Found");
            }

        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(ClassName + " Not Found");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete (string Id)
        {
            T ttodelete = items.Find(i => i.Id == Id);

            if (ttodelete != null)
            {
                items.Remove(ttodelete);
            }
            else
            {
                throw new Exception(ClassName + " Not Found");
            }
        }

    }
}
