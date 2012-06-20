using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace TodoMVC.Persistence.Migrations
{
    [Migration(1)]
    public class M001_CreateTasksTable : Migration
    {
        public override void Up()
        {
            Create.Table("Tasks")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsCompleted").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Table("Tasks");
        } 
    }
}