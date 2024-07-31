namespace GoveeAPIController.Classes;
public record DeviceStateResponse
{
    public DeviceState data { get; set; }
    public string message { get; set; }
    public int code { get; set; }
}
