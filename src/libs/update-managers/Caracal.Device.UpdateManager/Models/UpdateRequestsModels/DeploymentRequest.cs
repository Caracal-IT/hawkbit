// ReSharper disable ClassNeverInstantiated.Global
namespace Caracal.Device.UpdateManager.Models.UpdateRequestsModels;

public sealed class DeploymentRequest
{
    public required int Id { get; set; }
    public required Deployment Deployment { get; set; }
}