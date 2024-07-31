namespace GoveeAPIController.Classes;

public record DeviceState
{
    public string device { get; set; }
    public string model { get; set; }
    public Properties[] properties { get; set; } = new Properties[1];
};
