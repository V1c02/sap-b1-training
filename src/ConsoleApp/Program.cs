using ConsoleApp.Models;
using ConsoleApp.Services;
using ConsoleApp.Security;

Console.WriteLine("===== SAP Business One - Training =====");

// 🔹 Ingreso de usuario SAP
Console.Write("Usuario SAP: ");

var sapUser = Console.ReadLine();

if (string.IsNullOrWhiteSpace(sapUser))
{
    Console.WriteLine("❌ Usuario inválido.");
    return;
}

SapUserContext.Login(sapUser);
Console.WriteLine($"🔐 Sesión iniciada como: {SapUserContext.CurrentUser}");


var service = new BusinessPartnerService();
bool exit = false;

while (!exit)
{
    Console.WriteLine();
    Console.WriteLine("1. Crear Business Partner");
    Console.WriteLine("2. Listar Business Partners");
    Console.WriteLine("3. Salir");
    Console.WriteLine("4. Actualizar Business Partner");
    Console.WriteLine("5. Eliminar Business Partner");
    Console.WriteLine("6. Ver Auditoría por Business Partner");
    Console.Write("Seleccione una opción: ");

    string? option = Console.ReadLine() ?? string.Empty;

    switch (option)
    {
        case "1":
            CrearBusinessPartner(service);
            break;

        case "2":
            ListarBusinessPartners(service);
            break;

        case "3":
            exit = true;
            Console.WriteLine("Saliendo del sistema...");
            break;

        case "4":
            ActualizarBusinessPartner(service);
            break;

        case "5":
            EliminarBusinessPartner(service);
            break;

        case "6":
            Console.Write("Ingrese CardCode: ");
            string code = Console.ReadLine() ?? string.Empty;

            var auditService = new AuditService();
            var logs = auditService.GetByCardCode(code);

            if (logs.Count == 0)
            {
                Console.WriteLine("No hay auditoría para este Business Partner.");
                break;
            }

            Console.WriteLine("\nAUDITORÍA:\n");

            foreach (var log in logs)
            {
                Console.WriteLine($"[{log.CreatedAt}] {log.Action} por {log.CreatedBy}");
                Console.WriteLine($"  Antes : {log.OldValue ?? "-"}");
                Console.WriteLine($"  Después: {log.NewValue ?? "-"}");
                Console.WriteLine();
            }
            break;

        default:
            Console.WriteLine("Opción inválida.");
            break;
    }
}

static void CrearBusinessPartner(BusinessPartnerService service)
{
    Console.Write("CardCode: ");
    string code = Console.ReadLine() ?? string.Empty;

    Console.Write("CardName: ");
    string name = Console.ReadLine() ?? string.Empty;

    Console.Write("TaxId: ");
    string taxId = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrWhiteSpace(code) ||
        string.IsNullOrWhiteSpace(name) ||
        string.IsNullOrWhiteSpace(taxId))
    {
        Console.WriteLine("❌ Todos los campos son obligatorios.");
        return;
    }

    var bp = new BusinessPartner
    {
        CardCode = code,
        CardName = name,
        TaxId = taxId
    };

    bool inserted = service.Add(bp);

    Console.WriteLine(inserted
        ? "✅ Business Partner creado correctamente."
        : "⚠️ El Business Partner ya existe.");
}

static void ListarBusinessPartners(BusinessPartnerService service)
{
    var list = service.GetAll();

    Console.WriteLine("\nListado de Business Partners:\n");
    foreach (var bp in list)
    {
        Console.WriteLine($"ID: {bp.Id} | Code: {bp.CardCode} | Name: {bp.CardName} | TaxId: {bp.TaxId}");
    }
}

static void ActualizarBusinessPartner(BusinessPartnerService service)
{
    Console.Write("CardCode a actualizar: ");
    string code = Console.ReadLine() ?? string.Empty;

    if (!service.Exists(code))
    {
        Console.WriteLine("❌ Business Partner no existe.");
        return;
    }

    Console.Write("Nuevo CardName: ");
    string name = Console.ReadLine() ?? string.Empty;

    Console.Write("Nuevo TaxId: ");
    string taxId = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(taxId))
    {
        Console.WriteLine("❌ Ningún campo puede estar vacío.");
        return;
    }

    if (taxId.Length > 15)
    {
        Console.WriteLine("❌ TaxId no puede tener más de 15 caracteres.");
        return;
    }

    var bp = new BusinessPartner
    {
        CardCode = code,
        CardName = name,
        TaxId = taxId
    };

    bool updated = service.Update(bp);

    Console.WriteLine(updated
        ? "✅ Business Partner actualizado correctamente."
        : "⚠️ No se pudo actualizar.");
}

static void EliminarBusinessPartner(BusinessPartnerService service)
{
    Console.Write("CardCode a eliminar: ");
    string code = Console.ReadLine() ?? string.Empty;

    if (!service.Exists(code))
    {
        Console.WriteLine("❌ Business Partner no existe.");
        return;
    }

    Console.Write("¿Está seguro? (S/N): ");
    string confirm = Console.ReadLine() ?? string.Empty;

    if (confirm.ToUpper() != "S")
    {
        Console.WriteLine("❌ Operación cancelada.");
        return;
    }

    bool deleted = service.Delete(code);

    Console.WriteLine(deleted
        ? "🗑️ Business Partner eliminado correctamente."
        : "⚠️ No se pudo eliminar.");
}
