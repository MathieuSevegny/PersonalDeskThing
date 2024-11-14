namespace PersonalDeskThing.App.Client.Models
{
    public class PlaybackState
    {
        public PlayingSong? CurrentSong { get; set; }
        public bool ShuffleState { get; set; }
        public RepeatState RepeatState { get; set; } = new RepeatState(RepeatState.RepeatMode.Off);
    }
}
