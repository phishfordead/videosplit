using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoSplit.Settings;

namespace VideoSplit.Factories
{
    class ImageTableFactory
    {
        public static PanelDoubleBuffered[] Factory(Action<object,PaintEventArgs> paintevent) {
            int num = ImageTableSettings.Rows * ImageTableSettings.Columns,                 
                x = 0, y = 0, h =ImageTableSettings.Height, w=ImageTableSettings.Width;
            PanelDoubleBuffered[] panels = new PanelDoubleBuffered[num];

            for (int i = 0; i < num; i++) {
                if (i % ImageTableSettings.Columns == 0 && i > 0) {
                    y += h;
                    x = 0;
                }
                PanelDoubleBuffered p = new PanelDoubleBuffered();
                p.Height = ImageTableSettings.Height;
                p.Width = ImageTableSettings.Width;
                p.Paint += new System.Windows.Forms.PaintEventHandler(paintevent);
                p.Location = new Point(x, y);
                panels[i] = p;
                x += w;
            }

            return panels;
        }
    }
}
