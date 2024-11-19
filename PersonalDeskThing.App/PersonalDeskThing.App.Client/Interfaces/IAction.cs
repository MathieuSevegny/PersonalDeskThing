namespace PersonalDeskThing.App.Client.Interfaces
{
    public interface IAction
    {
        public Uri Image { get; set; }
        public Task Launch();
    }
}
