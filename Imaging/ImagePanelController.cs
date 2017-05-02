using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoSplit.Factories;
using VideoSplit.Settings;

namespace VideoSplit.Imaging
{
    class ImagePanelController
    {
        public static Image Background { get; set; }
        public static Image Overlay { get; set; }
        public static Image OriginalOverlay { get; set; }
        public static PanelDoubleBuffered[] Panels {get;set;}

        public static void Init() {
            OriginalOverlay = null;
            Background = ImageTableSettings.Capture.QueryFrame().Bitmap;
            Overlay = null;
        }

        public static void Paint_PanelHelper(object sender, PaintEventArgs e) {
            e.Graphics.Clear(Color.Black);
            if (Background != null)
            {
                e.Graphics.DrawImage(Background, 0, 0);
            }
            if (Overlay != null)
            {
                e.Graphics.DrawImage(Overlay, 0, 0);
            }
        }

        public static void CreatePanels() {
            DisposePanels();
            Panels = ImageTableFactory.Factory(Paint_PanelHelper);
        }

        public static void Update(object sender, EventArgs e)
        {
            if (Background != null)
                Background.Dispose();
            if (Overlay != null)
                Overlay.Dispose();
            Background = ScaleImage(ImageTableSettings.Capture.QueryFrame().Bitmap, ImageTableSettings.Width, ImageTableSettings.Height, ImageTableSettings.Stretch);
            if (!ImageTableSettings.OverlayImagePath.Equals("None"))
            {
                Overlay = ScaleImage(OriginalOverlay, ImageTableSettings.Width, ImageTableSettings.Height, true);
            }
            Background.RotateFlip(RotateFlipType.RotateNoneFlipX);
            foreach (PanelDoubleBuffered p in Panels)
            {
                p.Invalidate();
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight, bool stretch)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            if (stretch)
            {
                newWidth = maxWidth;
                newHeight = maxHeight;
            }

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static void DisposeOriginalOverlay() {
            if (OriginalOverlay != null)
            {
                OriginalOverlay.Dispose();
                OriginalOverlay = null;
            }
        }

        public static void DisposeOverlay()
        {
            if (Overlay != null)
            {
                Overlay.Dispose();
                Overlay = null;
            }
        }

        public static void DisposeBackground()
        {
            if (Background != null)
            {
                Background.Dispose();
                Background = null;
            }
        }

        private static void DisposePanels() {
            if (Panels != null)
            {
                foreach (PanelDoubleBuffered pdb in Panels)
                    pdb.Dispose();
            }
        }

        public static void Dispose() {
            DisposeOriginalOverlay();
            DisposeBackground();
            DisposeOverlay();
            DisposePanels();
        }
    }
}
