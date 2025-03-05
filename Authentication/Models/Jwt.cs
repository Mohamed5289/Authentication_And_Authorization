using System.Text.Json.Serialization;

namespace Authentication.Models
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

        [JsonPropertyName("ExpireOfDay")]
        public int ExpireOfDay { get; set; }
    }
}
