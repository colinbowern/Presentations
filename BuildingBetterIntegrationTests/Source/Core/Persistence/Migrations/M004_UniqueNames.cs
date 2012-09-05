using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator;

namespace Bits.Persistence.Migrations
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
            //Delete.Index("UX_Name").OnTable("Tasks");
            Delete.UniqueConstraint("UX_Name").FromTable("Tasks");
        }
    }
}