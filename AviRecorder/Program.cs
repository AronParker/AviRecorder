using System;
using System.Windows.Forms;
using AviRecorder.Controller;
using AviRecorder.Forms;
using AviRecorder.Steam;

namespace AviRecorder
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();

            Configuration configuration;
            try
            {
                configuration = Configuration.Load();
            }
            catch (SteamException ex)
            {
                MessageBox.Show(ex.Message, "Failed to start AVI Recorder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(configuration));
        }
    }
}
