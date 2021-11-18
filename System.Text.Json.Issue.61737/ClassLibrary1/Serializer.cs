using System.Text.Json;

namespace ClassLibrary1
{
    public partial class Serializer
    {
        public static string Serialize(MyModel model)
        {
            return JsonSerializer.Serialize(
                model,
                SerializerContext.Default.MyModel);
        }

        public static string SerializeWithGenerics<T>(T model)
        {
            return JsonSerializer.Serialize(
                model,
                typeof(T),
                SerializerContext.Default);
        }
    }
}
