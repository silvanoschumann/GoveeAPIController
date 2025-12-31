using GoveeAPIController.src.Models;

namespace GoveeAPIController.src.Services.Interfaces;

public interface IApiService
{
    Task<bool> SendCommand<T>(Command<T> command);
    Task<DeviceStateResponse> GetDeviceState(string device, string model);
}
