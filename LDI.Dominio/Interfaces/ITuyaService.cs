using LDI.Dominio.Entidades;

namespace LDI.Dominio.Interfaces
{

    public interface ITuyaService
    {
        Task<RespostaAutenticacao> AutenticarAsync();
        Task<List<Dispositivo>> ObterDispositivosAsync();
    }
}
