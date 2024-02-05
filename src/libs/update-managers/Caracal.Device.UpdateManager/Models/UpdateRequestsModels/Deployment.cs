// ReSharper disable ClassNeverInstantiated.Global
namespace Caracal.Device.UpdateManager.Models.UpdateRequestsModels;

public sealed class Deployment
{
    public required string DownloadType { get; set; }
    public required string UpdateType { get; set; }
    public List<Chunk> Chunks { get; set; } = Array.Empty<Chunk>().ToList();
}