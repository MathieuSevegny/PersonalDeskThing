using PersonalDeskThing.App.Client.Models;
using SpotifyAPI.Web;

namespace PersonalDeskThing.App.Extensions
{
    static class SpotifyExtensions
    {
        public static Artist Convert(this SimpleArtist artist)
        {
            return new Artist
            {
                Name = artist.Name,
                Id = artist.Id,
            };
        }
        public static Album Convert(this SimpleAlbum album)
        {
            return new Album
            {
                Name = album.Name,
                Artists = album.Artists.ConvertAll(a => a.Convert()),
                ImageUrl = album.Images[0].Url,
                Id = album.Id,
            };
        }
    }
}
