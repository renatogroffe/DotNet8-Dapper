using APIRegioes.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();


builder.Services.AddScoped<RegioesRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RegioesContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BaseDadosGeograficos"));
    if (isDevelopment)
        options.EnableSensitiveDataLogging();
});

if (isDevelopment)
    builder.Services.AddMiniProfiler(options =>
        options.RouteBasePath = "/profiler").AddEntityFramework();

var app = builder.Build();

if (isDevelopment)
{
    var logger = app.Logger;
    logger.LogInformation("Ativando o middleware do MiniProfiler...");

    // Rotas possíveis com a configuração do MiniProfiler:
    logger.LogInformation("MiniProfiler - Última operação: /profiler/results");
    logger.LogInformation("MiniProfiler - Listagem de todas as operações: /profiler/results-index");
    logger.LogInformation("MiniProfiler - Operação específica: /profiler/results?id=<Guid Profiler>");

    app.UseMiniProfiler();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();