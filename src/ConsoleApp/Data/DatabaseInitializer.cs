using Microsoft.Data.SqlClient;

namespace ConsoleApp.Data;

public static class DatabaseInitializer
{
    public static void Initialize()
    {
        using var connection = DbConnection.GetConnection();
        connection.Open();

        var createDb = new SqlCommand(@"
        IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SapTraining')
            CREATE DATABASE SapTraining;", connection);

        createDb.ExecuteNonQuery();

        connection.ChangeDatabase("SapTraining");

        var createTable = new SqlCommand(@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BusinessPartner')
        CREATE TABLE BusinessPartner (
            Id INT IDENTITY PRIMARY KEY,
            CardCode NVARCHAR(20) NOT NULL,
            CardName NVARCHAR(100) NOT NULL,
            TaxId NVARCHAR(20) NOT NULL
        );", connection);

        createTable.ExecuteNonQuery();
    }
}
