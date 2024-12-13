using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LDI.Dominio.Entidades;
using LDI.Dominio.Interfaces;
using LDI.Infraestrutura.Tratamentos;
using Microsoft.Extensions.Logging;

namespace LDI.Infraestrutura.Servicos;

public class TuyaService : ITuyaService
{
    private const string BaseUrl = "https://openapi.tuyaus.com";
    private readonly string _accessId;
    private readonly string _accessSecret;
    private readonly HttpClient _httpClient;
    private string? _token;

    public TuyaService(string accessId, string accessSecret)
    {
        _accessId = accessId ?? throw new ArgumentNullException(nameof(accessId), "Access ID não pode ser nulo.");
        _accessSecret = accessSecret ?? throw new ArgumentNullException(nameof(accessSecret), "Access Secret não pode ser nulo.");
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    /// <summary>
    /// Autentica na API Tuya e obtém o token de acesso.
    /// </summary>
    public async Task<RespostaAutenticacao> AutenticarAsync()
    {
        Console.WriteLine("[INFO] Iniciando autenticação na API Tuya...");

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        Console.WriteLine($"[DEBUG] Timestamp: {timestamp}");

        var sign = GerarAssinatura(_accessId, timestamp, _accessSecret);

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("client_id", _accessId);
        _httpClient.DefaultRequestHeaders.Add("sign", sign);
        _httpClient.DefaultRequestHeaders.Add("t", timestamp);
        _httpClient.DefaultRequestHeaders.Add("sign_method", "HMAC-SHA256");

        Console.WriteLine("[DEBUG] Headers preparados:");
        Console.WriteLine($"[DEBUG] client_id: {_accessId}");
        Console.WriteLine($"[DEBUG] timestamp: {timestamp}");
        Console.WriteLine($"[DEBUG] sign: {sign}");

        try
        {
            var responseMessage = await _httpClient.GetAsync("/v1.0/token?grant_type=1");
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] Resposta da API: {responseContent}");


            var response = JsonSerializer.Deserialize<RespostaAutenticacao>(responseContent);
            if (response == null)
            {
                Console.WriteLine("[ERRO] Resposta da API não pode ser desserializada.");
                return new RespostaAutenticacao
                {
                    Success = false,
                    Msg = "Resposta inválida ou nula.",
                    Code = -1
                };
            }

            if (!responseMessage.IsSuccessStatusCode || !response.Success || response.Result == null)
            {
                Console.WriteLine($"[ERRO] Falha na autenticação: {response.Msg} (Code: {response.Code})");
                return response;
            }

            _token = response.Result.Access_Token;
            Console.WriteLine("[INFO] Autenticação realizada com sucesso. Token obtido.");

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO] Exceção ao autenticar na API Tuya: {ex.Message}");
            return new RespostaAutenticacao
            {
                Success = false,
                Msg = ex.Message,
                Code = -1
            };
        }
    }

    /// <summary>
    /// Obtém a lista de dispositivos disponíveis na conta.
    /// </summary>
    public async Task<List<Dispositivo>> ObterDispositivosAsync()
    {
        if (string.IsNullOrEmpty(_token))
        {
            Logger.Error("Tentativa de buscar dispositivos sem autenticação.");
            throw new InvalidOperationException("Autenticação não realizada. Chame AutenticarAsync primeiro.");
        }

        Logger.Info("Buscando dispositivos na API Tuya...");

        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

            var response = await _httpClient.GetFromJsonAsync<RespostaDispositivos>("/v1.0/devices");
            if (response == null || response.Result == null)
            {
                throw new ApiException("Erro ao obter dispositivos: Resposta nula ou inválida.");
            }

            var dispositivos = response.Result.Select(item => new Dispositivo
            {
                Id = item.Id,
                Nome = item.Name,
                Online = item.Online
            }).ToList();

            Logger.Success($"{dispositivos.Count} dispositivos encontrados.");
            return dispositivos;
        }
        catch (Exception ex)
        {
            Logger.Error($"Falha ao obter dispositivos: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Gera a assinatura (sign) exigida pela API Tuya.
    /// </summary>
    /// <summary>
    /// Gera a assinatura (sign) exigida pela API Tuya.
    /// </summary>
    /// <summary>
    /// Gera a assinatura (sign) exigida pela API Tuya.
    /// </summary>
    private string GerarAssinatura(string clientId, string timestamp, string chave)
    {
        // Gerar um nonce aleatório
        var random = new Random();
        var nonce = random.Next(100000, 999999).ToString();

        // Definir um parâmetro param (pode ser vazio ou JSON dependendo da API)
        string param = "{}";  // Caso a API espere um parâmetro JSON vazio

        // Concatenar os parâmetros corretamente, incluindo o nonce
        string textoParaAssinatura = $"client_id{clientId}&nonce{nonce}&timestamp{timestamp}&sign_methodHMAC-SHA256&param{param}";

        // Exibir a string de assinatura para verificação
        Console.WriteLine($"[DEBUG] Texto para assinatura: {textoParaAssinatura}");

        // Usar o HMAC-SHA256 para gerar a assinatura
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(chave));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(textoParaAssinatura));
        var assinatura = BitConverter.ToString(hash).Replace("-", "").ToLower();

        // Verificar assinatura gerada
        Console.WriteLine($"[DEBUG] Assinatura gerada: {assinatura}");

        return assinatura;
    }
}

