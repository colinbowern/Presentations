using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using FluentMigrator;
using TodoMVC.Models;

namespace TodoMVC.Persistence.Profiles
{
    [Profile("Sample Data")]
    public class SampleDataProfile : Migration
    {
        public override void Up()
        {
            Delete.FromTable("Tasks").AllRows();

            BuilderSetup.DisablePropertyNamingFor<Task, int?>(x => x.Id);
            BuilderSetup.SetCreatePersistenceMethod<IList<Task>>(tasks =>
                tasks.ToList().ForEach(task =>
                    Insert.IntoTable("Tasks").Row(new
                    {
                        // NOTE: Add in columns with int conversion 
                        Name = task.Name,
                        //IsCompleted = task.IsCompleted,
                        Priority = (int)task.Priority,
                        Status = (int)task.Status,
                    })));

            Builder<Task>.CreateListOfSize(5)
                .Persist();
        }

        public override void Down()
        {
            // No Op 
        }
    }
}