namespace NuGetGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFederatedCredentialPolicies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FederatedCredentialPolicies",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        UserKey = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastUpdated = c.DateTime(precision: 7, storeType: "datetime2"),
                        LastEvaluated = c.DateTime(precision: 7, storeType: "datetime2"),
                        Type = c.String(nullable: false, maxLength: 64),
                        Criteria = c.String(nullable: false),
                        OwnerKey = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Users", t => t.UserKey, cascadeDelete: true)
                .Index(t => t.UserKey);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FederatedCredentialPolicies", "UserKey", "dbo.Users");
            DropIndex("dbo.FederatedCredentialPolicies", new[] { "UserKey" });
            DropTable("dbo.FederatedCredentialPolicies");
        }
    }
}
