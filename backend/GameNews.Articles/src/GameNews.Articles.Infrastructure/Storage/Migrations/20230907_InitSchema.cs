using FluentMigrator;

namespace GameNews.Articles.Infrastructure.Storage.Migrations;

[Migration(20230907, TransactionBehavior.None)]
public class InitSchema : Migration
{
    public override void Up()
    {
        Create.Table("articles")
            .WithColumn("article_id").AsInt64().PrimaryKey()
            .WithColumn("title").AsString()
            .WithColumn("preview_image").AsString()
            .WithColumn("publication_date").AsDateTime()
            .WithColumn("content").AsString();
    }

    public override void Down()
    {
        Delete.Table("articles");
    }
}