using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FirebaseExperiment01.Models;

public class Settings
{
    [JsonPropertyName("google_web_application_cliend_id")]
    public string GoogleWebApplicationClientId { get; set; }
}
