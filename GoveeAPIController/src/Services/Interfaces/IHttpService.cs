using GoveeAPIController.src.Models;

namespace GoveeAPIController.src.Services.Interfaces;

public interface IHttpService
{
    Task<bool> SetColorTemp(int colorTemp);
    Task<bool> SetBrightness(int brightness);
    Task<bool> SetColor(RgbColor color);
    Task<bool> ToggleLight(string lightSwitch);
    Task<DeviceStateResponse> GetDeviceState();
}
