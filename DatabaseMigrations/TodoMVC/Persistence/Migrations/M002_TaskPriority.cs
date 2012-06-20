using FluentMigrator;
using TodoMVC.Models;

namespace TodoMVC.Persistence.Migrations
{
    [Migration(2)]
    public class M002_TaskPriority : Migration
    {
        public override void Up()
        {
            Alter.Table("Tasks").AddColumn("Priority").AsInt32().NotNullable().WithDefaultValue((int)TaskPriority.Normal);
        }
        public override void Down()
        {
            Delete.Column("Priority").FromTable("Tasks");
        }  
    }
}