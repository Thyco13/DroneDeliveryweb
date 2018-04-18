namespace DroneDeliveryweb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedOrderHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderHistories",
                c => new
                    {
                        Orderid = c.Int(nullable: false, identity: true),
                        ToCoordinates_FromAddress = c.String(nullable: false),
                        ToCoordinates_FromCity = c.String(nullable: false),
                        ToCoordinates_FromZip = c.Int(nullable: false),
                        ToCoordinates_ToStreet = c.String(nullable: false),
                        ToCoordinates_ToCity = c.String(nullable: false),
                        ToCoordinates_ToZip = c.Int(nullable: false),
                        ToCoordinates_Fromlat = c.String(),
                        ToCoordinates_Fromlng = c.String(),
                        ToCoordinates_ToLat = c.String(),
                        ToCoordinates_ToLng = c.String(),
                        ToCoordinates_Distance = c.String(),
                        ToCoordinates_Weather = c.String(),
                        ToCoordinates_Weight = c.String(),
                        ToCoordinates_Price = c.String(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Orderid)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderHistories", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.OrderHistories", new[] { "Customer_Id" });
            DropTable("dbo.OrderHistories");
        }
    }
}
