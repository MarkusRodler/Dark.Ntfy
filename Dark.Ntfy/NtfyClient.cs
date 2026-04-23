namespace Dark.Ntfy;

public enum Prio
{
    Min = 1,
    Low = 2,
    Normal = 3,
    High = 4,
    Max = 5
}

public record NtfyMessage(
    string Topic,
    string Title,
    string? Message = null,
    Prio Prio = Prio.Normal,
    string? Tags = null,
    string? ClickUrl = null
);

public static class NtfyClient
{
    public static async Task SendNtfy(
        this HttpClient client,
        NtfyMessage msg,
        string baseUrl = "https://ntfy.sh",
        CancellationToken cancellationToken = default
    )
    {
        using HttpRequestMessage request = new(HttpMethod.Post, $"{baseUrl.TrimEnd('/')}/{msg.Topic}");
        request.Headers.Add("Title", msg.Title);
        request.Headers.Add("Priority", ((int)msg.Prio).ToString());
        if (!string.IsNullOrEmpty(msg.Tags)) request.Headers.Add("Tags", msg.Tags);
        if (!string.IsNullOrEmpty(msg.ClickUrl)) request.Headers.Add("Click", msg.ClickUrl);
        if (!string.IsNullOrEmpty(msg.Message)) request.Content = new StringContent(msg.Message);

        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
