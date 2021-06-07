using NewShop.Core.Contracts;
using NewShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShop.WebUI.Tests.Mocks
{
   public class MockContext<T> : IRepository<T> where T:BaseEntity
    {

        List<T> items;
        string ClassName;

        public MockContext()
        {

           items = new List<T>();
            
        }


        public void Commit()
        {
            return;
        }



        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
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

        public void Delete(string Id)
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
