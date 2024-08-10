using Newtonsoft.Json;
 

namespace Countries.Classes
{
    public class Flag
    {
        [JsonProperty("png")]
        public string Png { get; set; }
    }
}
