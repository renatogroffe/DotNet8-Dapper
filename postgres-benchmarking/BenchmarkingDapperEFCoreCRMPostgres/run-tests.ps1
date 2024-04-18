$env:NumeroContatosPorCompanhia = "2"
$env:BaseEFCoreConnectionString = "Server=localhost;Port=5432;Database=basecrmef;User Id=postgres;Password=Postgres2024!"
$env:BaseDapperConnectionString = "Server=localhost;Port=5432;Database=basecrmdapper;User Id=postgres;Password=Postgres2024!"
$env:BaseADOConnectionString = "Server=localhost;Port=5432;Database=basecrmado;User Id=postgres;Password=Postgres2024!"
dotnet run --filter BenchmarkingDapperEFCoreCRMPostgres.Tests.* -c Release