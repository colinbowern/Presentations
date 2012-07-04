using System.Collections.Generic;
using TodoMVC.Models;

namespace TodoMVC.Persistence
{
    public interface IRepository
    {
        Task Get(int id);
        IList<Task> GetAll();
        Task Add(Task item);
        void Update(Task item);
        void Delete(Task item);
    }
}