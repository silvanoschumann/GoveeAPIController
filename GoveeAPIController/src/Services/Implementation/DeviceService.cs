using GoveeAPIController.src.Services.Interfaces;

namespace GoveeAPIController.src.Services.Implementation;

public class DeviceService
{
    public string Device { get; set; }
    public string Model { get; set; }

    public DeviceService(string device, string model)
    {
        Device = device;
        Model = model;
    }
}
