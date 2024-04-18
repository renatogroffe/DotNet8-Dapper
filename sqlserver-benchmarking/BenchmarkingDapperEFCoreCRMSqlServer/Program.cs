using BenchmarkDotNet.Running;
using BenchmarkingDapperEFCoreCRMSqlServer;
using BenchmarkingDapperEFCoreCRMSqlServer.Tests;
using DbUp;
using System.Reflection;

Console.WriteLine("Executando Migrations com DbUp...");

// Aguarda 7 segundos para se assegurar de que
// a instancia do SQL Server esteja ativa 
Thread.Sleep(7_000);

var upgrader = DeployChanges.To.SqlDatabase(Configurations.BaseMaster)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();
var result = upgrader.PerformUpgrade();

if (result.Successful)
{
    Console.WriteLine("Migrations do DbUp executadas com sucesso!");
    new BenchmarkSwitcher(new[] { typeof(CRMTests) }).Run(args);
}
else
{
    Environment.ExitCode = 3;
    Console.WriteLine($"Falha na execucao das Migrations do DbUp: {result.Error.Message}");
}

return Environment.ExitCode;