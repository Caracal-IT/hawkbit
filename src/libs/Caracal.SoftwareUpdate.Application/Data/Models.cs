using System.Text.Json.Serialization;

// ReSharper disable ClassNeverInstantiated.Global

namespace Caracal.SoftwareUpdate.Application.Data;

public sealed record UpdateRequest(
    [property: JsonPropertyName("id")] Guid Id, 
    [property: JsonPropertyName("name")] string Name, 
    [property: JsonPropertyName("chunks")] List<Chunk> Chunks);
public sealed record Chunk(
    [property: JsonPropertyName("name")] string Name, 
    [property: JsonPropertyName("artifacts")] List<Artifact> Artifacts);
public sealed record Artifact(
    [property: JsonPropertyName("name")] string Name
);