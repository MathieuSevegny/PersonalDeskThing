using PersonalDeskThing.Core.Interfaces;
using PersonalDeskThing.Core.Models;
using SpotifyAPI.Web;
using PersonalDeskThing.Core.Builders;
using PersonalDeskThing.App.Core.Extensions;

namespace PersonalDeskThing.App.Core.Spotify
{
    class SpotifyService : IMusicPlayer
    {
        SpotifyClient _client;

        public event EventHandler? OnSongUpdated;

        public SpotifyService(SpotifyClient client)
        {
            _client = client;
        }

        public async Task<bool> GetIsStillConnected()
        {
            PrivateUser? privateUser = null;
            try
            {
                privateUser = await _client.UserProfile.Current();
            }
            catch(APIUnauthorizedException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            return privateUser != null;
        }

        public async Task<PlayingSong?> GetCurrentSong()
        {
            var playback = await _client.Player.GetCurrentPlayback();
            var builder = new SongBuilder();

            if (playback.Item is FullTrack ft)
            {
                return builder.WithName(ft.Name)
                    .WithArtists(ft.Artists.ConvertAll(a => a.Convert()))
                    .WithAlbum(ft.Album.Convert())
                    .WithDuration(TimeSpan.FromMilliseconds(ft.DurationMs))
                    .WithProgress(TimeSpan.FromMilliseconds(playback.ProgressMs))
                    .WithIsPlaying(playback.IsPlaying)
                    .BuildPlaying();
            }
            return null;
        }

        public async Task<bool> NextSong()
        {
            await _client.Player.SkipNext().ConfigureAwait(true);
            await sendSongUpdated().ConfigureAwait(true);
            return true;
        }

        public async Task<bool> Pause()
        {
            await _client.Player.PausePlayback().ConfigureAwait(true);
            await sendSongUpdated().ConfigureAwait(true);
            return true;
        }

        public async Task<bool> Play()
        {
            await _client.Player.ResumePlayback().ConfigureAwait(true);
            await sendSongUpdated().ConfigureAwait(true);
            return true;
        }

        public async Task<bool> PreviousSong()
        {
            await _client.Player.SkipPrevious().ConfigureAwait(true);
            await sendSongUpdated().ConfigureAwait(true);
            return true;
        }

        public Task<Song?> GetNextSong()
        {
            throw new NotImplementedException();
        }
        async Task sendSongUpdated()
        {
            await Task.Delay(100).ConfigureAwait(true);
            OnSongUpdated?.Invoke(this,null!);
        }
    }
}
