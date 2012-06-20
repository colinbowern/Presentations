using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace TodoMVC.Persistence.Migrations
{
    [Migration(3)]
    public class ThirdMigration : Migration
    {
        public override void Up()
        {
            // IsCompleted > Status (NotCompleted, InProress, Completed)
            // IsCompleted=True = Completed
            // IsCompleted=False = NotCompleted

            Alter.Table("Tasks").AddColumn("Status").AsInt32().NotNullable().WithDefaultValue(0);
            Execute.Sql("UPDATE Tasks SET Status = 1 WHERE IsCompleted = 1");
            Delete.Column("IsCompleted").FromTable("Tasks");
        }

        public override void Down()
        {
            Alter.Table("Tasks").AddColumn("IsCompleted").AsBoolean().NotNullable().WithDefaultValue(false);
            Execute.Sql("UPDATE Tasks SET IsCompleted = 1 WHERE Status = 1");
            Delete.Column("Status").FromTable("Tasks");
        }
    }
}