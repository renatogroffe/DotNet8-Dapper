using DustInTheWind.ConsoleTools.Controls.InputControls;
using Sharprompt;

namespace ConsoleAppDadosEmpresas.Inputs;

public static class InputHelper
{
    public static string GetAnswerCountry()
    {
        var answer = Prompt.Select("Selecione o pais",
            new[] { "Brasil", "Estados Unidos", "Italia", "Alemanha", "Franca" });
        Console.WriteLine();
        return answer;
    }

    public static string GetLocaleCode(string country)
    {
        switch (country)
        {
            case "Brasil":
                return "pt_BR";
            case "Estados Unidos":
                return "en_US";
            case "Italia":
                return "it";
            case "Alemanha":
                return "de";
            case "Franca":
                return "fr";
            default:
                throw new ArgumentException("Pais invalido!");
        }
    }

    public static int GetNumberOfRecords()
    {
        var answer = Prompt.Select<int>(options =>
        {
            options.Message = "Selecione a quantidade de registros a ser gerada";
            options.Items = new[] { 5, 10, 15 };
        });
        Console.WriteLine();
        return answer;
    }

    public static string GetConnectionString()
    {
        const string DefaultConnectionString =
            "Server=127.0.0.1,1433;Database=BaseEmpresas;User Id=sa;Password=SqlServer2019!;TrustServerCertificate=True;";
        var inputConnectionString = new StringValue(
            $"Connection String (pressione Enter para assumir default):");
        inputConnectionString.DefaultValue = DefaultConnectionString;
        inputConnectionString.AcceptDefaultValue = true;
        string connectionString;
        do
        {
            connectionString = inputConnectionString.Read();
        } while (String.IsNullOrWhiteSpace(connectionString));
        return connectionString;
    }
}