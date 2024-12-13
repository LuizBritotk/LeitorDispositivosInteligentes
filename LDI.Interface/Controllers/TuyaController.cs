using LDI.Aplicacao.CasosDeUso;
using LDI.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace LDI.Interface.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class TuyaController : ControllerBase
    {
        private readonly GerenciarDispositivosTuya _gerenciarDispositivos;

        public TuyaController(GerenciarDispositivosTuya gerenciarDispositivos)
        {
            _gerenciarDispositivos = gerenciarDispositivos;
        }

        [HttpGet]
        public async Task<IActionResult> ObterDispositivos()
        {
            try
            {
                Console.WriteLine("[INFO] Iniciando requisição para obter dispositivos...");
                var resposta = await _gerenciarDispositivos.ExecutarAsync();

                if (resposta.Erro || resposta.Data == null || !resposta.Data.Any())
                {
                    Console.WriteLine("[INFO] Nenhum dispositivo encontrado ou falha na autenticação.");
                    return NotFound(resposta);
                }

                Console.WriteLine("[INFO] Dispositivos obtidos com sucesso.");
                return Ok(resposta.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao obter dispositivos: {ex.Message}");
                return StatusCode(500, new { Message = "Erro interno ao processar a requisição.", Detalhes = ex.Message });
            }
        }
    }
}