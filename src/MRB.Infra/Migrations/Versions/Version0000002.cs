using FluentMigrator;

namespace MRB.Infra.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_RECIPES, "Create table to save the recipes and Ingredients information")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        CreateTable("Recipes")
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("CookingTime").AsInt32().Nullable()
            .WithColumn("Difficulty").AsInt32().Nullable()
            .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("FK_Recipe_User_Id", "Users", "Id");

        CreateTable("Ingredients")
            .WithColumn("Item").AsString().NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable().ForeignKey("FK_Ingredient_Recipe_Id", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);

        CreateTable("Instructions")
            .WithColumn("Step").AsString().NotNullable()
            .WithColumn("Text").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable().ForeignKey("FK_Intruction_Recipe_Id", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);

        CreateTable("DishType")
            .WithColumn("Type").AsString().NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable().ForeignKey("FK_DishType_Recipe_Id", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);
    }
}