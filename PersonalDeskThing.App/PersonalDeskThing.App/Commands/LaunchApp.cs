using PersonalDeskThing.App.Client.Interfaces;
using System.Diagnostics;

namespace PersonalDeskThing.App.Commands
{
    public class LaunchApp : IAction
    {
        /// <summary>
        /// Path to the executable
        /// </summary>
        public string ExecutablePath { get; set; }
        /// <summary>
        /// Image to display
        /// </summary>
        public Uri Image { get; set; }
        /// <summary>
        /// Launch the application
        /// </summary>
        /// <returns></returns>

        public Task Launch()
        {
            if (string.IsNullOrEmpty(ExecutablePath))
            {
                throw new InvalidOperationException("Executable path is not set");
            }
            if (!File.Exists(ExecutablePath))
            {
                throw new InvalidOperationException("Executable path does not exist");
            }
            try
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(ExecutablePath)
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }
        public LaunchApp(string executablePath, Uri image)
        {
            ExecutablePath = executablePath;
            Image = image;
        }
    }
}
