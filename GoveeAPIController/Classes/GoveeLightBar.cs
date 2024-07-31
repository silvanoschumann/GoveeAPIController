namespace GoveeAPIController.Classes;

class GoveeLightBar<T>
{
    public string device { get; set; } = "";
    public string model { get; set; } = "";
    public Command<T> cmd { get; set; } = new Command<T>();
}
