using GoveeAPIController.src.Models;
using GoveeAPIController.src.Services.Interfaces;
using System.Diagnostics;

namespace GoveeAPIController.src.Services.Implementation;

public class ApiService : IApiService
{
    private readonly HttpClient _client = new();

    public static string ApiKey => GoveeApplication.ApiKey;

    public async Task<DeviceStateResponse> GetDeviceState(string device, string model)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://developer-api.govee.com/v1/devices/state?device={device}&model={model}");
            request.Headers.Add("Govee-API-Key", ApiKey);

            HttpResponseMessage response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            DeviceStateResponse deviceStateResponse = System.Text.Json.JsonSerializer.Deserialize<DeviceStateResponse>(jsonString);

            if (deviceStateResponse.code != 200)
                throw new Exception($"Request failed. Status code: {deviceStateResponse.code}, Message: {deviceStateResponse.message}");

            return deviceStateResponse;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return null;
        }
    }

    public async Task<bool> SendCommand<T>(Command<T> command)
    {


        var govee = new GoveeLightBar<T> { device = GoveeApplication.Device, model = GoveeApplication.Model, cmd = command };
        var json = JsonConvert.SerializeObject(govee, Formatting.Indented);

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "https://developer-api.govee.com/v1/devices/control");
            request.Headers.Add("Govee-API-Key", GoveeApplication.ApiKey);
            request.Content = new StringContent(json, null, "application/json");

            HttpResponseMessage response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Debug.WriteLine(await response.Content.ReadAsStringAsync());
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }
    }
}
