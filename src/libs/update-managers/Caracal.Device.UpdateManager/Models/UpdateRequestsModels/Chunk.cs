// ReSharper disable ClassNeverInstantiated.Global
namespace Caracal.Device.UpdateManager.Models.UpdateRequestsModels;

public sealed class Chunk
{
    public required string Part { get; set; }
    public required string Version { get; set; }
    public required string Name { get; set; }
    public required List<Artifact> Artifacts { get; set; }
}