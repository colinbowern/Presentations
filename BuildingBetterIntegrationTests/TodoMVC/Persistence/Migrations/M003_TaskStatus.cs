using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator;
using NLog;
using TodoMVC.Models;

namespace TodoMVC.Persistence.Migrations
{
    [Migration(3)]
    public class M003_TaskStatus : Migration
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public override void Up()
        {
            Alter.Table("Tasks").AddColumn("Status").AsInt32().NotNullable().WithDefaultValue((int)TaskStatus.NotStarted);
            Execute.Sql("UPDATE Tasks SET Status = 1 WHERE IsCompleted = 1");
            Delete.Column("IsCompleted").FromTable("Tasks");
        }

        public override void Down()
        {
            Alter.Table("Tasks").AddColumn("IsCompleted").AsBoolean().NotNullable().WithDefaultValue(false);

            log.Warn("Migrating down has caused a loss of data resolution on task status (Status >> IsCompleted)");
            Execute.Sql("UPDATE Tasks SET IsCompleted = 1 WHERE Status = 1");

            Delete.Column("Status").FromTable("Tasks");
        }
    }
}