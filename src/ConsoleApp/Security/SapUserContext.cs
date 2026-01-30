namespace ConsoleApp.Security;

public static class SapUserContext
{
    // Usuario SAP actual (simulado)
    public static string CurrentUser { get; private set; } = "SAP_ADMIN";

    // Simula login SAP
    public static void Login(string userName)
    {
        CurrentUser = userName;
    }
}
