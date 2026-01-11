using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoveeAPIController.src.Models
{
    public class OpenWeatherResponse
    {
        [JsonPropertyName("sys")]
        public SysData Sys { get; set; } = default!;
    }

    public sealed class SysData
    {
        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }
    }

}
