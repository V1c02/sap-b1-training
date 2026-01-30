namespace ConsoleApp.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string Entity { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string CardCode { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime CreatedAt { get; set; }

    // 🔹 Usuario que realizó la acción (nuevo)
    public string CreatedBy { get; set; } = "SYSTEM";
}
