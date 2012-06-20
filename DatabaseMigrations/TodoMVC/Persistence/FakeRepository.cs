using System.Collections.Generic;
using System.Linq;
using TodoMVC.Models;

namespace TodoMVC.Persistence
{
    public class FakeRepository : IRepository
    {
        private static readonly List<Task> tasks = new List<Task>();

        public Task Get(int id)
        {
            return tasks.First(x => x.Id == id);
        }

        public IList<Task> GetAll()
        {
            return tasks;
        }

        public Task Add(Task item)
        {
            item.Id = tasks.Any() ? tasks.Max(x => x.Id) + 1 : 1;
            tasks.Add(item);
            return item;
        }

        public void Update(Task item)
        {
            var index = tasks.FindIndex(x => x.Id == item.Id);
            tasks[index] = item;
        }

        public void Delete(Task item)
        {
            tasks.RemoveAll(x => x.Id == item.Id);
        }
    }
}