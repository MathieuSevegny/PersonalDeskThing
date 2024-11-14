using PersonalDeskThing.App.Client.Interfaces;
using PersonalDeskThing.App.Client.Models;
using SpotifyAPI.Web;
using PersonalDeskThing.App.Client.Builders;
using PersonalDeskThing.App.Extensions;

namespace PersonalDeskThing.App.Spotify
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
            catch (APIUnauthorizedException)
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
            CurrentlyPlayingContext? playback;
            try
            {
                playback = await _client.Player.GetCurrentPlayback();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
            var builder = new SongBuilder();

            if (playback == null)
            {
                return null;
            }

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
            try
            {
                await _client.Player.SkipNext().ConfigureAwait(true);
                await sendSongUpdated().ConfigureAwait(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
            return true;
        }

        public async Task<bool> Pause()
        {
            try
            {
                await _client.Player.PausePlayback().ConfigureAwait(true);
                await sendSongUpdated().ConfigureAwait(true);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
            return true;
        }

        public async Task<bool> Play()
        {
            try
            {
                await _client.Player.ResumePlayback().ConfigureAwait(true);
                await sendSongUpdated().ConfigureAwait(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
            return true;
        }

        public async Task<bool> PreviousSong()
        {
            try
            {
                await _client.Player.SkipPrevious().ConfigureAwait(true);
                await sendSongUpdated().ConfigureAwait(true);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
            return true;
        }

        public async Task<Song?> GetNextSong()
        {
            QueueResponse? queue;
            try
            {
                queue = await _client.Player.GetQueue().ConfigureAwait(true);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            if (queue == null || queue.Queue.Count == 0)
            {
                return null;
            }
            var builder = new SongBuilder();

            if (queue.Queue[0] is not FullTrack ft)
            {
                return null;
            }
            return builder.WithName(ft.Name)
                    .WithArtists(ft.Artists.ConvertAll(a => a.Convert()))
                    .WithAlbum(ft.Album.Convert())
                    .WithDuration(TimeSpan.FromMilliseconds(ft.DurationMs))
                    .Build();
        }
        async Task sendSongUpdated()
        {
            await Task.Delay(100).ConfigureAwait(true);
            OnSongUpdated?.Invoke(this, null!);
        }
    }
}
