// ReSharper disable ClassNeverInstantiated.Global
namespace Caracal.Device.UpdateManager.Models.UpdateRequestsModels;

public sealed class Links
{
    public required Link Download { get; set; }
    public required Link Md5Sum { get; set; }
}