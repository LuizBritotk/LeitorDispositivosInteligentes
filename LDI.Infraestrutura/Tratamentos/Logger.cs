namespace LDI.Infraestrutura.Tratamentos;

public static class Logger
{
    public static void Info(string message) => Console.WriteLine($"[INFO] {message}");
    public static void Success(string message) => Console.WriteLine($"[SUCESSO] {message}");
    public static void Error(string message) => Console.WriteLine($"[ERRO] {message}");
}
