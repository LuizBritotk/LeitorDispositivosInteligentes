namespace LDI.Dominio.Interfaces;

using LDI.Dominio.Entidades;

public interface IHomeAssistantService
{
    /// <summary>
    /// Obtém o estado de uma entidade específica no Home Assistant.
    /// </summary>
    /// <param name="entidadeId">ID da entidade no Home Assistant.</param>
    /// <returns>O estado da entidade, ou null se não for encontrada.</returns>
    Task<EstadoEntidade?> ObterEstadoEntidadeAsync(string entidadeId);

    /// <summary>
    /// Chama um serviço no Home Assistant para uma entidade específica.
    /// </summary>
    /// <param name="dominio">Domínio do serviço (ex: light, switch).</param>
    /// <param name="servico">Nome do serviço a ser chamado.</param>
    /// <param name="payload">Payload contendo os parâmetros do serviço.</param>
    /// <returns>Retorna true se o serviço foi chamado com sucesso.</returns>
    Task<bool> ChamarServicoAsync(string dominio, string servico, object payload);

    /// <summary>
    /// Obtém a lista de todas as entidades disponíveis no Home Assistant.
    /// </summary>
    /// <returns>Lista de entidades disponíveis.</returns>
    Task<List<ApiEntidade>> ObterEntidadesAsync();
}
