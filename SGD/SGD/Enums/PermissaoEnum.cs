using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;
namespace SGD.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PermissaoEnum
    {
        Admin = 1,
        Preparo = 2
    }
}
