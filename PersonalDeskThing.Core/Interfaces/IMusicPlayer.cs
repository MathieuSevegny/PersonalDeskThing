using PersonalDeskThing.Core.Models;

namespace PersonalDeskThing.Core.Interfaces
{
    public interface IMusicPlayer
    {
        public Task<PlayingSong?> GetCurrentSong();
        public Task<Song?> GetNextSong();
        public Task<bool> Play();
        public Task<bool> Pause();
        public Task<bool> NextSong();
        public Task<bool> PreviousSong();
#pragma warning disable CA1003 // Use generic event handler instances
        public event EventHandler? OnSongUpdated;
#pragma warning restore CA1003 // Use generic event handler instances
    }
}
