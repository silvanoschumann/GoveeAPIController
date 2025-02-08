namespace GoveeAPIController.src.Models;

public class Command<T>
{
    public string name { get; set; } = "";
    public T value { get; set; }
}