using LDI.Dominio.Entidades;
using LDI.Dominio.Interfaces;

namespace LDI.Aplicacao.CasosDeUso
{
    public class GerenciarDispositivosHomeAssistant
    {
        private readonly IHomeAssistantService _homeAssistantService;

        public GerenciarDispositivosHomeAssistant(IHomeAssistantService homeAssistantService)
        {
            _homeAssistantService = homeAssistantService ?? throw new ArgumentNullException(nameof(homeAssistantService));
        }

        /// <summary>
        /// Obtém todos os dispositivos ou entidades registradas no Home Assistant.
        /// </summary>
        public async Task<RespostaPadrao<List<ApiEntidade>>> ObterDispositivosAsync()
        {
            try
            {
                var entidades = await _homeAssistantService.ObterEntidadesAsync();
                return new RespostaPadrao<List<ApiEntidade>>
                {
                    Erro = false,
                    HttpCode = 200,
                    Msg = "Sucesso",
                    Data = entidades
                };
            }
            catch (Exception ex)
            {
                return new RespostaPadrao<List<ApiEntidade>>
                {
                    Erro = true,
                    HttpCode = 500,
                    Msg = "Falha ao obter dispositivos do Home Assistant.",
                    Data = new List<ApiEntidade>()
                };
            }
        }

        /// <summary>
        /// Obtém o estado de uma entidade específica do Home Assistant.
        /// </summary>
        public async Task<RespostaPadrao<EstadoEntidade>> ObterEstadoEntidadeAsync(string entidadeId)
        {
            try
            {
                var estado = await _homeAssistantService.ObterEstadoEntidadeAsync(entidadeId);
                if (estado == null)
                {
                    return new RespostaPadrao<EstadoEntidade>
                    {
                        Erro = true,
                        HttpCode = 404,
                        Msg = "Estado não encontrado.",
                        Data = null
                    };
                }

                return new RespostaPadrao<EstadoEntidade>
                {
                    Erro = false,
                    HttpCode = 200,
                    Msg = "Sucesso",
                    Data = estado
                };
            }
            catch (Exception ex)
            {
                return new RespostaPadrao<EstadoEntidade>
                {
                    Erro = true,
                    HttpCode = 500,
                    Msg = $"Falha ao obter estado da entidade {entidadeId}.",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Controla um dispositivo/entidade, alterando seu estado (ligar/desligar, etc).
        /// </summary>
        public async Task<RespostaPadrao<bool>> ControlarDispositivoAsync(string dominio, string servico, object payload)
        {
            try
            {
                var resultado = await _homeAssistantService.ChamarServicoAsync(dominio, servico, payload);
                return new RespostaPadrao<bool>
                {
                    Erro = false,
                    HttpCode = 200,
                    Msg = "Sucesso",
                    Data = resultado
                };
            }
            catch (Exception ex)
            {
                return new RespostaPadrao<bool>
                {
                    Erro = true,
                    HttpCode = 500,
                    Msg = $"Falha ao controlar dispositivo {dominio}/{servico}.",
                    Data = false
                };
            }
        }

        /// <summary>
        /// Executa o fluxo para gerenciar dispositivos do Home Assistant.
        /// </summary>
        public async Task<RespostaPadrao<List<ApiEntidade>>> ExecutarAsync()
        {
            Console.WriteLine("[INFO] Iniciando fluxo para gerenciar dispositivos...");

            try
            {
                Console.WriteLine("[INFO] Obtendo dispositivos...");
                var dispositivos = await _homeAssistantService.ObterEntidadesAsync();

                return new RespostaPadrao<List<ApiEntidade>>
                {
                    Erro = false,
                    HttpCode = 200,
                    Msg = "Sucesso",
                    Data = dispositivos
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao obter dispositivos: {ex.Message}");
                return new RespostaPadrao<List<ApiEntidade>>
                {
                    Erro = true,
                    HttpCode = 500,
                    Msg = "Falha ao obter dispositivos",
                    Data = new List<ApiEntidade>()
                };
            }
        }
    }
}