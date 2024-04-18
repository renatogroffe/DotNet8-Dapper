using BenchmarkDotNet.Attributes;
using BenchmarkingDapperEFCoreCRMSqlServer;
using BenchmarkingDapperEFCoreCRMSqlServer.EFCore;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BenchmarkingDapperEFCoreCRMSqlServer.Tests;

[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput, launchCount: 5)]
public class CRMTests
{
    private const int NumeroContatosPorCompanhia = 1;

    private int GetNumeroContatosPorCompanhia()
    {
        var envNumeroContatosPorCompanhia =
            Environment.GetEnvironmentVariable("NumeroContatosPorCompanhia");
        if (!string.IsNullOrWhiteSpace(envNumeroContatosPorCompanhia) &&
            int.TryParse(envNumeroContatosPorCompanhia, out int result))
            return result;
        return NumeroContatosPorCompanhia;
    }

    #region EFCore Tests

    private CRMContext? _context;
    private Name? _namesDataSetEF;
    private PhoneNumbers? _phonesDataSetEF;
    private Address? _addressesDataSetEF;
    private Company? _companiesDataSetEF;
    private int _numeroContatosPorCompanhiaEF;

    [IterationSetup(Target = nameof(InputDataWithEntityFrameworkCore))]
    public void SetupEntityFrameworkCore()
    {
        _context = new CRMContext();
        _namesDataSetEF = new Name("pt_BR");
        _phonesDataSetEF = new PhoneNumbers("pt_BR");
        _addressesDataSetEF = new Address("pt_BR");
        _companiesDataSetEF = new Company("pt_BR");
        _numeroContatosPorCompanhiaEF = GetNumeroContatosPorCompanhia();
    }

    [Benchmark]
    public EFCore.Empresa InputDataWithEntityFrameworkCore()
    {
        var empresa = new EFCore.Empresa()
        {
            Nome = _companiesDataSetEF!.CompanyName(),
            CNPJ = _companiesDataSetEF!.Cnpj(includeFormatSymbols: false),
            Cidade = _addressesDataSetEF!.City(),
            Contatos = new()
        };
        for (int i = 0; i < _numeroContatosPorCompanhiaEF; i++)
        {
            empresa.Contatos.Add(new()
            {
                Nome = _namesDataSetEF!.FullName(),
                Telefone = _phonesDataSetEF!.PhoneNumber()
            });
        }

        _context!.Add(empresa);
        _context!.SaveChanges();

        return empresa;
    }

    [IterationCleanup(Target = nameof(InputDataWithEntityFrameworkCore))]
    public void CleanupEntityFrameworkCore()
    {
        _context = null;
    }

    #endregion

    #region Dapper Tests

    private SqlConnection? _connectionDapper;
    private Name? _namesDataSetDapper;
    private PhoneNumbers? _phonesDataSetDapper;
    private Address? _addressesDataSetDapper;
    private Company? _companiesDataSetDapper;
    private int _numeroContatosPorCompanhiaDapper;

    [IterationSetup(Target = nameof(InputDataWithDapper))]
    public void SetupDapper()
    {
        _connectionDapper = new SqlConnection(Configurations.BaseDapper);
        _namesDataSetDapper = new Name("pt_BR");
        _phonesDataSetDapper = new PhoneNumbers("pt_BR");
        _addressesDataSetDapper = new Address("pt_BR");
        _companiesDataSetDapper = new Company("pt_BR");
        _numeroContatosPorCompanhiaDapper = GetNumeroContatosPorCompanhia();
    }

    [Benchmark]
    public Dapper.Empresa InputDataWithDapper()
    {
        var empresa = new Dapper.Empresa()
        {
            Nome = _companiesDataSetDapper!.CompanyName(),
            CNPJ = _companiesDataSetDapper!.Cnpj(includeFormatSymbols: false),
            Cidade = _addressesDataSetDapper!.City()
        };

        _connectionDapper!.Open();
        var transaction = _connectionDapper.BeginTransaction();

        empresa.IdEmpresa = _connectionDapper.QuerySingle<int>(
            "INSERT INTO dbo.Empresas (CNPJ, Nome, Cidade) " +
            "VALUES (@CNPJ, @Nome, @Cidade);" + Environment.NewLine +
            "SELECT CAST(SCOPE_IDENTITY() as int)", empresa, transaction);

        empresa.Contatos = new();
        for (int i = 0; i < _numeroContatosPorCompanhiaDapper; i++)
        {
            var contato = new Dapper.Contato()
            {
                IdEmpresa = empresa.IdEmpresa,
                Nome = _namesDataSetDapper!.FullName(),
                Telefone = _phonesDataSetDapper!.PhoneNumber()
            };
            contato.IdContato = _connectionDapper.QuerySingle<int>(
                "INSERT INTO dbo.Contatos (Nome, Telefone, IdEmpresa) " +
                "VALUES (@Nome, @Telefone, @IdEmpresa);" + Environment.NewLine +
                "SELECT CAST(SCOPE_IDENTITY() as int)", contato, transaction);
            empresa.Contatos.Add(contato);
        }

        transaction.Commit();
        _connectionDapper.Close();

        return empresa;
    }

    [IterationCleanup(Target = nameof(InputDataWithDapper))]
    public void CleanupDapper()
    {
        _connectionDapper = null;
    }

    #endregion

    #region Dapper.Contrib Tests

