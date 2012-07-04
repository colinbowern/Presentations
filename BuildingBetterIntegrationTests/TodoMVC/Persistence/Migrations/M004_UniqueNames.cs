using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator;

namespace TodoMVC.Persistence.Migrations
{
    [Migration(4)]
    public class M004_UniqueNames : Migration
    {
        public override void Up()
        {
            Alter.Table("Tasks").AlterColumn("Name").AsString().Unique("UX_Name");
        }

        public override void Down()
        {
            Delete.UniqueConstraint("UX_Name").FromTable("Tasks");
        }
    }
}