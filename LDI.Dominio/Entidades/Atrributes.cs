using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LDI.Dominio.Entidades
{

    public class Attributes
    {
        [JsonPropertyName("auto_update")]
        public bool AutoUpdate { get; set; }

        [JsonPropertyName("display_precision")]
        public int DisplayPrecision { get; set; }

        [JsonPropertyName("installed_version")]
        public string InstalledVersion { get; set; }

        [JsonPropertyName("in_progress")]
        public bool InProgress { get; set; }

        [JsonPropertyName("latest_version")]
        public string LatestVersion { get; set; }

        [JsonPropertyName("release_summary")]
        public string ReleaseSummary { get; set; }

        [JsonPropertyName("release_url")]
        public string ReleaseUrl { get; set; }

        [JsonPropertyName("skipped_version")]
        public string SkippedVersion { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("update_percentage")]
        public int? UpdatePercentage { get; set; }

        [JsonPropertyName("entity_picture")]
        public string EntityPicture { get; set; }

        [JsonPropertyName("friendly_name")]
        public string FriendlyName { get; set; }

        [JsonPropertyName("supported_features")]
        public int SupportedFeatures { get; set; }
    }
}
