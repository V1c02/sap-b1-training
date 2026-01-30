using ConsoleApp.Data;
using ConsoleApp.Models;
using ConsoleApp.Services;
using Microsoft.Data.SqlClient;
using ConsoleApp.Security;

namespace ConsoleApp.Services;

public class BusinessPartnerService
{
    public bool Add(BusinessPartner bp)
    {
        if (!ValidateLengths(bp))
            return false;

        if (Exists(bp.CardCode))
            return false;

        using var connection = DbConnection.GetConnection();
        connection.Open();
        connection.ChangeDatabase("SapTraining");

        var command = new SqlCommand(
            @"INSERT INTO BusinessPartner (CardCode, CardName, TaxId)
              VALUES (@code, @name, @tax)", connection);

        command.Parameters.AddWithValue("@code", bp.CardCode);
        command.Parameters.AddWithValue("@name", bp.CardName);
        command.Parameters.AddWithValue("@tax", bp.TaxId);

        int rows = command.ExecuteNonQuery();
        if (rows == 0)
            return false;

        // 🔹 Auditoría CREATE
        var audit = new AuditService();
        audit.Log(new AuditLog
        {
            Entity = "BusinessPartner",
            Action = "CREATE",
            CardCode = bp.CardCode,
            OldValue = null,
            NewValue = $"{bp.CardName} | {bp.TaxId}"
        });

        return true;
    }

    public bool Update(BusinessPartner bp)
    {
        if (!ValidateLengths(bp))
            return false;

        try
        {
            using var connection = DbConnection.GetConnection();
            connection.Open();
            connection.ChangeDatabase("SapTraining");

            // 1️⃣ Obtener valores actuales
            var selectCmd = new SqlCommand(
                "SELECT CardName, TaxId FROM BusinessPartner WHERE CardCode = @code",
                connection);
            selectCmd.Parameters.AddWithValue("@code", bp.CardCode);

            using var reader = selectCmd.ExecuteReader();
            if (!reader.Read())
                return false;

            string oldValue = $"{reader.GetString(0)} | {reader.GetString(1)}";
            reader.Close();

            // 2️⃣ Actualizar
            var command = new SqlCommand(
                @"UPDATE BusinessPartner
                  SET CardName = @name,
                      TaxId = @tax
                  WHERE CardCode = @code",
                connection);
            command.Parameters.AddWithValue("@name", bp.CardName);
            command.Parameters.AddWithValue("@tax", bp.TaxId);
            command.Parameters.AddWithValue("@code", bp.CardCode);

            int rows = command.ExecuteNonQuery();
            if (rows == 0)
                return false;

            // 🔹 Auditoría UPDATE
            var audit = new AuditService();
            audit.Log(new AuditLog
            {
                Entity = "BusinessPartner",
                Action = "UPDATE",
                CardCode = bp.CardCode,
                OldValue = oldValue,
                NewValue = $"{bp.CardName} | {bp.TaxId}"
            });

            return true;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"❌ Error de base de datos: {ex.Message}");
            return false;
        }
    }

    public bool Delete(string cardCode)
    {
        try
        {
            using var connection = DbConnection.GetConnection();
            connection.Open();
            connection.ChangeDatabase("SapTraining");

            // 1️⃣ Obtener datos antes de eliminar
            var selectCmd = new SqlCommand(
                "SELECT CardName, TaxId FROM BusinessPartner WHERE CardCode = @code",
                connection);
            selectCmd.Parameters.AddWithValue("@code", cardCode);

            using var reader = selectCmd.ExecuteReader();
            if (!reader.Read())
                return false;

            string oldValue = $"{reader.GetString(0)} | {reader.GetString(1)}";
            reader.Close();

            // 2️⃣ Eliminar
            var deleteCmd = new SqlCommand(
                "DELETE FROM BusinessPartner WHERE CardCode = @code",
                connection);
            deleteCmd.Parameters.AddWithValue("@code", cardCode);

            int rows = deleteCmd.ExecuteNonQuery();
            if (rows == 0)
                return false;

            // 🔹 Auditoría DELETE
            var audit = new AuditService();
            audit.Log(new AuditLog
            {
                Entity = "BusinessPartner",
                Action = "DELETE",
                CardCode = cardCode,
                OldValue = oldValue,
                NewValue = null
            });

            return true;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"❌ Error al eliminar: {ex.Message}");
            return false;
        }
    }

    public List<BusinessPartner> GetAll()
    {
        var list = new List<BusinessPartner>();

        using var connection = DbConnection.GetConnection();
        connection.Open();
        connection.ChangeDatabase("SapTraining");

        var command = new SqlCommand(
            "SELECT Id, CardCode, CardName, TaxId FROM BusinessPartner",
            connection);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new BusinessPartner
            {
                Id = reader.GetInt32(0),
                CardCode = reader.GetString(1),
                CardName = reader.GetString(2),
                TaxId = reader.GetString(3)
            });
        }

        return list;
    }

    public bool Exists(string cardCode)
    {
        using var connection = DbConnection.GetConnection();
        connection.Open();
        connection.ChangeDatabase("SapTraining");

        var command = new SqlCommand(
            "SELECT COUNT(*) FROM BusinessPartner WHERE CardCode = @code",
            connection);
        command.Parameters.AddWithValue("@code", cardCode);

        int count = (int)command.ExecuteScalar();
        return count > 0;
    }

    private bool ValidateLengths(BusinessPartner bp)
    {
        if (bp.CardCode.Length > 40)
        {
            Console.WriteLine("❌ CardCode supera el máximo permitido (40)");
            return false;
        }

        if (bp.CardName.Length > 200)
        {
            Console.WriteLine("❌ CardName supera el máximo permitido (200)");
            return false;
        }

        if (bp.TaxId.Length > 40)
        {
            Console.WriteLine("❌ TaxId supera el máximo permitido (40)");
            return false;
        }

        return true;
    }
}
