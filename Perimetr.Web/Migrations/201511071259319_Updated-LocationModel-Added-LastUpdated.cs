namespace Perimetr.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedLocationModelAddedLastUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LocationModels", "LastUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LocationModels", "LastUpdated");
        }
    }
}
