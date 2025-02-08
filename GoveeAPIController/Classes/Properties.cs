using GoveeAPIController.src.Models;

namespace GoveeAPIController.Classes;

public record Properties
{
    public bool online { get; set; }
    public string powerState { get; set; }
    public int brightness { get; set; }
    public int? colorTem { get; set; }
    public RgbColor color { get; set; }
}
