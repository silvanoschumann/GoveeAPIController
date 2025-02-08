using GoveeAPIController.src.Models;
using GoveeAPIController.src.Services.Interfaces;

namespace GoveeAPIController.src.Services;

public class HttpService : IHttpService
{
    private readonly IDeviceService _deviceService;
    private readonly IApiService _apiService;

    public HttpService(IDeviceService deviceService, IApiService apiService)
    {
        _deviceService = deviceService;
        _apiService = apiService;
    }

    public async Task<bool> SetColorTemp(int colorTemp)
    {
        var command = new Command<int> { name = "colorTem", value = colorTemp };
        return await _apiService.SendCommand(_deviceService.Device, _deviceService.Model, command);
    }

    public async Task<bool> SetBrightness(int brightness)
    {
        var command = new Command<int> { name = "brightness", value = brightness };
        return await _apiService.SendCommand(_deviceService.Device, _deviceService.Model, command);
    }

    public async Task<bool> SetColor(RgbColor color)
    {
        var command = new Command<RgbColor> { name = "color", value = color };
        return await _apiService.SendCommand(_deviceService.Device, _deviceService.Model, command);
    }

    public async Task<bool> ToggleLight(string lightSwitch)
    {
        var command = new Command<string> { name = "turn", value = lightSwitch };
        return await _apiService.SendCommand(_deviceService.Device, _deviceService.Model, command);
    }

    public async Task<DeviceStateResponse> GetDeviceState()
    {
        return await _apiService.GetDeviceState(_deviceService.Device, _deviceService.Model);
    }
}