    private SqlConnection? _connectionDapperContrib;
    private Name? _namesDataSetDapperContrib;
    private PhoneNumbers? _phonesDataSetDapperContrib;
    private Address? _addressesDataSetDapperContrib;
    private Company? _companiesDataSetDapperContrib;
    private int _numeroContatosPorCompanhiaDapperContrib;

    [IterationSetup(Target = nameof(InputDataWithDapperContrib))]
    public void SetupDapperContrib()
    {
        _connectionDapperContrib = new SqlConnection(Configurations.BaseDapperContrib);
        _namesDataSetDapperContrib = new Name("pt_BR");
        _phonesDataSetDapperContrib = new PhoneNumbers("pt_BR");
        _addressesDataSetDapperContrib = new Address("pt_BR");
        _companiesDataSetDapperContrib = new Company("pt_BR");
        _numeroContatosPorCompanhiaDapperContrib = GetNumeroContatosPorCompanhia();
    }

    [Benchmark]
    public Dapper.Empresa InputDataWithDapperContrib()
    {
        var empresa = new Dapper.Empresa()
        {
            Nome = _companiesDataSetDapperContrib!.CompanyName(),
            CNPJ = _companiesDataSetDapperContrib!.Cnpj(includeFormatSymbols: false),
            Cidade = _addressesDataSetDapperContrib!.City()
        };

        _connectionDapperContrib!.Open();
        var transaction = _connectionDapperContrib.BeginTransaction();

        _connectionDapperContrib.Insert<Dapper.Empresa>(empresa, transaction);

        empresa.Contatos = new();
        for (int i = 0; i < _numeroContatosPorCompanhiaDapperContrib; i++)
        {
            var contato = new Dapper.Contato()
            {
                IdEmpresa = empresa.IdEmpresa,
                Nome = _namesDataSetDapperContrib!.FullName(),
                Telefone = _phonesDataSetDapperContrib!.PhoneNumber()
            };
            _connectionDapperContrib.Insert<Dapper.Contato>(contato, transaction);
            empresa.Contatos.Add(contato);
        }

        transaction.Commit();
        _connectionDapperContrib.Close();

        return empresa;
    }

    [IterationCleanup(Target = nameof(InputDataWithDapperContrib))]
    public void CleanupDapperContrib()
    {
        _connectionDapperContrib = null;
    }

    #endregion

    #region ADO.NET Tests

    private SqlConnection? _connectionADO;
    private Name? _namesDataSetADO;
    private PhoneNumbers? _phonesDataSetADO;
    private Address? _addressesDataSetADO;
    private Company? _companiesDataSetADO;
    private int _numeroContatosPorCompanhiaADO;

    [IterationSetup(Target = nameof(InputDataWithADO))]
    public void SetupADO()
    {
        _connectionADO = new SqlConnection(Configurations.BaseADO);
        _namesDataSetADO = new Name("pt_BR");
        _phonesDataSetADO = new PhoneNumbers("pt_BR");
        _addressesDataSetADO = new Address("pt_BR");
        _companiesDataSetADO = new Company("pt_BR");
        _numeroContatosPorCompanhiaADO = GetNumeroContatosPorCompanhia();
    }

    [Benchmark]
    public Dapper.Empresa InputDataWithADO()
    {
        var empresa = new Dapper.Empresa()
        {
            Nome = _companiesDataSetADO!.CompanyName(),
            CNPJ = _companiesDataSetADO!.Cnpj(includeFormatSymbols: false),
            Cidade = _addressesDataSetADO!.City()
        };

        _connectionADO!.Open();
        var transaction = _connectionADO.BeginTransaction();

        using var commandInsertEmpresa = new SqlCommand(
            "INSERT INTO dbo.Empresas (CNPJ, Nome, Cidade) VALUES (@CNPJ, @Nome, @Cidade);" +
            Environment.NewLine +
            "SELECT CAST(SCOPE_IDENTITY() as int)",
            _connectionADO, transaction);
        commandInsertEmpresa.Parameters.AddWithValue("@CNPJ", empresa.CNPJ);
        commandInsertEmpresa.Parameters.AddWithValue("@Nome", empresa.Nome);
        commandInsertEmpresa.Parameters.AddWithValue("@Cidade", empresa.Cidade);
        empresa.IdEmpresa = (int)commandInsertEmpresa.ExecuteScalar();

        empresa.Contatos = new();
        for (int i = 0; i < _numeroContatosPorCompanhiaADO; i++)
        {
            var contato = new Dapper.Contato()
            {
                IdEmpresa = empresa.IdEmpresa,
                Nome = _namesDataSetADO!.FullName(),
                Telefone = _phonesDataSetADO!.PhoneNumber()
            };
            using var commandInsertContato = new SqlCommand(
                "INSERT INTO dbo.Contatos (Nome, Telefone, IdEmpresa) VALUES (@Nome, @Telefone, @IdEmpresa);" +
                Environment.NewLine +
                "SELECT CAST(SCOPE_IDENTITY() as int)",
                _connectionADO, transaction);
            commandInsertContato.Parameters.AddWithValue("@Nome", contato.Nome);
            commandInsertContato.Parameters.AddWithValue("@Telefone", contato.Telefone);
            commandInsertContato.Parameters.AddWithValue("@IdEmpresa", contato.IdEmpresa);
            contato.IdContato = (int)commandInsertContato.ExecuteScalar();

            empresa.Contatos.Add(contato);
        }

        transaction.Commit();
        _connectionADO.Close();

        return empresa;
    }

    [IterationCleanup(Target = nameof(InputDataWithADO))]
    public void CleanupADO()
    {
        _connectionADO = null;
    }

    #endregion
}