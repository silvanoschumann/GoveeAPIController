# Govee Lightbar Controller

 

🚧 This project is still under work! Expect changes and improvements. 🚧

A WPF application written in C# that allows users to control their Govee Lightbars via HTTP requests. The application provides functionalities such as turning the lights on/off, adjusting brightness, changing colors, and setting color temperature.


## Features

- 🟢 Turn Lights On/Off

- 🎨 Change Color (RGB)

- 🌞 Adjust Brightness

- 🔥 Set Color Temperature

- 🔄 Retrieve Device State


## Technologies Used

- C# with WPF for UI

- HTTP Requests (GET, POST, PUT) for API communication

- Newtonsoft.Json for JSON serialization


## Installation

### Prerequisites

- .NET 6 or higher

- A valid Govee API Key (Get it from Govee Developer Portal)

### Setup

1. Clone the repository:


```sh
git clone https://github.com/yourusername/Govee-Lightbar-Controller.git
```

2. Navigate to the project directory:
```sh
cd Govee-Lightbar-Controller
```
3. Open the solution in Visual Studio and build the project.


## Usage

### Configure API Key & Device

Before running the application, ensure you have the correct API key and device details. These can be set in ```HttpService.cs```:
```csharp
HttpService httpService = new HttpService("YOUR_DEVICE_ID", "YOUR_DEVICE_MODEL", "YOUR_API_KEY");
```

### Available Controls

#### Turn Lights On/Off
```csharp
await httpService.ToggleLight("on"); // Turn on
await httpService.ToggleLight("off"); // Turn off
```
#### Set Brightness
```csharp
await httpService.SetBrightness(50); // Set brightness (0-100)
```
#### Change Color (RGB)
```csharp
await httpService.SetColor(new RgbColor { r = 255, g = 0, b = 0 }); // Set to red
```
#### Set Color Temperature
```csharp
await httpService.SetColorTemp(3000); // Set color temperature (2000-9000K)
```
#### Get Device State
```csharp
var state = await httpService.GetDeviceState();
Console.WriteLine(state);
```
## Contributing

Feel free to submit pull requests or open issues to improve the project!

## License

This project is licensed under the MIT License.




⭐ If you like this project, don't forget to give it a star!

