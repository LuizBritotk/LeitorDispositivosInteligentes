using System.Text.Json.Serialization;

namespace LDI.Dominio.Entidades
{
    public class RespostaPadrao<T>
    {
        [JsonPropertyName("erro")]
        public bool Erro { get; set; }

        [JsonPropertyName("httpCode")]
        public int HttpCode { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}