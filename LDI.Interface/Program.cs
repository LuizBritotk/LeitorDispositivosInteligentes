using LDI.Infraestrutura.Servicos;
using LDI.Dominio.Interfaces;
using LDI.Aplicacao.CasosDeUso;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configurar serviços
ConfigurarServicos(builder.Services);

// Configurar aplicação
var app = builder.Build();
ConfigurarAplicacao(app);

// Tentar autenticar logo ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var tuyaService = scope.ServiceProvider.GetRequiredService<ITuyaService>();
    var homeAssistantService = scope.ServiceProvider.GetRequiredService<IHomeAssistantService>();

    try
    {
        await tuyaService.AutenticarAsync();
        Console.WriteLine("[INFO] Autenticação com a API Tuya realizada com sucesso.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERRO] Falha na autenticação com a API Tuya: {ex.Message}");
    }

    try
    {
        var entidades = await homeAssistantService.ObterEntidadesAsync();
        Console.WriteLine($"[INFO] Conexão com o Home Assistant validada. {entidades.Count} entidades encontradas.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERRO] Falha na conexão com o Home Assistant: {ex.Message}");
    }
}

app.Run();

void ConfigurarServicos(IServiceCollection services)
{
    services.AddScoped<ITuyaService, TuyaService>(provider =>
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        var apiKey = configuration["Tuya:ApiKey"];
        var apiSecret = configuration["Tuya:ApiSecret"];
        return new TuyaService(apiKey, apiSecret);
    });

    services.AddScoped<IHomeAssistantService, HomeAssistantService>(provider =>
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        var baseUrl = configuration["HomeAssistant:BaseUrl"];
        var token = configuration["HomeAssistant:Token"];
        return new HomeAssistantService(baseUrl, token);
    });

    services.AddScoped<ITuyaService>(provider =>
        new TuyaService("kneh9axn7stvejce5een", "25b449687f2c4431aa8d90a1a80566d6"));

    services.AddScoped<IHomeAssistantService>(provider =>
        new HomeAssistantService("http://homeassistant.local:8123/", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiIzYTcxMjljYmIxZTg0MmJlODQzMjFmNTQ1MWIzYTZkNyIsImlhdCI6MTczNDExODc0NCwiZXhwIjoyMDQ5NDc4NzQ0fQ.LlgnrRv_j2Zf5Iulz5PJ_WYFz87RqX9kttwGZIJC26o"));

    services.AddScoped<GerenciarDispositivosTuya>();
    services.AddScoped<GerenciarDispositivosHomeAssistant>(); // Adicione esta linha

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    Console.WriteLine("[INFO] Serviços configurados com sucesso.");
}

void ConfigurarAplicacao(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        Console.WriteLine("[INFO] Swagger habilitado no ambiente de desenvolvimento.");
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    Console.WriteLine("[INFO] Aplicação configurada e pronta para execução.");
}
