using System;
using System.Drawing;
using System.Windows.Forms;

namespace SMM2VIEWER
{
    public partial class Form2
    {
        public Form2()
        {
            InitializeComponent();
            _P.Name = "P";
        }

        private int MX, MY;
        private bool isMove;
        private int CX, CY, CX0, CY0;

        private void P_MouseDown(object sender, MouseEventArgs e)
        {
            MX = e.X;
            MY = e.Y;
            isMove = true;
        }

        private void P_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
        }

        private void P_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                P.Left += e.X - MX;
                P.Top += e.Y - MY;
            }

            CX = 1 + e.X / 16;
            CY = 1 + e.Y / 16;
            if (CX != CX0 || CY != CY0)
            {
                ToolTip1.Hide(P);
                ObjC = ModuleSMM.ObjLocData[0, CX, ModuleSMM.MapHeight[0] + 1 - CY];
                TTipImg = (Bitmap)ModuleSMM.GetItemImg(ObjC, ref TTW, ref TTH);
                ToolTip1.Show(" ", P);
                CX0 = CX;
                CY0 = CY;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ToolTip1.OwnerDraw = true;
            ToolTip1.AutomaticDelay = 0;
            ToolTip1.AutoPopDelay = 0;
            // ToolTip1.SetToolTip(P, "")
        }

        private void ToolTip1_Popup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = new Size(TTW, TTH);
        }

        private void ToolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.Graphics.Clear(SystemColors.Info);
            e.Graphics.DrawImage(TTipImg, new Point(0, 0));
            TextRenderer.DrawText(e.Graphics, e.ToolTipText, e.Font, new Rectangle(64, 8, e.Bounds.Width - 72, e.Bounds.Height - 16), SystemColors.InfoText, Color.Empty, TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter);
        }

        private Bitmap TTipImg;
        private int TTW = 16;
        private int TTH = 16;
        private ModuleSMM.ObjStr ObjC;
    }
}