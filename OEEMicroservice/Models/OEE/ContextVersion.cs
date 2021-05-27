using System.Text.Json.Serialization;

namespace OEEMicroservice.Models.OEE
{
    public class ContextVersion
    {
        [JsonPropertyName("orion")]
        public VersionMeta Orion { get; set; }
    }

    public class VersionMeta
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("uptime")]
        public string Uptime { get; set; }

        [JsonPropertyName("git_hash")]
        public string GitHash { get; set; }

        [JsonPropertyName("compile_time")]
        public string CompileTime { get; set; }

        [JsonPropertyName("compile_by")]
        public string CompiledBy { get; set; }

        [JsonPropertyName("compile_in")]
        public string CompiledIn { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }

        [JsonPropertyName("doc")]
        public string Doc { get; set; }
    }
}
