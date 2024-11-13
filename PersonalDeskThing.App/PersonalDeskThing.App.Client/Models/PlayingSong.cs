namespace PersonalDeskThing.App.Client.Models
{
    public class PlayingSong : Song
    {
        TimeSpan _progress;
        public PlayingSong() : base()
        {
        }

        public TimeSpan Progress 
        { 
            get => _progress;
            set
            {
                _progress = value;
                ProgressChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public bool IsPlaying { get; set; }
        public event EventHandler? ProgressChanged;
    }
}
