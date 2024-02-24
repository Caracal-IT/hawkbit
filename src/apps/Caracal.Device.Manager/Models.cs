namespace Caracal.Device.Manager;

public record UpdateRequest(Guid Id, string Name, List<Chunk> Chunks);
public record Chunk(string Name, List<Artifact> Artifacts);
public record Artifact(string Name);