namespace ConsoleApp.Security
{
    public static class SapUserContext
    {
        // 🔹 Usuario actual que realiza las operaciones
        public static string CurrentUser { get; private set; } = "SYSTEM";

        // 🔹 Inicializa sesión con el usuario SAP
        public static void Login(string user)
        {
            if (string.IsNullOrWhiteSpace(user))
            {
                CurrentUser = "SYSTEM"; // fallback si no se ingresa usuario
            }
            else
            {
                CurrentUser = user.Trim().ToUpper(); // normalizar
            }
        }
    }
}
