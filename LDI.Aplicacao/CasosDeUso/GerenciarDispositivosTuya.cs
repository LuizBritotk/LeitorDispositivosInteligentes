using LDI.Dominio.Entidades;
using LDI.Dominio.Interfaces;

namespace LDI.Aplicacao.CasosDeUso
{
    public class GerenciarDispositivosTuya
    {
        private readonly ITuyaService _tuyaService;

        public GerenciarDispositivosTuya(ITuyaService tuyaService)
        {
            _tuyaService = tuyaService;
        }

        public async Task<RespostaPadrao<List<Dispositivo>>> ExecutarAsync()
        {
            Console.WriteLine("[INFO] Iniciando fluxo para gerenciar dispositivos...");

            // Tenta autenticar e verifica o sucesso
            var respostaAutenticacao = await _tuyaService.AutenticarAsync();

            if (!respostaAutenticacao.Success)
            {
                Console.WriteLine($"[ERRO] Autenticação falhou. Código: {respostaAutenticacao.Code}, Mensagem: {respostaAutenticacao.Msg}");
                return new RespostaPadrao<List<Dispositivo>>
                {
                    Erro = true,
                    HttpCode = respostaAutenticacao.Code,
                    Msg = respostaAutenticacao.Msg,
                    Data = new List<Dispositivo>()
                };
            }

            Console.WriteLine("[INFO] Autenticação bem-sucedida. Obtendo dispositivos...");
            var dispositivos = await _tuyaService.ObterDispositivosAsync();

            return new RespostaPadrao<List<Dispositivo>>
            {
                Erro = false,
                HttpCode = 200,
                Msg = "Sucesso",
                Data = dispositivos
            };
        }
    }
}