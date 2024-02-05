// ReSharper disable ClassNeverInstantiated.Global
namespace Caracal.Device.UpdateManager.Models.UpdateRequestsModels;

public sealed class Artifact
{
    public required string Filename { get; set; }
    
    public required Hashes Hashes { get; set; }
    
    public required int Size { get; set; }
    
    public required Links Links { get; set; }
}