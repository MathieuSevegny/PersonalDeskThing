namespace PersonalDeskThing.App.Client.Models
{
    /// <summary>
    /// Represents a song
    /// </summary>
    public class Song
    {
        /// <summary>
        /// Name of the song
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Name of the artist
        /// </summary>
#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only
        public List<Artist> Artists { get; set; } = new();
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning restore CA1002 // Do not expose generic lists
        /// <summary>
        /// Album name
        /// </summary>
        public Album Album { get; set; } = new();
        /// <summary>
        /// Duration of the song
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// URL of the image
        /// </summary>
        public Uri ImageUrl { get; set; } = new Uri("https://via.placeholder.com/150");
        public Song()
        {
        }
    }
}
