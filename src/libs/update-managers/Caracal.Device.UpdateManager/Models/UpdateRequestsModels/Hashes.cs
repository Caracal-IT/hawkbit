// ReSharper disable ClassNeverInstantiated.Global
namespace Caracal.Device.UpdateManager.Models.UpdateRequestsModels;

public sealed class Hashes
{
    public required string Sha1 { get; set; }
    public required string Md5 { get; set; }
    public required string Sha256 { get; set; }
}