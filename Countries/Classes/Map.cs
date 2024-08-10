using Newtonsoft.Json;
 

namespace Countries.Classes
{
    public class Map
    {
        [JsonProperty("googleMaps")]
        public string GoogleMaps { get; set; }
           
    }
}
