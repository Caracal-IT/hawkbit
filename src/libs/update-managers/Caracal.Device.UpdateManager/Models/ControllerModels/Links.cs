// ReSharper disable ClassNeverInstantiated.Global
namespace Caracal.Device.UpdateManager.Models.ControllerModels;

public sealed class Links
{
    public Link? DeploymentBase { get; set; }
    public Link? InstalledBase { get; set; }
    public Link? ConfigData { get; set; }
}