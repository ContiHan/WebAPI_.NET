using System.Text.Json.Serialization;

namespace EntityFramework_DotNet7_SQLServer.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RpgClass
{
    Knight = 1,
    Mage = 2,
    Priest = 3
}