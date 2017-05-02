using DirectShowLib;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoSplit.Factories;
using VideoSplit.Imaging;
using VideoSplit.Settings;

namespace VideoSplit.UI
{
    public partial class Main : Form
    {
        bool infullscreen = false;
        //Image background;
        //Image overlay;
        //Image overlayorig;
        Timer t = new Timer();
        

        public Main()
        {
            Properties.Settings.Default.Reset();
            InitializeComponent();
            ImageTableSettings.Init(Properties.Settings.Default.Rows,
                                    Properties.Settings.Default.Columns,
                                    this.Height, this.Width, Properties.Settings.Default.StretchImage, 
                                    Properties.Settings.Default.CameraID, 
                                    Properties.Settings.Default.OverlayImage);

            t.Tick += ImagePanelController.Update;
            t.Interval = 1000/60;
            this.ResizeEnd += Main_ResizeEnd;//ScaleImage(overlayorig, ImageTableSettings.Width, ImageTableSettings.Height, true);
            CreatePanels();
            this.FormClosed += Main_FormClosed;
        }

        void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            ImagePanelController.Dispose();
        }

        void Main_ResizeEnd(object sender, EventArgs e)
        {
            ImageTableSettings.Init(Properties.Settings.Default.Rows,
                                    Properties.Settings.Default.Columns,
                                    this.Height, this.Width, false, ImageTableSettings.CameraID, ImageTableSettings.OverlayImagePath);
            
            CreatePanels();
        }

        private void CreatePanels() {
            ImagePanelController.CreatePanels();
            this.Controls.AddRange(ImagePanelController.Panels);
            t.Start();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                DoFullscreen();
            }
        }

        private void DoFullscreen()
        {
            if (!infullscreen)
            {
                FullScreen.EnterFullScreenMode(this);
                menuStrip1.Visible = false;
                Cursor.Hide();
            }
            else
            {
                FullScreen.LeaveFullScreenMode(this);
                menuStrip1.Visible = true;
                Cursor.Show();
            }
            ImageTableSettings.Init(Properties.Settings.Default.Rows,
                                    Properties.Settings.Default.Columns,
                                    this.Height, this.Width, ImageTableSettings.Stretch,ImageTableSettings.CameraID, ImageTableSettings.OverlayImagePath);
            CreatePanels();
            infullscreen = !infullscreen;
        }

        

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoFullscreen();
        }

        private void mnuConfig_Click(object sender, EventArgs e)
        {
            Config c = new Config(this);
            c.SettingsUpdated += c_SettingsUpdated;
            c.Show();
        }

        void c_SettingsUpdated(object sender, EventArgs e)
        {
            Properties.Settings.Default.Columns = ImageTableSettings.Columns;
            Properties.Settings.Default.Rows = ImageTableSettings.Rows;
            Properties.Settings.Default.Save();
            ImagePanelController.DisposeOriginalOverlay();
            if (!ImageTableSettings.OverlayImagePath.Equals("None"))
            {
                ImagePanelController.OriginalOverlay = Image.FromFile(ImageTableSettings.OverlayImagePath);
            }
            CreatePanels();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }                    
    }
}
