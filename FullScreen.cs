using System.Drawing;
using System.Windows.Forms;

namespace VideoSplit
{
    class FullScreen
    {
        private static Point origloc;
        private static Size origSize;
        public static void EnterFullScreenMode(Form targetForm)
        {
            targetForm.WindowState = FormWindowState.Normal;
            targetForm.FormBorderStyle = FormBorderStyle.None;
            //targetForm.WindowState = FormWindowState.Maximized;
            Screen s = Screen.FromControl(targetForm);
            origloc = targetForm.DesktopLocation;
            origSize = targetForm.Size;
            targetForm.DesktopLocation = new System.Drawing.Point(0, 0);
            targetForm.Size = new System.Drawing.Size(s.Bounds.Width,s.Bounds.Height);
            //targetForm.Size = new System.Drawing.Size(s.Bounds.Width + 100,s.Bounds.Height + 100);
        }

        public static void LeaveFullScreenMode(Form targetForm)
        {
            targetForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            targetForm.WindowState = FormWindowState.Normal;
            targetForm.DesktopLocation = origloc;
            targetForm.Size = origSize;
        }
    }
}