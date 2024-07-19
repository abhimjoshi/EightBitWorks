using System.Text.Json.Serialization;

namespace EightBitWorks.Web.Models;

public class RecaptchaResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("challenge_ts")]
    public string ChallengeTs { get; set; }
    
    [JsonPropertyName("hostname")]
    public string Hostname { get; set; }
}