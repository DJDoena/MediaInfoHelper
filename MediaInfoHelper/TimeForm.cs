namespace DoenaSoft.MediaInfoHelper
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    internal sealed partial class TimeForm : Form
    {
        internal uint Time { get; private set; }

        public TimeForm(string fileName)
        {
            this.InitializeComponent();

            this.Text = (new FileInfo(fileName)).Name;
        }

        private void OnAcceptButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnSecondsTextBoxTextChanged(object sender, EventArgs e)
        {
            try
            {
                this.TrySetTime();
            }
            catch
            {
                TimeTextBox.Text = string.Empty;

                this.Time = 0;
            }
        }

        private void TrySetTime()
        {
            var text = SecondsTextBox.Text;

            TimeTextBox.Text = text.Contains(":") ? GetSeconds(text) : Helper.FormatTime(uint.Parse(text));

            uint.TryParse(TimeTextBox.Text, out uint time);

            this.Time = time;
        }

        private static string GetSeconds(string text)
        {
            GetTimeParts(text, out int hours, out int minutes, out int seconds);

            var totalSeconds = (hours * 3600) + (minutes * 60) + seconds;

            return totalSeconds.ToString();
        }

        private static void GetTimeParts(string text, out int hours, out int minutes, out int seconds)
        {
            var split = text.Split(':');

            if (split.Length == 3)
            {
                hours = int.Parse(split[0]);
                minutes = int.Parse(split[1]);
                seconds = int.Parse(split[2]);
            }
            else
            {
                hours = 0;
                minutes = int.Parse(split[0]);
                seconds = int.Parse(split[1]);
            }
        }
    }
}