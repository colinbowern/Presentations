using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoMVC.Models
{
    /// <remarks>
    ///  Yes, this is a very anemic model. :(
    /// </remarks>
    public class Task
    {
        public virtual int? Id { get; set; }
        public virtual string Name { get; set; }

        //public virtual bool IsCompleted { get; set; }

        public virtual TaskPriority Priority { get; set; }
        public virtual TaskStatus Status { get; set; }
    }
}