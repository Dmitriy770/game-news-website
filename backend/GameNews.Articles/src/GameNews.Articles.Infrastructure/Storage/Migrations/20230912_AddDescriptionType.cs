using FluentMigrator;

namespace GameNews.Articles.Infrastructure.Storage.Migrations;

[Migration(20230912, TransactionBehavior.None)]
public class AddDescriptionType : Migration{
    public override void Up()
    {
        const string sql = @"
        DO $$
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname='description') THEN
                    CREATE TYPE description as
                    (
                          article_id        bigint
                        , title             text
                        , preview_image     text
                        , publication_date  timestamp
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
                DROP TYPE IF EXISTS description;
            END
        $$;";
        
        Execute.Sql(sql);
    }
}