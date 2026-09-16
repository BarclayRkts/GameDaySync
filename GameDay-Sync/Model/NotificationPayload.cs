namespace GameDay_Sync.Model;

public class NotificationPayload
{
    [System.Text.Json.Serialization.JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}