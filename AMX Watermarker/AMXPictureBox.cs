/*
 *
 * AMX Watermarker
 * A watermarking tool written in C#
 * 
 * (c) Afaan Bilal
 * https://www.coderevolution.tk
 * https://www.amxinfinity.tk
 * https://google.com/+AfaanBilal
 * 
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMX_Watermarker
{
    public class AMXPictureBox : PictureBox
    {
        public AMXPictureBox()
        {
            this.ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var rc = new Rectangle(this.ClientSize.Width - grab, this.ClientSize.Height - grab, grab, grab);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
        }
        /*
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                var pos = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                if (pos.X >= this.ClientSize.Width - grab && pos.Y >= this.ClientSize.Height - grab)
                    m.Result = new IntPtr(17);  // HT_BOTTOMRIGHT
            }
        }
        */
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.Style |= 0x840000;  // Turn on WS_BORDER + WS_THICKFRAME //0x00040000; 
                return cp;
            }
        }

        private const int grab = 16;
    }
}
