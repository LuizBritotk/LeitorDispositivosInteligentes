using LDI.Aplicacao.CasosDeUso;
using LDI.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace LDI.Api.Controllers
{
    /// <summary>
    /// Controller para integração com o Home Assistant.
    /// </summary>
    [ApiController]
    [Route("v1/[controller]")]
    public class HomeAssistantController : ControllerBase
    {
        private readonly GerenciarDispositivosHomeAssistant _gerenciarDispositivosHomeAssistant;

        public HomeAssistantController(GerenciarDispositivosHomeAssistant gerenciarDispositivosHomeAssistant)
        {
            _gerenciarDispositivosHomeAssistant = gerenciarDispositivosHomeAssistant ?? throw new ArgumentNullException(nameof(gerenciarDispositivosHomeAssistant));
            Console.WriteLine("[INFO] Controller HomeAssistantController inicializado.");
        }

        /// <summary>
        /// Obtém o estado de uma entidade específica no Home Assistant.
        /// </summary>
        /// <param name="entidadeId">ID da entidade a ser buscada.</param>
        /// <returns>O estado da entidade.</returns>
        [HttpGet("estado/{entidadeId}")]
        public async Task<IActionResult> ObterEstadoEntidade(string entidadeId)
        {
            if (string.IsNullOrWhiteSpace(entidadeId))
            {
                Console.WriteLine("[WARN] O ID da entidade está vazio.");
                return BadRequest("O ID da entidade não pode ser vazio.");
            }

            try
            {
                Console.WriteLine($"[INFO] Buscando estado para a entidade com ID: {entidadeId}");
                var resposta = await _gerenciarDispositivosHomeAssistant.ObterEstadoEntidadeAsync(entidadeId);
                if (resposta.Erro)
                {
                    Console.WriteLine($"[ERROR] Erro ao obter estado da entidade {entidadeId}: {resposta.Msg}");
                    return StatusCode(resposta.HttpCode, resposta);
                }

                Console.WriteLine($"[INFO] Estado da entidade {entidadeId} obtido com sucesso.");
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Erro ao obter estado da entidade {entidadeId}: {ex.Message}");
                return StatusCode(500, new { Message = "Erro ao obter estado da entidade.", Detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Chama um serviço no Home Assistant.
        /// </summary>
        /// <param name="dominio">Domínio do serviço (ex: light, switch).</param>
        /// <param name="servico">Nome do serviço (ex: turn_on, turn_off).</param>
        /// <param name="payload">Payload com os parâmetros do serviço.</param>
        /// <returns>Resultado da chamada do serviço.</returns>
        [HttpPost("servico/{dominio}/{servico}")]
        public async Task<IActionResult> ChamarServico(string dominio, string servico, [FromBody] object payload)
        {
            if (string.IsNullOrWhiteSpace(dominio) || string.IsNullOrWhiteSpace(servico))
            {
                Console.WriteLine("[WARN] Domínio ou nome do serviço não informado.");
                return BadRequest("Domínio e nome do serviço são obrigatórios.");
            }

            try
            {
                Console.WriteLine($"[INFO] Chamando serviço {dominio}/{servico}.");
                var resposta = await _gerenciarDispositivosHomeAssistant.ControlarDispositivoAsync(dominio, servico, payload);
                if (resposta.Erro)
                {
                    Console.WriteLine($"[ERROR] Falha ao chamar o serviço {dominio}/{servico}: {resposta.Msg}");
                    return StatusCode(resposta.HttpCode, resposta);
                }

                Console.WriteLine($"[INFO] Serviço {dominio}/{servico} chamado com sucesso.");
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Erro ao chamar o serviço {dominio}/{servico}: {ex.Message}");
                return StatusCode(500, new { Message = "Erro ao chamar o serviço.", Detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Obtém todas as entidades disponíveis no Home Assistant.
        /// </summary>
        /// <returns>Lista de entidades.</returns>
        [HttpGet("entidades")]
        public async Task<IActionResult> ObterEntidades()
        {
            try
            {
                Console.WriteLine("[INFO] Buscando todas as entidades do Home Assistant.");
                var resposta = await _gerenciarDispositivosHomeAssistant.ExecutarAsync();

                if (resposta.Erro || resposta.Data == null || !resposta.Data.Any())
                {
                    Console.WriteLine("[INFO] Nenhuma entidade encontrada ou falha na autenticação.");
                    return NotFound(resposta);
                }

                Console.WriteLine("[INFO] Entidades obtidas com sucesso.");
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Erro ao obter entidades: {ex.Message}");
                return StatusCode(500, new { Message = "Erro ao obter entidades.", Detalhes = ex.Message });
            }
        }
    }
}