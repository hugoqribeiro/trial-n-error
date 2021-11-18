using System.Text.Json.Serialization;

namespace ClassLibrary1
{
    [JsonSerializable(typeof(MyModel))]
    internal partial class SerializerContext : JsonSerializerContext
    {
    }
}
