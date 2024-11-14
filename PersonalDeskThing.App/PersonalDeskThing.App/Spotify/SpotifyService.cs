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

        public async Task<PlaybackState?> GetCurrentPlaybackState()
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
            PlaybackState playbackState = new PlaybackState()
            {
                RepeatState = new RepeatState(playback.RepeatState),
                ShuffleState = playback.ShuffleState
            };

            if (playback.Item is FullTrack ft)
            {
                var song = builder.WithName(ft.Name)
                    .WithArtists(ft.Artists.ConvertAll(a => a.Convert()))
                    .WithAlbum(ft.Album.Convert())
                    .WithDuration(TimeSpan.FromMilliseconds(ft.DurationMs))
                    .WithProgress(TimeSpan.FromMilliseconds(playback.ProgressMs))
                    .WithIsPlaying(playback.IsPlaying)
                    .BuildPlaying();
                playbackState.CurrentSong = song;
                return playbackState;
            }
            return null;
        }
        public async Task<bool> SetShuffle(bool shuffle)
        {
            PlayerShuffleRequest request = new PlayerShuffleRequest(shuffle);
            try
            {
                await _client.Player.SetShuffle(request).ConfigureAwait(true);
                await sendSongUpdated().ConfigureAwait(true);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
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

        public async Task<bool> SetRepeat(RepeatState repeatState)
        {
            PlayerSetRepeatRequest.State state = repeatState.Mode switch
            {
                RepeatState.RepeatMode.Off => PlayerSetRepeatRequest.State.Off,
                RepeatState.RepeatMode.Context => PlayerSetRepeatRequest.State.Context,
                RepeatState.RepeatMode.Track => PlayerSetRepeatRequest.State.Track,
                _ => throw new NotImplementedException()
            };

            PlayerSetRepeatRequest request = new(state);
            try
            {
                await _client.Player.SetRepeat(request).ConfigureAwait(true);
                await sendSongUpdated().ConfigureAwait(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }
}
