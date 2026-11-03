using System.Text.Json;

namespace WasmBaseServices.Services
{
    public static class ConfigParser
    {
        private const string TheKey = "clientSettings";

        public static string GetValueFromDefaultConfig(string config, string key)
        {
            try
            {
                var Config1 = JsonSerializer.Deserialize<Dictionary<string, object>>(config);

                var Level1 = Config1.FirstOrDefault(x => x.Key == TheKey).Value.ToString();

                using JsonDocument doc = JsonDocument.Parse(Level1);
                JsonElement root = doc.RootElement;
  
                var J =  root.GetProperty(key).ToString();

                if (string.IsNullOrEmpty(J))
                {
                    return string.Empty;
                }else
                {
                    return J;
                }

            }
            catch (Exception ex)
            {
                string m = ex.Message;
                return string.Empty;
            }
        }
    }
}