namespace PersonalDeskThing.App.Client.Models
{
    public class RepeatState
    {
        public enum RepeatMode
        {
            Off,
            Context,
            Track
        }
        public RepeatState(string mode)
        {
            Mode = mode switch
            {
                "off" => RepeatMode.Off,
                "context" => RepeatMode.Context,
                "track" => RepeatMode.Track,
                _ => RepeatMode.Off
            };
        }
        public RepeatState(RepeatMode mode)
        {
            Mode = mode;
        }
        public RepeatMode Mode { get; set; }
        public override string ToString()
        {
            return Mode switch
            {
                RepeatMode.Off => "off",
                RepeatMode.Context => "context",
                RepeatMode.Track => "track",
                _ => "off"
            };
        }
    }
}
