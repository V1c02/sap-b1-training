using ConsoleApp.Data;
using ConsoleApp.Models;
using Microsoft.Data.SqlClient;
using ConsoleApp.Security;

namespace ConsoleApp.Services;

public class AuditService
{
    public void Log(AuditLog log)
    {
        using var connection = DbConnection.GetConnection();
        connection.Open();
        connection.ChangeDatabase("SapTraining");

        // 🔹 Registrar auditoría con usuario SAP y fecha actual
        var command = new SqlCommand(
            @"INSERT INTO AuditLog 
                (Entity, Action, CardCode, OldValue, NewValue, CreatedBy, CreatedAt)
              VALUES 
                (@entity, @action, @code, @old, @new, @user, @createdAt)",
            connection);

        command.Parameters.AddWithValue("@entity", log.Entity);
        command.Parameters.AddWithValue("@action", log.Action);
        command.Parameters.AddWithValue("@code", log.CardCode);
        command.Parameters.AddWithValue("@old", (object?)log.OldValue ?? DBNull.Value);
        command.Parameters.AddWithValue("@new", (object?)log.NewValue ?? DBNull.Value);
        command.Parameters.AddWithValue("@user", SapUserContext.CurrentUser);
        command.Parameters.AddWithValue("@createdAt", DateTime.Now);

        command.ExecuteNonQuery();
    }

    public List<AuditLog> GetByCardCode(string cardCode)
    {
        var list = new List<AuditLog>();

        using var connection = DbConnection.GetConnection();
        connection.Open();
        connection.ChangeDatabase("SapTraining");

        var command = new SqlCommand(
            @"SELECT Id, Entity, Action, CardCode, OldValue, NewValue, CreatedBy, CreatedAt
              FROM AuditLog
              WHERE CardCode = @code
              ORDER BY CreatedAt DESC",
            connection);

        command.Parameters.AddWithValue("@code", cardCode);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new AuditLog
            {
                Id = reader.GetInt32(0),
                Entity = reader.GetString(1),
                Action = reader.GetString(2),
                CardCode = reader.GetString(3),
                OldValue = reader.IsDBNull(4) ? null : reader.GetString(4),
                NewValue = reader.IsDBNull(5) ? null : reader.GetString(5),
                CreatedBy = reader.IsDBNull(6) ? "SYSTEM" : reader.GetString(6),
                CreatedAt = reader.GetDateTime(7)
            });
        }

        return list;
    }
}
