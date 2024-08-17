using ControlzEx.Standard;
using System.Reflection;
using System.Windows.Media.Media3D;

namespace GoveeAPIController.src.Services;

public class HttpService
{
    private string _device;
    public string Device
    {
        get => _device; set
        {
            if (_device != value)
            {
                _device = value;
            }
        }
    }

    private string _model;
    public string Model
    {
        get => _model; set
        {
            if (_model != value)
            {
                _model = value;
            }
        }
    }

    public string _apiKey;
    public string ApiKey
    {
        get => _apiKey; set
        {
            if (_apiKey != value)
            {
                _apiKey = value;
            }
        }
    }

    private static readonly HttpClient client = new();

    public HttpService(string device, string model, string apikey)
    {
        Device = device;
        Model = model;
        ApiKey = apikey;
    }

    public async Task<bool> SetColorTemp(int colorTemp)
    {
        GoveeLightBar<int> govee = new()
        {
            device = Device,
            model = Model,
            cmd = new Command<int>()
            {
                name = "colorTem",
                value = colorTemp
            },
        };

        string json = JsonConvert.SerializeObject(govee, Formatting.Indented);

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "https://developer-api.govee.com/v1/devices/control");
            request.Headers.Add("Govee-API-Key", ApiKey);

            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }

        return true;
    }
    public async Task<bool> SetBrightness(int brightness)
    {
        GoveeLightBar<int> Govee = new()
        {
            device = Device,
            model = Model,
            cmd = new Command<int>()
            {
                name = "brightness",
                value = brightness
            },
        };

        string json = JsonConvert.SerializeObject(Govee, Formatting.Indented);

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "https://developer-api.govee.com/v1/devices/control");
            request.Headers.Add("Govee-API-Key", ApiKey);

            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }

        return true;
    }
    public async Task<bool> SetColor(RgbColor color)
    {
        GoveeLightBar<RgbColor> Govee = new()
        {
            device = Device,
            model = Model,
            cmd = new Command<RgbColor>()
            {
                name = "color",
                value = color
            },
        };

        string json = JsonConvert.SerializeObject(Govee, Formatting.Indented);

        try
        {
            HttpRequestMessage request = new(HttpMethod.Put, "https://developer-api.govee.com/v1/devices/control");
            request.Headers.Add("Govee-API-Key", ApiKey);

            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //TbResult = response.StatusCode.ToString();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }

        return true;
    }
    public async Task<bool> ToggleLight(string lightSwitch)
    {
        GoveeLightBar<string> Govee = new()
        {
            device = Device,
            model = Model,
            cmd = new Command<string>()
            {
                name = "turn",
                value = lightSwitch
            },
        };

        string json = JsonConvert.SerializeObject(Govee, Formatting.Indented);

        try
        {
            HttpRequestMessage request = new(HttpMethod.Put, "https://developer-api.govee.com/v1/devices/control");
            request.Headers.Add("Govee-API-Key", ApiKey);

            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //TbResult = response.StatusCode.ToString();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }
        return true;
    }

    public async Task<DeviceStateResponse> GetDeviceState()
    {
        DeviceStateResponse deviceStateResponse;

        GoveeLightBar<string> Govee = new()
        {
            device = Device,
            model = Model
        };

        JsonConvert.SerializeObject(Govee, Formatting.Indented);

        try
        {
            HttpRequestMessage request = new(HttpMethod.Get, "https://developer-api.govee.com/v1/devices/state?device=7E:F6:CD:32:37:36:49:09&model=H6056&");

            request.Headers.Add("Govee-API-Key", ApiKey);
            var content = new StringContent("", null, "text/plain");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //TbResult = response.StatusCode.ToString();
            var jsonString = await response.Content.ReadAsStringAsync();
            deviceStateResponse = System.Text.Json.JsonSerializer.Deserialize<DeviceStateResponse>(jsonString);

            if (deviceStateResponse.code != 200)
                throw new Exception($"Request failed. Status code: {deviceStateResponse.code}, Message: {deviceStateResponse.message}");


        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return null;
        }

        return deviceStateResponse;
    }
}
