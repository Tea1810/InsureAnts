# Prerequisites

Make sure to have the EF Core CLI tools installed using the following command:

`dotnet tool install --global dotnet-ef`

# Add a migration

To add a new migrations run one of the following commands depending on the context:

AzureSqlServer: 
`dotnet ef migrations add CommitName --project .\InsureAnts\InsureAnts.Infrastructure --startup-project .\InsureAnts\InsureAnts.Web -o Migrations/SqlServerMigrations -c InsureAntsDbContext`

# Apply all migrations

To apply all migrations run the following command (depending on the context):

AzureSqlServer: 
`dotnet ef database update --project .\InsureAnts.Infrastructure --startup-project .\InsureAnts.Web --context InsureAntsDbContext`
