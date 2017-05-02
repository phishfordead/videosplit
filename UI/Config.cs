using DirectShowLib;
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
using Telerik.WinControls.UI;
using VideoSplit.Settings;

namespace VideoSplit.UI
{
    public partial class Config : Form
    {
        public event EventHandler SettingsUpdated;

        Main parent;

        public Config(Main parent)
        {
            InitializeComponent();
            this.parent = parent;
            tbRows.Text = ImageTableSettings.Rows.ToString();
            tbColumns.Text = ImageTableSettings.Columns.ToString();
            cbStretch.Checked = ImageTableSettings.Stretch;
            FillCameraCombo();
            FillOverlayCombo();
        }

        private void FillOverlayCombo() {
            rddlOverlay.Items.Add(new RadListDataItem("None","None"){Selected=ImageTableSettings.OverlayImagePath.Equals("None")});
            foreach (string f in Directory.GetFiles("Overlays", "*.png", SearchOption.TopDirectoryOnly))
            {
                rddlOverlay.Items.Add(new RadListDataItem(Path.GetFileName(f), f) { Selected = ImageTableSettings.OverlayImagePath.Equals(f) });
            }
        }

        private void FillCameraCombo() {
            DsDevice[] systemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            for (int i = 0; i < systemCamereas.Length; i++)
            {
                rddlCamera.Items.Add(systemCamereas[i].Name);
            }
            rddlCamera.SelectedIndex = ImageTableSettings.CameraID;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string olay = rddlOverlay.SelectedIndex == 0 ? "None" : rddlOverlay.Items[rddlOverlay.SelectedIndex].Value.ToString();
            ImageTableSettings.Init(int.Parse(tbRows.Text),
                                    int.Parse(tbColumns.Text),
                                    parent.Height,
                                    parent.Width, cbStretch.Checked, rddlCamera.SelectedIndex, olay);
            if (SettingsUpdated != null) {
                SettingsUpdated(this, null);
            }
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            string olay = rddlOverlay.SelectedIndex == 0 ? "None" : rddlOverlay.Items[rddlOverlay.SelectedIndex].Value.ToString();
            ImageTableSettings.Init(int.Parse(tbRows.Text),
                                    int.Parse(tbColumns.Text),
                                    parent.Height,
                                    parent.Width, cbStretch.Checked,rddlCamera.SelectedIndex,olay);
            if (SettingsUpdated != null) {
                SettingsUpdated(this, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            cbStretch.Checked = !cbStretch.Checked;
        }
    }
}
