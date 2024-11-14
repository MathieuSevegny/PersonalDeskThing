using PersonalDeskThing.App.Client.Models;

namespace PersonalDeskThing.App.Client.Interfaces
{
    public interface IMusicPlayer
    {
        public Task<PlaybackState?> GetCurrentPlaybackState();
        public Task<Song?> GetNextSong();
        public Task<bool> Play();
        public Task<bool> Pause();
        public Task<bool> NextSong();
        public Task<bool> PreviousSong();
        public Task<bool> SetShuffle(bool shuffle);
        public Task<bool> SetRepeat(RepeatState repeatState);
#pragma warning disable CA1003 // Use generic event handler instances
        public event EventHandler? OnSongUpdated;
#pragma warning restore CA1003 // Use generic event handler instances
    }
}
