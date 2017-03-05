using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace AviRecorder.Forms
{
    partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            var asm = Assembly.GetExecutingAssembly();

            _titleLabel.Text = asm.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            _versionLabel.Text = asm.GetName().Version.ToString();
            _copyrightLabel.Text = asm.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            
            _thanksToLinkLabel.Links.Clear();
            _thanksToLinkLabel.Links.Add(32, 23, "https://www.magicyuv.com/");
            _thanksToLinkLabel.Links.Add(133, 12, "http://web.archive.org/web/20160826144709/http://www.4p8.com/eric.brasseur/gamma.html");
            _thanksToLinkLabel.Links.Add(213, 20, "http://www.famfamfam.com/lab/icons/silk/");
            _thanksToLinkLabel.Links.Add(247, 41, "https://github.com/AronParker/AviRecorder");
        }

#if DEBUG
        private string FindLinkLocation(string link)
        {
            var pos = _thanksToLinkLabel.Text.IndexOf(link);
            var count = link.Length;
            return $"{pos}, {count}";
        }
#endif

        private void ThanksToLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start((string)e.Link.LinkData);
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("Unable to open link: " + ex.Message, "Unable to open link", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}