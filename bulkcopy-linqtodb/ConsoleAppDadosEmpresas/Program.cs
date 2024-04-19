using Bogus;
using DustInTheWind.ConsoleTools.Controls.Tables;
using LinqToDB;
using LinqToDB.Data;
using ConsoleAppDadosEmpresas.Data;
using ConsoleAppDadosEmpresas.Inputs;

Console.WriteLine(
    "***** Carga de Dados utilizando .NET 8 + LinqToDB + SQL Server + Bogus (dados fake) *****");
Console.WriteLine();

var country = InputHelper.GetAnswerCountry();
var localeCode = InputHelper.GetLocaleCode(country);
var numberOfRecords = InputHelper.GetNumberOfRecords();
var connectionString = InputHelper.GetConnectionString();

var db = new DataConnection(new DataOptions()
    .UseSqlServer(connectionString));

var fakeEmpresas = new Faker<Empresa>(localeCode).StrictMode(false)
            .RuleFor(p => p.Nome, f => f.Company.CompanyName())
            .RuleFor(p => p.Contato, f => f.Name.FullName())
            .RuleFor(p => p.Cidade, f => f.Address.City())
            .RuleFor(p => p.Pais, f => country)
            .Generate(numberOfRecords);

await db.BulkCopyAsync<Empresa>(fakeEmpresas);

Console.WriteLine();
var dataGrid = new DataGrid("Dados gerados em dbo.Empresas");
dataGrid.Columns.Add("* Empresa *");
dataGrid.Columns.Add("* Contato *");
dataGrid.Columns.Add("* Cidade *");
dataGrid.Columns.Add("* Pais *");
foreach (var empresa in fakeEmpresas)
    dataGrid.Rows.Add(empresa.Nome, empresa.Contato, empresa.Cidade, empresa.Pais);
dataGrid.DisplayBorderBetweenRows = true;
Console.WriteLine();
dataGrid.Display();

Console.WriteLine();       
