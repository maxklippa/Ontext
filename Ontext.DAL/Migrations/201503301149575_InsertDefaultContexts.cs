namespace Ontext.DAL.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertDefaultContexts : DbMigration
    {
        public override void Up()
        {
            Sql(string.Format("INSERT INTO [dbo].[Contexts] ([Id],[Name]) VALUES ('{0}','Family')", Guid.NewGuid()));
            Sql(string.Format("INSERT INTO [dbo].[Contexts] ([Id],[Name]) VALUES ('{0}','Work')", Guid.NewGuid()));
            Sql(string.Format("INSERT INTO [dbo].[Contexts] ([Id],[Name]) VALUES ('{0}','Game')", Guid.NewGuid()));
        }
        
        public override void Down()
        {
            Sql("DELETE FROM [dbo].[Contexts] WHERE [Name]='Game'");
            Sql("DELETE FROM [dbo].[Contexts] WHERE [Name]='Work'");
            Sql("DELETE FROM [dbo].[Contexts] WHERE [Name]='Family'");
        }
    }
}
