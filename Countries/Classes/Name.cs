using Newtonsoft.Json;
 

namespace Countries.Classes
{
    public class Name
    {
        [JsonProperty("common")]
        public string commonName { get; set; }
        
    }
}
