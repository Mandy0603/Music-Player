using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            musicPlayer.settings.autoStart = false;
            label1.Image = Image.FromFile(@"E:\Java_Notes\素材\icons\unmute.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            
        }

       
        private void btnPlayOrPause_Click(object sender, EventArgs e)
        {
            if (btnPlayOrPause.Text == "Play")
            {

                btnPlayOrPause.Text = "Pause";
                musicPlayer.Ctlcontrols.play();
            }
            else if (btnPlayOrPause.Text == "Pause")
            {
                btnPlayOrPause.Text = "Play";
                musicPlayer.Ctlcontrols.pause();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            musicPlayer.Ctlcontrols.stop();
            btnPlayOrPause.Text = "Play";

        }
        List<string> listPath = new List<string>();

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose File";
            ofd.InitialDirectory = @"E:\Java_Notes\素材\Music";
            ofd.Filter = " |*.wav| |*.mp3| |*.*";
            ofd.Multiselect = true;
            ofd.ShowDialog();

            string[] path = ofd.FileNames;
            for (int i = 0; i < path.Length; i++)
            {
                listPath.Add(path[i]);
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(path[i]));
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Please open a file");
                return;
            }
            
                musicPlayer.URL = listPath[listBox1.SelectedIndex];
                musicPlayer.Ctlcontrols.play();
                btnPlayOrPause.Text = "Pause";
                ExistLrc(listPath[listBox1.SelectedIndex]);
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            listBox1.SelectedIndices.Clear();
            index--;

            if (index == -1)
            {
                index = listBox1.Items.Count - 1;
            }

            listBox1.SelectedIndex = index;
            musicPlayer.URL = listPath[listBox1.SelectedIndex];
            musicPlayer.Ctlcontrols.play();
            btnPlayOrPause.Text = "Pause";
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            listBox1.SelectedIndices.Clear();
            index++;

            if (index == listBox1.Items.Count)
            {
                index = 0;
            }

            listBox1.SelectedIndex = index;
            musicPlayer.URL = listPath[listBox1.SelectedIndex];
            musicPlayer.Ctlcontrols.play();
            btnPlayOrPause.Text = "Pause";

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = listBox1.SelectedItems.Count;
            for (int i = 0; i < count; i++)
            {
                listPath.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (label1.Tag.ToString() == "1")
            {
                musicPlayer.settings.mute = true;
                label1.Tag = 2;
                label1.Image = Image.FromFile(@"E:\Java_Notes\素材\icons\mute.jpg");
            }
            else if (label1.Tag.ToString() == "2")
            {
                musicPlayer.settings.mute = false;
                label1.Tag = 1;
                label1.Image = Image.FromFile(@"E:\Java_Notes\素材\icons\unmute.jpg");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                lblInfo.Text = musicPlayer.Ctlcontrols.currentPositionString + " \\ " + musicPlayer.currentMedia.durationString;

                double d1 = double.Parse(musicPlayer.currentMedia.duration.ToString());
                double d2 = double.Parse(musicPlayer.Ctlcontrols.currentPosition.ToString());

                if (d2 + 1 > d1)
                {
                    int index = listBox1.SelectedIndex;
                    listBox1.SelectedIndices.Clear();
                    index++;

                    if (index == listBox1.Items.Count)
                    {
                        index = 0;
                    }

                    listBox1.SelectedIndex = index;
                    musicPlayer.URL = listPath[listBox1.SelectedIndex];
                    musicPlayer.Ctlcontrols.play();
                    btnPlayOrPause.Text = "Pause";
                }
            }

        }

        //private void musicPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        
            void ExistLrc(string songPath)
            {
                songPath += ".lrc";
                if (File.Exists(songPath))
                {
                    string[] lrcText=File.ReadAllLines(songPath,Encoding.Default);
                    FormatLrc(lrcText);
                }
                else
                {
                    label2.Text = "Lyrics file does not exist";
                }
            }
        
        List<double> listTime = new List<double>();
        List<string> listLrcText = new List<string>();

        void FormatLrc(string[] lrcText)
        {
            for (int i = 0; i < lrcText.Length; i++)
            {
                string[] lrcTemp = lrcText[i].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                string[] lrcNewTemp = lrcTemp[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                double time = double.Parse(lrcNewTemp[0]) * 60 + double.Parse(lrcNewTemp[1]);
                listTime.Add(time);
                listLrcText.Add(lrcTemp[1]);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < listTime.Count; i++)
            {
                if(musicPlayer.Ctlcontrols.currentPosition>=listTime[i]&& musicPlayer.Ctlcontrols.currentPosition < listTime[i + 1])
                {
                    label2.Text=listLrcText[i];
                }
            }
        }
        string[] path = Directory.GetFiles(@"E:\Java_Notes\素材\古装手绘美女图片③冬季");
        Random r = new Random();
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if(musicPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                pictureBox1.Image = Image.FromFile(path[r.Next(0, path.Length - 1)]);
            }
            else
            {
                pictureBox1.Image = Image.FromFile(@"E:\Java_Notes\素材\背景\16pic_3512975_b.jpg");
            }
            
        }
    }
}
