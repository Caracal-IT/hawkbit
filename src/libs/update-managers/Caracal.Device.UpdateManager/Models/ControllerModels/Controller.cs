namespace Caracal.Device.UpdateManager.Models.ControllerModels;

public sealed class Controller
{
    public required Config Config { get; set; }
    
    public required Links Links { get; set; }
}