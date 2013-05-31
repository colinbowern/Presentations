using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

using TasksApp.Models;

namespace TasksApp.Controllers.Api.Version1
{
    /// <summary>
    /// Tasks collection
    /// </summary>
    [Authorize(Users = "Colin")]
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
    }
}