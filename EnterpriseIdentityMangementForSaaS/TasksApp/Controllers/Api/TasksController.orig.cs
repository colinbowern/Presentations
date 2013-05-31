using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TasksApp.Models;

namespace TasksApp.Controllers.Api
{
    ///// <summary>
    ///// Tasks resources
    ///// </summary>
    //public class TasksController : ApiController
    //{
    //    /// <summary>
    //    /// Gets all tasks.
    //    /// </summary>
    //    /// <returns></returns>
    //    public IEnumerable<Task> Get()
    //    {
    //        using (var dataContext = new DataContext())
    //        {
    //            var data = dataContext.Tasks.AsNoTracking();
    //            var model = data.Where(x => x.User.UserName == this.User.Identity.Name);
    //            return model.ToList();
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the task by id.
    //    /// </summary>
    //    /// <param name="id">The id.</param>
    //    /// <returns></returns>
    //    /// <exception cref="System.Web.Http.HttpResponseException"></exception>
    //    public Task Get(int id)
    //    {
    //        using (var dataContext = new DataContext())
    //        {
    //            var item = dataContext.Tasks.AsNoTracking().FirstOrDefault(x => x.Id == id && x.User.UserName == this.User.Identity.Name);
    //            if (item == null) throw new HttpResponseException(HttpStatusCode.NotFound);
    //            return item;
    //        }
    //    }

    //    /// <summary>
    //    /// Creates a new task.
    //    /// </summary>
    //    /// <param name="model">The model.</param>
    //    /// <returns></returns>
    //    public HttpResponseMessage Post(TaskModel model)
    //    {
    //        using (var dataContext = new DataContext())
    //        {
    //            var item = new Task
    //            {
    //                Title = model.Title,
    //                Completed = model.Completed,
    //                User = dataContext.UserProfiles.AsNoTracking().First(x => x.UserName == this.User.Identity.Name)
    //            };
    //            dataContext.Tasks.Add(item);
    //            dataContext.SaveChanges();
    //            var response = this.Request.CreateResponse(HttpStatusCode.Created, item);
    //            response.Headers.Location = new Uri(this.Url.Link("DefaultApi", new { id = item.Id }));
    //            return response;
    //        }
    //    }

    //    /// <summary>
    //    /// Deletes the task.
    //    /// </summary>
    //    /// <param name="id">The id.</param>
    //    public void Delete(int id)
    //    {
    //        using (var dataContext = new DataContext())
    //        {
    //            var item = dataContext.Tasks.FirstOrDefault(x => x.Id == id && x.User.UserName == this.User.Identity.Name);
    //            if (item == null) return;
    //            dataContext.Tasks.Remove(item);
    //            dataContext.SaveChanges();
    //        }
    //    }

    //    /// <summary>
    //    /// Updates the existing task.
    //    /// </summary>
    //    /// <param name="id">The id.</param>
    //    /// <param name="model">The model.</param>
    //    public void Put(int id, TaskModel model)
    //    {
    //        using (var dataContext = new DataContext())
    //        {
    //            var item = dataContext.Tasks.FirstOrDefault(x => x.Id == model.Id && x.User.UserName == this.User.Identity.Name);
    //            if (item == null) throw new HttpResponseException(HttpStatusCode.NotFound);

    //            item.Title = model.Title;
    //            item.Completed = model.Completed;
    //            dataContext.SaveChanges();
    //        }
    //    }
    //}
}