namespace PersonalDeskThing.App.Core.Models.Options
{
    class SpotifyOptions
    {
        public const string Position = "Spotify";
        public string ClientId { get; set; } = string.Empty;
        public string CallbackUrl { get; set; } = string.Empty;
    }
}
