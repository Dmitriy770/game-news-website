using System.Transactions;
using FluentMigrator;

namespace GameNews.Articles.Infrastructure.Storage.Migrations;

[Migration(20230906, TransactionBehavior.None)]
public class Empty : Migration
{
    public override void Up()
    {
    }

    public override void Down()
    {
    }
}