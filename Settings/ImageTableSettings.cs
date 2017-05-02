using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoSplit.Settings
{
    static class ImageTableSettings
    {
        public static int Rows { get; private set; }
        public static int Columns { get; private set; }
        public static int Height { get; set; }
        public static int Width { get; set; }
        public static bool Stretch { get; set; }
        public static int CameraID { get; set; }
        public static string OverlayImagePath { get; set; }
        public static Capture Capture { get; set; }

        public static void Init(int r, int c, int h, int w, bool stretch, int cameraid, string overlayImagePath) 
        {
            Rows = r;
            Columns = c;
            Height = h / r;
            Width = w / c;
            Stretch = stretch;
            CameraID = cameraid;
            if (Capture != null) {
                Capture.Dispose();
            }
            OverlayImagePath = overlayImagePath;
            Capture = new Capture(cameraid);
        }

        public static void Shutdown()
        {
            Capture.Dispose();
        }
    }
}
