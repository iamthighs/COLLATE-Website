using System.Text.Json.Serialization;


namespace COLLATEFINAL.Data
{
    public class GoogleUserInfo
    {
        [JsonPropertyName("sub")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }

        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; }

        [JsonPropertyName("picture")]
        public string PictureUrl { get; set; }

        // Additional properties can be added based on your requirements
    }


}
