using FluentMigrator;

namespace GameNews.Articles.Infrastructure.Storage.Migrations;

[Migration(20230908, TransactionBehavior.None)]
public class AddArticleType : Migration
{
    public override void Up()
    {
        const string sql = @"
        DO $$
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname='article') THEN
                    CREATE TYPE article as
                    (
                          article_id        bigint
                        , title             text
                        , preview_image     text
                        , publication_date  timestamp
                        , content           text
                    );
                END IF;
            END
        $$;";
        
        Execute.Sql(sql);
    }

    public override void Down()
    {
        const string sql = @"
        DO $$
            BEGIN
                DROP TYPE IF EXISTS article
            END
        $$;";
        
        Execute.Sql(sql);
    }
}