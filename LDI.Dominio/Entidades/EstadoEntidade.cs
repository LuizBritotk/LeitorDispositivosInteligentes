using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LDI.Dominio.Entidades
{
    public class EstadoEntidade
    {
        [JsonPropertyName("entity_id")]
        public string EntityId { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("attributes")]
        public Dictionary<string, object> Attributes { get; set; }

        [JsonPropertyName("last_changed")]
        public DateTime LastChanged { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("last_reported")]
        public DateTime LastReported { get; set; }

        [JsonPropertyName("context")]
        public Context Context { get; set; }
    }

    public class Context
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } 

        [JsonPropertyName("parent_id")]
        public string? ParentId { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }
}