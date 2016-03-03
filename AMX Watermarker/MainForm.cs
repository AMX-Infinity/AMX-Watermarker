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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMX_Watermarker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.Resize += MainForm_Resize;

            pbWatermark = new AMXPictureBox();
            pbWatermark.SizeMode = PictureBoxSizeMode.StretchImage;
            pbWatermark.Cursor = Cursors.SizeAll;
            pbWatermark.BackColor = Color.FromArgb(0, 0, 0, 0);
            
            pbWatermark.MouseDown += pbWatermark_MouseDown;
            pbWatermark.MouseUp += pbWatermark_MouseUp;
            pbWatermark.MouseMove += pbWatermark_MouseMove;
        }

        void MainForm_Resize(object sender, EventArgs e)
        {
            pbImg.Refresh();
        }

        bool isHolding = false;

        int pEx = 0, pEy = 0, cEx = 0, cEy = 0;

        void pbWatermark_MouseMove(object sender, MouseEventArgs e)
        {
            if (isHolding)
            {
                if (pEx == 0)
                    pEx = e.X;
                if (pEy == 0)
                    pEy = e.Y;

                cEx = e.X;
                cEy = e.Y;

                int delEx = cEx - pEx;
                int delEy = cEy - pEy;

                pbWatermark.Left += delEx;
                pbWatermark.Top += delEy;
            }
        }
        
        void pbWatermark_MouseUp(object sender, MouseEventArgs e)
        {
            isHolding = false;
            markX = pbWatermark.Left + pbImg.Left;
            markY = pbWatermark.Top + pbImg.Top;
        }

        void pbWatermark_MouseDown(object sender, MouseEventArgs e)
        {
            isHolding = true;
        }
        
        string imageFile;
        string watermarkFile;

        Image image;
        Image watermark;

        AMXPictureBox pbWatermark;

        int markX, markY;

        private void selectImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Image Files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                status.Text = "Loading image...";
                imageFile = ofd.FileName;
            }
            else
            {
                return;
            }

            image = Image.FromFile(imageFile);

            float currWidth = image.Width;
            float currHeight = image.Height;
            while (currHeight > 800 && currWidth > 800)
            {
                currHeight /= 1.1f;
                currWidth /= 1.1f;
            }
            
            this.Width = (int)(currWidth);
            this.Height = (int)(currHeight);
            
            pbImg.Image = image;

            status.Text = "Image loaded.";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "AMX Watermarker", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void selectWatermarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Image Files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                status.Text = "Loading watermark image...";
                watermarkFile = ofd.FileName;
            }
            else
            {
                return;
            }

            watermark = Image.FromFile(watermarkFile);

            float currWidth = watermark.Width;
            float currHeight = watermark.Height;
            while (currHeight > 300 && currWidth > 300)
            {
                currHeight /= 1.1f;
                currWidth /= 1.1f;
            }

            pbWatermark.Height = (int)currHeight;
            pbWatermark.Width = (int)currWidth;

            pbWatermark.SetBounds((pbImg.Width - pbWatermark.Width) / 2, (pbImg.Height - pbWatermark.Height) / 2, pbWatermark.Width, pbWatermark.Height);
                        
            pbImg.Controls.Add(pbWatermark);

            pbWatermark.BringToFront();

            pbWatermark.BorderStyle = BorderStyle.FixedSingle;

            pbWatermark.Image = watermark;

            status.Text = "Watermark image loaded.";
        }

        private void saveWatermarkedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image mainImage = (Image)pbImg.Image.Clone();
            Image wmImage = (Image)pbWatermark.Image.Clone();

            using (Bitmap img = (Bitmap)mainImage)
            using (Bitmap watermarkImage = (Bitmap)wmImage)
            using (Graphics imageGraphics = Graphics.FromImage(img))
            {
                status.Text = "Watermarking...";
                img.SetResolution(Graphics.FromImage(wmImage).DpiX, Graphics.FromImage(wmImage).DpiY);
                watermarkImage.SetResolution(imageGraphics.DpiX, imageGraphics.DpiY);

                int x = markX;//((image.Width - watermarkImage.Width) / 2);
                int y = markY;//((image.Height - watermarkImage.Height) / 2);

                //imageGraphics.DrawImage(watermarkImage, x, y, watermarkImage.Width, watermarkImage.Height);
                imageGraphics.DrawImage(watermarkImage, x, y, pbWatermark.Width, pbWatermark.Height);

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Image Files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    img.Save(sfd.FileName);
                    status.Text = "Watermarked image saved.";
                    MessageBox.Show("Watermarked image saved!", "AMX Watermarker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" A watermarking tool written in C#. \n\n (c) Afaan Bilal \n www.coderevolution.tk", "AMX Watermarker", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
