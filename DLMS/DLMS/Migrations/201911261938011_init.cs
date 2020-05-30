namespace DLMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        c_id = c.Int(nullable: false, identity: true),
                        d_id = c.Int(),
                        car_manufacturer = c.String(nullable: false),
                        car_model = c.String(nullable: false),
                        car_type = c.String(nullable: false),
                        price = c.Int(nullable: false),
                        power = c.Int(nullable: false),
                        fuel_tank = c.Int(nullable: false),
                        airbag = c.String(nullable: false),
                        gear = c.String(nullable: false),
                        mileage = c.Int(nullable: false),
                        img = c.String(),
                    })
                .PrimaryKey(t => t.c_id)
                .ForeignKey("dbo.Dealers", t => t.d_id)
                .Index(t => t.d_id);
            
            CreateTable(
                "dbo.Dealers",
                c => new
                    {
                        d_id = c.Int(nullable: false, identity: true),
                        company_name = c.String(nullable: false),
                        d_address = c.String(nullable: false),
                        email = c.String(nullable: false),
                        contact_no = c.String(nullable: false),
                        password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.d_id);
            
            CreateTable(
                "dbo.FullPayments",
                c => new
                    {
                        fp_id = c.Int(nullable: false, identity: true),
                        u_id = c.Int(),
                        d_id = c.Int(),
                        c_id = c.Int(),
                        downpayment = c.Int(nullable: false),
                        user_address = c.String(nullable: false),
                        mode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.fp_id)
                .ForeignKey("dbo.Cars", t => t.c_id)
                .ForeignKey("dbo.Dealers", t => t.d_id)
                .ForeignKey("dbo.Users", t => t.u_id)
                .Index(t => t.u_id)
                .Index(t => t.d_id)
                .Index(t => t.c_id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        u_id = c.Int(nullable: false, identity: true),
                        firstname = c.String(nullable: false),
                        lastname = c.String(nullable: false),
                        email = c.String(nullable: false),
                        mobile_no = c.String(nullable: false),
                        password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.u_id);
            
            CreateTable(
                "dbo.LoanApproveds",
                c => new
                    {
                        la_id = c.Int(nullable: false, identity: true),
                        u_id = c.Int(),
                        d_id = c.Int(),
                        c_id = c.Int(),
                        lender_id = c.Int(),
                        loan_id = c.Int(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.la_id)
                .ForeignKey("dbo.Cars", t => t.c_id)
                .ForeignKey("dbo.Dealers", t => t.d_id)
                .ForeignKey("dbo.Lenders", t => t.lender_id)
                .ForeignKey("dbo.LoanRequests", t => t.loan_id)
                .ForeignKey("dbo.Users", t => t.u_id)
                .Index(t => t.u_id)
                .Index(t => t.d_id)
                .Index(t => t.c_id)
                .Index(t => t.lender_id)
                .Index(t => t.loan_id);
            
            CreateTable(
                "dbo.Lenders",
                c => new
                    {
                        lender_id = c.Int(nullable: false, identity: true),
                        bankname = c.String(nullable: false),
                        branchname = c.String(nullable: false),
                        ifsc = c.String(nullable: false),
                        b_email = c.String(nullable: false),
                        contact_no = c.String(nullable: false),
                        password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.lender_id);
            
            CreateTable(
                "dbo.LoanRequests",
                c => new
                    {
                        loan_id = c.Int(nullable: false, identity: true),
                        lender_id = c.Int(),
                        downpayment = c.Int(nullable: false),
                        u_id = c.Int(),
                        c_id = c.Int(),
                        d_id = c.Int(),
                        loan_period = c.String(nullable: false),
                        emi = c.Int(nullable: false),
                        user_address = c.String(nullable: false),
                        id_proof = c.String(),
                        income_certificate = c.String(),
                        income_tax_return = c.String(),
                        address_proof = c.String(),
                        gnn = c.String(nullable: false),
                        gnn_address = c.String(nullable: false),
                        gnn_address_p = c.String(),
                    })
                .PrimaryKey(t => t.loan_id)
                .ForeignKey("dbo.Cars", t => t.c_id)
                .ForeignKey("dbo.Dealers", t => t.d_id)
                .ForeignKey("dbo.Lenders", t => t.lender_id)
                .ForeignKey("dbo.Users", t => t.u_id)
                .Index(t => t.lender_id)
                .Index(t => t.u_id)
                .Index(t => t.c_id)
                .Index(t => t.d_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoanApproveds", "u_id", "dbo.Users");
            DropForeignKey("dbo.LoanRequests", "u_id", "dbo.Users");
            DropForeignKey("dbo.LoanApproveds", "loan_id", "dbo.LoanRequests");
            DropForeignKey("dbo.LoanRequests", "lender_id", "dbo.Lenders");
            DropForeignKey("dbo.LoanRequests", "d_id", "dbo.Dealers");
            DropForeignKey("dbo.LoanRequests", "c_id", "dbo.Cars");
            DropForeignKey("dbo.LoanApproveds", "lender_id", "dbo.Lenders");
            DropForeignKey("dbo.LoanApproveds", "d_id", "dbo.Dealers");
            DropForeignKey("dbo.LoanApproveds", "c_id", "dbo.Cars");
            DropForeignKey("dbo.FullPayments", "u_id", "dbo.Users");
            DropForeignKey("dbo.FullPayments", "d_id", "dbo.Dealers");
            DropForeignKey("dbo.FullPayments", "c_id", "dbo.Cars");
            DropForeignKey("dbo.Cars", "d_id", "dbo.Dealers");
            DropIndex("dbo.LoanRequests", new[] { "d_id" });
            DropIndex("dbo.LoanRequests", new[] { "c_id" });
            DropIndex("dbo.LoanRequests", new[] { "u_id" });
            DropIndex("dbo.LoanRequests", new[] { "lender_id" });
            DropIndex("dbo.LoanApproveds", new[] { "loan_id" });
            DropIndex("dbo.LoanApproveds", new[] { "lender_id" });
            DropIndex("dbo.LoanApproveds", new[] { "c_id" });
            DropIndex("dbo.LoanApproveds", new[] { "d_id" });
            DropIndex("dbo.LoanApproveds", new[] { "u_id" });
            DropIndex("dbo.FullPayments", new[] { "c_id" });
            DropIndex("dbo.FullPayments", new[] { "d_id" });
            DropIndex("dbo.FullPayments", new[] { "u_id" });
            DropIndex("dbo.Cars", new[] { "d_id" });
            DropTable("dbo.LoanRequests");
            DropTable("dbo.Lenders");
            DropTable("dbo.LoanApproveds");
            DropTable("dbo.Users");
            DropTable("dbo.FullPayments");
            DropTable("dbo.Dealers");
            DropTable("dbo.Cars");
        }
    }
}
