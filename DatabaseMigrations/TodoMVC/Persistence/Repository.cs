using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;
using TodoMVC.Models;

namespace TodoMVC.Persistence
{
    public class Repository : IRepository
    {
        private readonly ISession session = (ISession)HttpContext.Current.Items["Session"];

        public Task Get(int id)
        {
            return session.Get<Task>(id);
        }

        public IList<Task> GetAll()
        {
            return session.Query<Task>().ToList();
        }

        public Task Add(Task item)
        {
            return session.Save(item) as Task;
        }

        public void Update(Task item)
        {
            session.SaveOrUpdate(item);
        }

        public void Delete(Task item)
        {
            session.Delete(item);
        }
    }
}