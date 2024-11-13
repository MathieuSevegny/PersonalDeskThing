using PersonalDeskThing.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace PersonalDeskThing.Core.Builders
{
    public class SongBuilder
    {
        string _id { get; set; } = string.Empty;
        /// <summary>
        /// Name of the song
        /// </summary>
        string _name { get; set; } = string.Empty;
        /// <summary>
        /// Name of the artist
        /// </summary>
#pragma warning disable CA1002 // Do not expose generic lists
        List<Artist> _artists { get; } = [];
#pragma warning restore CA1002 // Do not expose generic lists
        /// <summary>
        /// Album name
        /// </summary>
        Album _album { get; set; } = new Album();
        /// <summary>
        /// Duration of the song
        /// </summary>
        TimeSpan _duration { get; set; }
        TimeSpan? _progress { get; set; }
        bool? _isPlaying { get; set; }
        public SongBuilder()
        {
        }
        public SongBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        public SongBuilder WithArtists(IEnumerable<Artist> artist)
        {
            _artists.AddRange(artist);
            return this;
        }
        public SongBuilder WithAlbum(Album album)
        {
            _album = album;
            return this;
        }
        public SongBuilder WithDuration(TimeSpan duration)
        {
            _duration = duration;
            return this;
        }
        public SongBuilder WithProgress(TimeSpan progress)
        {
            _progress = progress;
            return this;
        }
        public SongBuilder WithIsPlaying(bool isPlaying)
        {
            _isPlaying = isPlaying;
            return this;
        }
        Uri getImageUrl()
        {
            if (_album.ImageUrl != null)
            {
                return new Uri(_album.ImageUrl);
            }
            return new Uri("https://via.placeholder.com/150");
        }

        public PlayingSong BuildPlaying()
        {
            if (_progress == null)
            {
                throw new InvalidOperationException("Progress is required");
            }
            if (_isPlaying == null)
            {
                throw new InvalidOperationException("IsPlaying is required");
            }
            return new PlayingSong
            {
                Name = _name,
                Artists = _artists,
                Album = _album,
                Duration = _duration,
                ImageUrl = getImageUrl(),
                Progress = (TimeSpan)_progress,
                IsPlaying = (bool)_isPlaying
            };
        }
        public Song Build()
        {
            return new Song
            {
                Name = _name,
                Artists = _artists,
                Album = _album,
                Duration = _duration,
                ImageUrl = getImageUrl()
            };
        }
    }
}
