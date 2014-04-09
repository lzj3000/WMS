using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Acctrue.Library.Data;
//using Lephone.Data;

namespace Acc.Manage.DesginManager
{
    public class BaseDao<T> where T : class
    {
        public virtual void Insert(T t)
        {
            DbEntry.Insert(t);
        }

        public virtual void Update(T t)
        {
            DbEntry.Update(t);
        }

        public virtual void Delete(T t)
        {
            DbEntry.Delete(t);
        }

        public virtual T GetObjectById(object key)
        {
            return DbEntry.GetObject<T>(key);
        }

        public virtual IList<T> GetAll()
        {
            return DbEntry.From<T>().Where(Condition.Empty).Select();
        }
    }
}