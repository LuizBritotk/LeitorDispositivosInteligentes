using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LDI.Dominio.Entidades
{
    public class RespostaAutenticacao
    {
        [JsonPropertyName("result")]
        public AutenticacaoResultado Result { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("t")]
        public long T { get; set; }

        [JsonPropertyName("tid")]
        public string Tid { get; set; }
    }
}