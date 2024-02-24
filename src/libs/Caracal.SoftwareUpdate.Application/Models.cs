using System.Text.Json.Serialization;

namespace Caracal.SoftwareUpdate.Application;

public record UpdateRequest(
    [property: JsonPropertyName("id")] Guid Id, 
    [property: JsonPropertyName("name")] string Name, 
    [property: JsonPropertyName("chunks")] List<Chunk> Chunks);
public record Chunk(
    [property: JsonPropertyName("name")] string Name, 
    [property: JsonPropertyName("artifacts")] List<Artifact> Artifacts);
public record Artifact(
    [property: JsonPropertyName("name")] string Name
);