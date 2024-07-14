using System;
using System.Windows.Forms;

namespace JMWPlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem != null)
            {
                wmp.URL = lstFiles.SelectedItem.ToString();
                wmp.Ctlcontrols.play();
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            wmp.Ctlcontrols.pause();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            wmp.Ctlcontrols.stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
        }

        private void lstFiles_DoubleClick(object sender, EventArgs e)
        {
            btnPlay_Click(sender, e);
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (string file in openFileDialog1.FileNames)
            {
                lstFiles.Items.Add(file);
            }
        }

        private void btnOpenFiles_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
