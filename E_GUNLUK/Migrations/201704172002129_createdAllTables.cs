namespace E_GUNLUK.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdAllTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        commentId = c.Int(nullable: false, identity: true),
                        whichNote = c.Int(nullable: false),
                        theComment = c.String(),
                        commentDate = c.DateTime(nullable: false),
                        commentator_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.commentId)
                .ForeignKey("dbo.AspNetUsers", t => t.commentator_Id)
                .ForeignKey("dbo.Notes", t => t.whichNote, cascadeDelete: true)
                .Index(t => t.whichNote)
                .Index(t => t.commentator_Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        NoteId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        NoteText = c.String(),
                        NoteDate = c.DateTime(nullable: false),
                        PubOrPvt = c.Boolean(nullable: false),
                        NoteTaker_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.AspNetUsers", t => t.NoteTaker_Id)
                .Index(t => t.NoteTaker_Id);
            
            CreateTable(
                "dbo.Favorites",
                c => new
                    {
                        whichNote = c.Int(nullable: false),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.whichNote)
                .ForeignKey("dbo.Notes", t => t.whichNote)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.whichNote)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.FriendsLists",
                c => new
                    {
                        frindshipID = c.Int(nullable: false, identity: true),
                        friend_user_Id = c.String(maxLength: 128),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.frindshipID)
                .ForeignKey("dbo.AspNetUsers", t => t.friend_user_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.friend_user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        whichNote = c.Int(nullable: false),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.whichNote)
                .ForeignKey("dbo.Notes", t => t.whichNote)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.whichNote)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        tagId = c.Int(nullable: false, identity: true),
                        whichNote = c.Int(nullable: false),
                        tag = c.String(),
                    })
                .PrimaryKey(t => t.tagId)
                .ForeignKey("dbo.Notes", t => t.whichNote, cascadeDelete: true)
                .Index(t => t.whichNote);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "whichNote", "dbo.Notes");
            DropForeignKey("dbo.Likes", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "whichNote", "dbo.Notes");
            DropForeignKey("dbo.FriendsLists", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendsLists", "friend_user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Favorites", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Favorites", "whichNote", "dbo.Notes");
            DropForeignKey("dbo.Comments", "whichNote", "dbo.Notes");
            DropForeignKey("dbo.Notes", "NoteTaker_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "commentator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Tags", new[] { "whichNote" });
            DropIndex("dbo.Likes", new[] { "user_Id" });
            DropIndex("dbo.Likes", new[] { "whichNote" });
            DropIndex("dbo.FriendsLists", new[] { "user_Id" });
            DropIndex("dbo.FriendsLists", new[] { "friend_user_Id" });
            DropIndex("dbo.Favorites", new[] { "user_Id" });
            DropIndex("dbo.Favorites", new[] { "whichNote" });
            DropIndex("dbo.Notes", new[] { "NoteTaker_Id" });
            DropIndex("dbo.Comments", new[] { "commentator_Id" });
            DropIndex("dbo.Comments", new[] { "whichNote" });
            DropTable("dbo.Tags");
            DropTable("dbo.Likes");
            DropTable("dbo.FriendsLists");
            DropTable("dbo.Favorites");
            DropTable("dbo.Notes");
            DropTable("dbo.Comments");
        }
    }
}