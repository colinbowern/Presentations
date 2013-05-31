using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

using TasksApp.Models;

namespace TasksApp.Controllers.Api.Version2
{
    /// <summary>
    /// Tasks collection
    /// </summary>
    public class TasksController : ApiController
    {
        // GET /Tasks
        /// <summary>
        /// Retrieves all the tasks
        /// </summary>
        /// <returns>All the tasks</returns>
        public IEnumerable<Task> Get()
        {
            using (var dataContext = new DataContext())
            {
                var data = dataContext.Tasks.AsNoTracking();
                var model = data.Where(x => x.User.UserName == this.User.Identity.Name).ToList();
                return model;
            }
        }

        // GET /Tasks/5
        /// <summary>
        /// Gets task with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public Task Get(int id)
        {
            using (var dataContext = new DataContext())
            {
                var data = dataContext.Tasks.AsNoTracking();
                var model = data.FirstOrDefault(x => x.Id == id && x.User.UserName == this.User.Identity.Name);
                return model;
            }
        }

        // POST /Tasks
        /// <summary>
        /// Posts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Post(TaskModel model)
        {
            using (var dataContext = new DataContext())
            {
                var item = new Task
                {
                    Title = model.Title,
                    Completed = model.Completed,
                    User = dataContext.UserProfiles.AsNoTracking().First(x => x.UserName == this.User.Identity.Name)
                };
                dataContext.Tasks.Add(item);
                dataContext.SaveChanges();
            }
        }

        // PUT api/<controller>/5
        /// <summary>
        /// Puts the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="model">The model.</param>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        public void Put(int id, TaskModel model)
        {
            using (var dataContext = new DataContext())
            {
                var item = dataContext.Tasks.FirstOrDefault(x => x.Id == model.Id && x.User.UserName == this.User.Identity.Name);
                if (item == null) throw new HttpResponseException(HttpStatusCode.NotFound);

                item.Title = model.Title;
                item.Completed = model.Completed;
                dataContext.SaveChanges();
            }
        }

        // DELETE api/<controller>/5
        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(int id)
        {
            using (var dataContext = new DataContext())
            {
                var item = dataContext.Tasks.FirstOrDefault(x => x.Id == id && x.User.UserName == this.User.Identity.Name);
                if (item == null) return;
                dataContext.Tasks.Remove(item);
                dataContext.SaveChanges();
            }
        }
    }
}