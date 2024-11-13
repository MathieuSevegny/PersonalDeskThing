namespace PersonalDeskThing.App.Client.Models
{
    public class PlayingSong : Song
    {
        public PlayingSong() : base()
        {
        }

        public TimeSpan Progress { get; set; }
        public bool IsPlaying { get; set; }
    }
}
