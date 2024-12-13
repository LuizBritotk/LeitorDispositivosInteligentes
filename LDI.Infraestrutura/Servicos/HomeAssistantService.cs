using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LDI.Dominio.Entidades;
using LDI.Dominio.Interfaces;
using LDI.Infraestrutura.Tratamentos;
using Microsoft.Extensions.Logging;

namespace LDI.Infraestrutura.Servicos;

public class HomeAssistantService : IHomeAssistantService
{
    private readonly string _baseUrl;
    private readonly string _accessToken;
    private readonly HttpClient _httpClient;

    public HomeAssistantService(string baseUrl, string accessToken)
    {
        _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl), "Base URL não pode ser nula.");
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken), "Token de acesso não pode ser nulo.");
        _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
    }

    /// <summary>
    /// Obtém o estado de uma entidade específica do Home Assistant.
    /// </summary>
    public async Task<EstadoEntidade?> ObterEstadoEntidadeAsync(string entidadeId)
    {
        Logger.Info($"[INFO] Obtendo estado da entidade '{entidadeId}' no Home Assistant...");

        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

            var responseMessage = await _httpClient.GetAsync($"/api/states/{entidadeId}");
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            Console.WriteLine($"[DEBUG] Resposta da API: {responseContent}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                Logger.Error($"[ERRO] Falha ao obter estado da entidade: {responseContent}");
                throw new ApiException($"Erro ao obter estado da entidade: {responseContent}");
            }

            var response = JsonSerializer.Deserialize<EstadoEntidade>(responseContent);
            if (response == null)
            {
                throw new ApiException("Erro ao obter estado da entidade: Resposta nula ou inválida.");
            }

            Logger.Success($"[SUCESSO] Estado da entidade '{entidadeId}' obtido com sucesso.");
            return response;
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERRO] Falha ao obter estado da entidade: {ex.Message}");
            throw;
        }
    }
    /// <summary>
    /// Chama um serviço no Home Assistant para uma entidade específica.
    /// </summary>
    public async Task<bool> ChamarServicoAsync(string dominio, string servico, object payload)
    {
        Logger.Info($"[INFO] Chamando serviço '{dominio}.{servico}' no Home Assistant...");

        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

            var response = await _httpClient.PostAsJsonAsync($"/api/services/{dominio}/{servico}", payload);
            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                Logger.Error($"[ERRO] Falha ao chamar o serviço: {erro}");
                throw new ApiException($"Erro ao chamar o serviço {dominio}.{servico}: {erro}");
            }

            Logger.Success($"[SUCESSO] Serviço '{dominio}.{servico}' chamado com sucesso.");
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERRO] Exceção ao chamar o serviço: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Obtém uma lista de entidades disponíveis no Home Assistant.
    /// </summary>
    public async Task<List<ApiEntidade>> ObterEntidadesAsync()
    {
        Logger.Info("[INFO] Obtendo lista de entidades no Home Assistant...");

        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

            var responseMessage = await _httpClient.GetAsync("/api/states");
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            Console.WriteLine($"[DEBUG] Resposta da API: {responseContent}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                Logger.Error($"[ERRO] Falha ao obter entidades: {responseContent}");
                throw new ApiException($"Erro ao obter entidades: {responseContent}");
            }

            var response = JsonSerializer.Deserialize<List<ApiEntidade>>(responseContent);
            if (response == null)
            {
                throw new ApiException("Erro ao obter entidades: Resposta nula ou inválida.");
            }

            Logger.Success($"[SUCESSO] {response.Count} entidades obtidas.");
            return response;
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERRO] Falha ao obter entidades: {ex.Message}");
            throw;
        }
    }
}