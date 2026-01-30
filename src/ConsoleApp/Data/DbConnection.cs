using Microsoft.Data.SqlClient;

namespace ConsoleApp.Data;

public static class DbConnection
{
    private const string ConnectionString =
        "Server=127.0.0.1,1433;" +
        "Database=SapTraining;" +
        "User Id=sa;" +
        "Password=SqlServer#2025!;" +
        "TrustServerCertificate=True;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}
