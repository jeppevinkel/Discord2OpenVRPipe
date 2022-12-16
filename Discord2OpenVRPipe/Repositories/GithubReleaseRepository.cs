using System.Text.Json.Serialization;

namespace Discord2OpenVRPipe.Repositories;

public record GithubReleaseRepository(
    [property: JsonPropertyName("url")]string Url,
    [property: JsonPropertyName("html_url")]string HtmlUrl,
    [property: JsonPropertyName("id")]long Id,
    [property: JsonPropertyName("tag_name")]string TagName,
    [property: JsonPropertyName("name")]string Name,
    [property: JsonPropertyName("created_at")]string CreatedAt,
    [property: JsonPropertyName("published_at")]string PublishedAt);