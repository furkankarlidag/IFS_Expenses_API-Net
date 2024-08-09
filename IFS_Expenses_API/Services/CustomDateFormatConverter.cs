using Newtonsoft.Json;

namespace IFS_Expenses_API.Services
{
    public class CustomDateFormatConverter : JsonConverter<DateTime>
    {


        private readonly string _dateFormat;

        public CustomDateFormatConverter(string dateFormat)
        {
            _dateFormat = dateFormat;
        }
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return DateTime.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(_dateFormat));

        }
    }
}
