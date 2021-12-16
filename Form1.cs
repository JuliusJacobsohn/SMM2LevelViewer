using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;

namespace SMM2VIEWER
{
    public partial class Form1
    {
        public Form1()
        {
            InitializeComponent();
            _Button1.Name = "Button1";
            _Button2.Name = "Button2";
            _Button4.Name = "Button4";
            _Button5.Name = "Button5";
            _Button8.Name = "Button8";
            _TrackBar1.Name = "TrackBar1";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            isMapIO = true;
            RefPic();
            Button2.Enabled = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            isMapIO = false;
            LoadEFile(false);
            InitPng();
            DrawObject(false);
            My.MyProject.Forms.Form2.P.Image = B;
            Button2.Enabled = true;
        }

        public void LoadEFile(bool IO)
        {
            // 关卡文件头H00长度200
            ModuleSMM.LoadLvlData(TextBox1.Text, IO);
            if (IO)
            {
                Label2.Text += Constants.vbCrLf + Constants.vbCrLf;
                Label2.Text += "图名：" + ModuleSMM.LH.Name + Constants.vbCrLf;
                Label2.Text += "描述：" + ModuleSMM.LH.Desc + Constants.vbCrLf;
                Label2.Text += "时间：" + ModuleSMM.LH.Timer + Constants.vbCrLf;
                Label2.Text += "风格：" + ModuleSMM.LH.GameStyle + Constants.vbCrLf;
                Label2.Text += "版本：" + ModuleSMM.LH.GameVer + Constants.vbCrLf;
                Label2.Text += "起点：" + ModuleSMM.LH.StartY + Constants.vbCrLf;
                Label2.Text += "终点：" + ModuleSMM.LH.GoalX + "," + ModuleSMM.LH.GoalY + Constants.vbCrLf;
                Label2.Text += "======表世界======" + Constants.vbCrLf;
                Label2.Text += "主题：" + ModuleSMM.MapHdr.Theme + Constants.vbCrLf;
                Label2.Text += "宽度：" + ModuleSMM.MapHdr.BorR + Constants.vbCrLf;
                Label2.Text += "高度：" + ModuleSMM.MapHdr.BorT + Constants.vbCrLf;
                Label2.Text += "砖块：" + ModuleSMM.MapHdr.GroundCount + Constants.vbCrLf;
                Label2.Text += "单位：" + ModuleSMM.MapHdr.ObjCount + Constants.vbCrLf;
                Label2.Text += "轨道：" + ModuleSMM.MapHdr.TrackCount + Constants.vbCrLf;
                Label2.Text += "卷轴：" + ModuleSMM.MapHdr.AutoscrollType + Constants.vbCrLf;
                Label2.Text += "水面：" + ModuleSMM.MapHdr.LiqSHeight + "-" + ModuleSMM.MapHdr.LiqEHeight + Constants.vbCrLf;
            }
            else
            {
                Label2.Text += "======里世界======" + Constants.vbCrLf;
                Label2.Text += "主题：" + ModuleSMM.MapHdr.Theme + Constants.vbCrLf;
                Label2.Text += "宽度：" + ModuleSMM.MapHdr.BorR + Constants.vbCrLf;
                Label2.Text += "高度：" + ModuleSMM.MapHdr.BorT + Constants.vbCrLf;
                Label2.Text += "砖块：" + ModuleSMM.MapHdr.GroundCount + Constants.vbCrLf;
                Label2.Text += "单位：" + ModuleSMM.MapHdr.ObjCount + Constants.vbCrLf;
                Label2.Text += "轨道：" + ModuleSMM.MapHdr.TrackCount + Constants.vbCrLf;
                Label2.Text += "卷轴：" + ModuleSMM.MapHdr.AutoscrollType + Constants.vbCrLf;
                Label2.Text += "水面：" + ModuleSMM.MapHdr.LiqSHeight + "-" + ModuleSMM.MapHdr.LiqEHeight + Constants.vbCrLf;
            }

            FieldInfo[] LInfo;
            FieldInfo I;
            if (IO)
            {
                LInfo = ModuleSMM.LH.GetType().GetFields();
                TextBox2.Text = "";
                foreach (var currentI in LInfo)
                {
                    I = currentI;
                    TextBox2.Text = Conversions.ToString(TextBox2.Text + Operators.ConcatenateObject(Operators.ConcatenateObject(I.Name + ":", I.GetValue(ModuleSMM.LH)), Constants.vbCrLf));
                }

                TextBox3.Text = "===M0===" + Constants.vbCrLf;
                // 表世界H200长度2DEE0
                LInfo = ModuleSMM.MapHdr.GetType().GetFields();
                foreach (var currentI1 in LInfo)
                {
                    I = currentI1;
                    TextBox3.Text += I.Name + ":" + I.GetValue(ModuleSMM.MapHdr).ToString() + Constants.vbCrLf;
                }
            }
            else
            {
                TextBox4.Text = "===M1===" + Constants.vbCrLf;
                // 表世界H200长度2DEE0
                LInfo = ModuleSMM.MapHdr.GetType().GetFields();
                foreach (var currentI2 in LInfo)
                {
                    I = currentI2;
                    TextBox4.Text += I.Name + ":" + I.GetValue(ModuleSMM.MapHdr).ToString() + Constants.vbCrLf;
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            My.MyProject.Forms.Form2.Show();
        }

        private Bitmap B;
        private Graphics G;
        private int Zm;

        public void InitPng()
        {
            int i;
            int H;
            int W;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            Zm = (int)Math.Round(Math.Pow(2d, TrackBar1.Value));
            B = new Bitmap(W * Zm, H * Zm);
            G = Graphics.FromImage(B);
            My.MyProject.Forms.Form2.P.Width = W * Zm;
            My.MyProject.Forms.Form2.P.Height = H * Zm;
            G.InterpolationMode = InterpolationMode.NearestNeighbor;
            var loopTo = H;
            for (i = 0; i <= loopTo; i++)
            {
                G.DrawLine(Pens.LightGray, 0f, i * Zm, W * Zm, i * Zm);
                if (i % 13 == 0)
                {
                    G.DrawLine(Pens.LightGray, 0f, (H - i) * Zm + 1, W * Zm, (H - i) * Zm + 1);
                }

                if ((H - i) % 10 == 0)
                {
                    G.DrawString((H - i).ToString(), Button1.Font, Brushes.Black, 0f, i * Zm);
                }
            }

            var loopTo1 = W;
            for (i = 0; i <= loopTo1; i++)
            {
                G.DrawLine(Pens.LightGray, i * Zm, 0f, i * Zm, H * Zm);
                if (i % 24 == 0)
                {
                    G.DrawLine(Pens.LightGray, i * Zm + 1, 0f, i * Zm + 1, H * Zm);
                }

                if (i % 10 == 9)
                {
                    G.DrawString((i + 1).ToString(), Button1.Font, Brushes.Black, i * Zm, 0f);
                }
            }

            Color BC1, BC2;
            if (ModuleSMM.MapHdr.Theme == 2)
            {
                BC1 = Color.FromArgb(64, 255, 0, 0);
                BC2 = Color.FromArgb(64, 255, 0, 0);
                G.FillRectangle(new SolidBrush(BC2), 0f, (float)((H - ModuleSMM.MapHdr.LiqEHeight - 0.5d) * Zm), W * Zm, H * Zm);
                G.FillRectangle(new SolidBrush(BC1), 0f, (float)((H - ModuleSMM.MapHdr.LiqSHeight - 0.5d) * Zm), W * Zm, H * Zm);
            }
            else if (ModuleSMM.MapHdr.Theme == 9)
            {
                BC1 = Color.FromArgb(64, 0, 0, 255);
                BC2 = Color.FromArgb(64, 0, 0, 255);
                G.FillRectangle(new SolidBrush(BC2), 0f, (float)((H - ModuleSMM.MapHdr.LiqEHeight - 0.5d) * Zm), W * Zm, H * Zm);
                G.FillRectangle(new SolidBrush(BC1), 0f, (float)((H - ModuleSMM.MapHdr.LiqSHeight - 0.5d) * Zm), W * Zm, H * Zm);
            }
        }

        public void InitPng2()
        {
            // ReDim OBJ(ObjEng.GetUpperBound(0))
            int i;
            // For i = 0 To OBJ.GetUpperBound(0)
            // Try
            // OBJ(i) = New Bitmap(PT & "\img\" & LH.GameStyle.ToString & "\obj\" & i.ToString & ".PNG")
            // Catch
            // End Try
            // Next
            // FLG(0) = New Bitmap(PT & "\img\" & LH.GameStyle.ToString & "\obj\F0.PNG")
            // FLG(1) = New Bitmap(PT & "\img\" & LH.GameStyle.ToString & "\obj\F1.PNG")
            // FLG(2) = New Bitmap(PT & "\img\" & LH.GameStyle.ToString & "\obj\F2.PNG")

            int H;
            int W;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            Zm = (int)Math.Round(Math.Pow(2d, TrackBar1.Value));
            B = new Bitmap(W * Zm, H * Zm);
            G = Graphics.FromImage(B);
            My.MyProject.Forms.Form3.P.Width = W * Zm;
            My.MyProject.Forms.Form3.P.Height = H * Zm;
            G.InterpolationMode = InterpolationMode.NearestNeighbor;
            var loopTo = H;
            for (i = 0; i <= loopTo; i++)
            {
                G.DrawLine(Pens.WhiteSmoke, 0f, i * Zm, W * Zm, i * Zm);
                if (i % 13 == 0)
                {
                    G.DrawLine(Pens.WhiteSmoke, 0f, (H - i) * Zm + 1, W * Zm, (H - i) * Zm + 1);
                }

                if ((H - i) % 10 == 0)
                {
                    G.DrawString((H - i).ToString(), Button1.Font, Brushes.Black, 0f, i * Zm);
                }
            }

            var loopTo1 = W;
            for (i = 0; i <= loopTo1; i++)
            {
                G.DrawLine(Pens.WhiteSmoke, i * Zm, 0f, i * Zm, H * Zm);
                if (i % 24 == 0)
                {
                    G.DrawLine(Pens.WhiteSmoke, i * Zm + 1, 0f, i * Zm + 1, H * Zm);
                }

                if (i % 10 == 9)
                {
                    G.DrawString((i + 1).ToString(), Button1.Font, Brushes.Black, i * Zm, 0f);
                }
            }

            Color BC1, BC2;
            if (ModuleSMM.MapHdr.Theme == 2)
            {
                BC1 = Color.FromArgb(64, 255, 0, 0);
                BC2 = Color.FromArgb(64, 255, 0, 0);
                G.FillRectangle(new SolidBrush(BC2), 0f, (float)((H - ModuleSMM.MapHdr.LiqEHeight - 0.5d) * Zm), W * Zm, H * Zm);
                G.FillRectangle(new SolidBrush(BC1), 0f, (float)((H - ModuleSMM.MapHdr.LiqSHeight - 0.5d) * Zm), W * Zm, H * Zm);
            }
            else if (ModuleSMM.MapHdr.Theme == 9)
            {
                BC1 = Color.FromArgb(64, 0, 0, 255);
                BC2 = Color.FromArgb(64, 0, 0, 255);
                G.FillRectangle(new SolidBrush(BC2), 0f, (float)((H - ModuleSMM.MapHdr.LiqEHeight - 0.5d) * Zm), W * Zm, H * Zm);
                G.FillRectangle(new SolidBrush(BC1), 0f, (float)((H - ModuleSMM.MapHdr.LiqSHeight - 0.5d) * Zm), W * Zm, H * Zm);
            }
        }

        public void DrawMoveBlock(byte ID, byte EX, int X, int Y)
        {
            int H, W, XX, YY;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            XX = (int)Math.Round(X / 160d + 1d);
            YY = (int)Math.Round((Y + 80) / 160d + 1d);
            int i;
            switch (ID)
            {
                case 85:
                    {
                        switch (ModuleSMM.MapTrackBlk[EX - 1].Node[0].p1)
                        {
                            case 1:
                            case 5:
                            case 7:
                            case 14:
                                {
                                    XX -= 4;
                                    break;
                                }

                            case 2:
                            case 9:
                            case 11:
                            case 13:
                                {
                                    break;
                                }

                            case 3:
                            case 6:
                            case 10:
                            case 16:
                                {
                                    XX -= 2;
                                    YY -= 2;
                                    break;
                                }

                            case 4:
                            case 8:
                            case 12:
                            case 15:
                                {
                                    XX -= 2;
                                    YY += 2;
                                    break;
                                }
                        }

                        var loopTo = ModuleSMM.MapTrackBlk[EX - 1].NodeCount - 1;
                        for (i = 0; i <= loopTo; i++)
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SS.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            // G.DrawString(MapTrackBlk(EX - 1).Node(i).p1, Me.Font, Brushes.Black, (XX) * Zm, (H - YY) * Zm)
                            switch (ModuleSMM.MapTrackBlk[EX - 1].Node[i].p1)
                            {
                                case 1: // L
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SL.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX -= 2;
                                        break;
                                    }

                                case 2: // R
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SR.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX += 2;
                                        break;
                                    }

                                case 3: // D
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SD.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY -= 2;
                                        break;
                                    }

                                case 4: // U
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SU.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY += 2;
                                        break;
                                    }

                                case 5: // LD
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SRD.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY -= 2;
                                        break;
                                    }

                                case 6: // DL
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SUL.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX -= 2;
                                        break;
                                    }

                                case 7: // LU
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SRU.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY += 2;
                                        break;
                                    }

                                case 8: // UL
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SDL.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX -= 2;
                                        break;
                                    }

                                case 9: // RD
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SLD.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY -= 2;
                                        break;
                                    }

                                case 10: // DR
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SUR.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX += 2;
                                        break;
                                    }

                                case 11: // RU
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SLU.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY += 2;
                                        break;
                                    }

                                case 12: // UR
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SDR.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX += 2;
                                        break;
                                    }

                                case 13: // RE
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        break;
                                    }

                                case 14: // LE
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        break;
                                    }

                                case 15: // UE
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        break;
                                    }

                                case 16: // DE
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        break;
                                    }
                            }
                        }

                        break;
                    }

                case 119:
                    {
                        switch (ModuleSMM.MapMoveBlk[EX - 1].Node[0].p1)
                        {
                            case 1:
                            case 5:
                            case 7:
                            case 14:
                                {
                                    XX -= 4;
                                    break;
                                }

                            case 2:
                            case 9:
                            case 11:
                            case 13:
                                {
                                    break;
                                }

                            case 3:
                            case 6:
                            case 10:
                            case 16:
                                {
                                    XX -= 2;
                                    YY -= 2;
                                    break;
                                }

                            case 4:
                            case 8:
                            case 12:
                            case 15:
                                {
                                    XX -= 2;
                                    YY += 2;
                                    break;
                                }
                        }

                        var loopTo1 = ModuleSMM.MapMoveBlk[EX - 1].NodeCount - 1;
                        for (i = 0; i <= loopTo1; i++)
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SS.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            // G.DrawString(MapMoveBlk(EX - 1).Node(i).p1, Me.Font, Brushes.Black, (XX) * Zm, (H - YY) * Zm)
                            switch (ModuleSMM.MapMoveBlk[EX - 1].Node[i].p1)
                            {
                                case 1: // L
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SL.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX -= 2;
                                        break;
                                    }

                                case 2: // R
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SR.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX += 2;
                                        break;
                                    }

                                case 3: // D
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SD.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY -= 2;
                                        break;
                                    }

                                case 4: // U
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SU.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY += 2;
                                        break;
                                    }

                                case 5: // LD
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SRD.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY -= 2;
                                        break;
                                    }

                                case 6: // DL
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SUL.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX -= 2;
                                        break;
                                    }

                                case 7: // LU
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SRU.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY += 2;
                                        break;
                                    }

                                case 8: // UL
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SDL.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX -= 2;
                                        break;
                                    }

                                case 9: // RD
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SLD.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY -= 2;
                                        break;
                                    }

                                case 10: // DR
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SUR.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX += 2;
                                        break;
                                    }

                                case 11: // RU
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SLU.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        YY += 2;
                                        break;
                                    }

                                case 12: // UR
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SDR.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        XX += 2;
                                        break;
                                    }

                                case 13: // RE
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        break;
                                    }

                                case 14: // LE
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        break;
                                    }

                                case 15: // UE
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        break;
                                    }

                                case 16: // DE
                                    {
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                                        break;
                                    }
                            }
                        }

                        break;
                    }
            }

            Err:
            ;
        }

        public void DrawCrp(byte EX, int X, int Y)
        {
            int H, W, XX, YY;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            XX = (int)Math.Round(X / 160d + 1d);
            YY = (int)Math.Round((Y + 80) / 160d + 1d);
            int i;
            switch (ModuleSMM.MapCrp[EX - 1].Node[0])
            {
                case 1:
                case 5:
                case 7:
                case 14:
                    {
                        XX -= 4;
                        break;
                    }

                case 2:
                case 9:
                case 11:
                case 13:
                    {
                        break;
                    }

                case 3:
                case 6:
                case 10:
                case 16:
                    {
                        XX -= 2;
                        YY -= 2;
                        break;
                    }

                case 4:
                case 8:
                case 12:
                case 15:
                    {
                        XX -= 2;
                        YY += 2;
                        break;
                    }
            }

            var loopTo = ModuleSMM.MapCrp[EX - 1].NodeCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SS.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                // G.DrawString(MapCrp(EX - 1).Node(i), Me.Font, Brushes.Black, (XX) * Zm, (H - YY) * Zm)
                switch (ModuleSMM.MapCrp[EX - 1].Node[i])
                {
                    case 1: // L
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SL.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            XX -= 2;
                            break;
                        }

                    case 2: // R
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SR.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            XX += 2;
                            break;
                        }

                    case 3: // D
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SD.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            YY -= 2;
                            break;
                        }

                    case 4: // U
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SU.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            YY += 2;
                            break;
                        }

                    case 5: // LD
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SRD.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            YY -= 2;
                            break;
                        }

                    case 6: // DL
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SUL.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            XX -= 2;
                            break;
                        }

                    case 7: // LU
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SRU.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            YY += 2;
                            break;
                        }

                    case 8: // UL
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SDL.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            XX -= 2;
                            break;
                        }

                    case 9: // RD
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SLD.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            YY -= 2;
                            break;
                        }

                    case 10: // DR
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SUR.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            XX += 2;
                            break;
                        }

                    case 11: // RU
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SLU.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            YY += 2;
                            break;
                        }

                    case 12: // UR
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SDR.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            XX += 2;
                            break;
                        }

                    case 13: // RE
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            break;
                        }

                    case 14: // LE
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            break;
                        }

                    case 15: // UE
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            break;
                        }

                    case 16: // DE
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm * 2, Zm * 2);
                            break;
                        }
                }
            }

            Err:
            ;
        }
        // 蛇砖块
        public void DrawSnake(byte EX, int X, int Y, int SW, int SH)
        {
            int H, W, XX, YY;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            YY = (int)Math.Round((Y + SH * 80) / 160d);
            if (EX < 0x10)
            {
                XX = (int)Math.Round((X + SW * 80) / 160d);
                EX = (byte)(EX % 0x10);
                switch (ModuleSMM.MapSnk[EX - 1].Node[0].Dir)
                {
                    case 1:
                    case 5:
                    case 7:
                        {
                            XX -= 1;
                            break;
                        }

                    case 2:
                    case 9:
                    case 11:
                        {
                            break;
                        }

                    case 3:
                    case 6:
                    case 10:
                        {
                            XX -= 1;
                            YY -= 1;
                            break;
                        }

                    case 4:
                    case 8:
                    case 12:
                        {
                            XX -= 1;
                            YY += 1;
                            break;
                        }
                }
            }
            else
            {
                XX = (int)Math.Round((X - SW * 80) / 160d);
                EX = (byte)(EX % 0x10);
                switch (ModuleSMM.MapSnk[EX - 1].Node[0].Dir)
                {
                    case 1:
                    case 5:
                    case 7:
                        {
                            XX -= 1;
                            break;
                        }

                    case 2:
                    case 9:
                    case 11:
                        {
                            break;
                        }

                    case 3:
                    case 6:
                    case 10:
                        {
                            YY -= 1;
                            break;
                        }

                    case 4:
                    case 8:
                    case 12:
                        {
                            YY += 1;
                            break;
                        }
                }
            }

            int i;
            var loopTo = ModuleSMM.MapSnk[EX - 1].NodeCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SS.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                // G.DrawString(MapSnk(EX - 1).Node(i).Dir, Me.Font, Brushes.Black, (XX + 0.5) * Zm, (H - YY - 0.5) * Zm)
                switch (ModuleSMM.MapSnk[EX - 1].Node[i].Dir)
                {
                    case 1: // L
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SL.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            XX -= 1;
                            break;
                        }

                    case 2: // R
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SR.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            XX += 1;
                            break;
                        }

                    case 3: // D
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SD.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            YY -= 1;
                            break;
                        }

                    case 4: // U
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SU.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            YY += 1;
                            break;
                        }

                    case 5: // LD
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SRD.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            YY -= 1;
                            break;
                        }

                    case 6: // DL
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SUL.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            XX -= 1;
                            break;
                        }

                    case 7: // LU
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SRU.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            YY += 1;
                            break;
                        }

                    case 8: // UL
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SDL.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            XX -= 1;
                            break;
                        }

                    case 9: // RD
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SLD.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            YY -= 1;
                            break;
                        }

                    case 10: // DR
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SUR.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            XX += 1;
                            break;
                        }

                    case 11: // RU
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SLU.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            YY += 1;
                            break;
                        }

                    case 12: // UR
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SDR.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            XX += 1;
                            break;
                        }

                    case 13: // RE
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            break;
                        }

                    case 14: // LE
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            break;
                        }

                    case 15: // UE
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            break;
                        }

                    case 16: // DE
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\CMN\SE.PNG"), XX * Zm, (H - YY) * Zm, Zm, Zm);
                            break;
                        }
                }
            }

            Err:
            ;
        }

        public void DrawIce()
        {
            // 冰块
            int i;
            int H;
            var loopTo = ModuleSMM.MapHdr.IceCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (ModuleSMM.MapIce[i].ID == 0)
                {
                    G.DrawImage(GetTile(15, 41, 1, 2), ModuleSMM.MapIce[i].X * Zm, (ModuleSMM.MapHdr.BorT / 16 - 2) * Zm - ModuleSMM.MapIce[i].Y * Zm, Zm, Zm * 2);
                    for (H = 1; H <= 2; H++)
                    {
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].Obj += "118,";
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].Flag += ",";
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].SubObj += ",";
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].SubFlag += ",";
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].State += ",";
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].SubState += ",";
                    }
                }
                else
                {
                    G.DrawImage(GetTile(15, 39, 1, 2), ModuleSMM.MapIce[i].X * Zm, (ModuleSMM.MapHdr.BorT / 16 - 2) * Zm - ModuleSMM.MapIce[i].Y * Zm, Zm, Zm * 2);
                    for (H = 1; H <= 2; H++)
                    {
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].Obj += "118A,";
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].Flag += ",";
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].SubObj += ",";
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].SubFlag += ",";
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].State += ",";
                        ModuleSMM.ObjLocData[NowIO, ModuleSMM.MapIce[i].X + 1, Conversion.Int(H + ModuleSMM.MapIce[i].Y)].SubState += ",";
                    }
                }
            }
        }

        public void DrawTrack()
        {
            // 轨道
            int H;
            int W;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            int i;
            var loopTo = ModuleSMM.MapHdr.TrackCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                // LID+1?
                ModuleSMM.ObjLinkType[ModuleSMM.MapTrk[i].LID] = 59;
                if (ModuleSMM.MapTrk[i].Type < 8)
                {
                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T" + ModuleSMM.MapTrk[i].Type.ToString() + ".PNG"), ModuleSMM.MapTrk[i].X * Zm - Zm, (H - 2) * Zm - ModuleSMM.MapTrk[i].Y * Zm, Zm * 3, Zm * 3);
                    switch (ModuleSMM.MapTrk[i].Type)
                    {
                        case 0:
                            {
                                if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + 1 + 1, ModuleSMM.MapTrk[i].Y + 1] == 1 && ModuleSMM.MapTrk[i].F0 == 0)
                                {
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), ModuleSMM.MapTrk[i].X * Zm + Zm, (H - 1) * Zm - ModuleSMM.MapTrk[i].Y * Zm, Zm, Zm);
                                }

                                if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + 1 - 1, ModuleSMM.MapTrk[i].Y + 1] == 1 && ModuleSMM.MapTrk[i].F1 == 0)
                                {
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), ModuleSMM.MapTrk[i].X * Zm - Zm, (H - 1) * Zm - ModuleSMM.MapTrk[i].Y * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 1:
                            {
                                if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + 1, ModuleSMM.MapTrk[i].Y + 1 + 1] == 1 && ModuleSMM.MapTrk[i].F0 == 0)
                                {
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), ModuleSMM.MapTrk[i].X * Zm, (H - 2) * Zm - ModuleSMM.MapTrk[i].Y * Zm, Zm, Zm);
                                }

                                if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + 1, ModuleSMM.MapTrk[i].Y + 1 - 1] == 1 && ModuleSMM.MapTrk[i].F1 == 0)
                                {
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), ModuleSMM.MapTrk[i].X * Zm, H * Zm - ModuleSMM.MapTrk[i].Y * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 2:
                        case 4:
                        case 5:
                            {
                                if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + 1 + 1, ModuleSMM.MapTrk[i].Y + 1 - 1] == 1 && ModuleSMM.MapTrk[i].F0 == 0)
                                {
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), ModuleSMM.MapTrk[i].X * Zm + Zm, H * Zm - ModuleSMM.MapTrk[i].Y * Zm, Zm, Zm);
                                }

                                if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + 1 - 1, ModuleSMM.MapTrk[i].Y + 1 + 1] == 1 && ModuleSMM.MapTrk[i].F1 == 0)
                                {
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), ModuleSMM.MapTrk[i].X * Zm - Zm, (H - 2) * Zm - ModuleSMM.MapTrk[i].Y * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 3:
                        case 6:
                        case 7:
                            {
                                if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + 1 + 1, ModuleSMM.MapTrk[i].Y + 1 + 1] == 1 && ModuleSMM.MapTrk[i].F0 == 0)
                                {
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), ModuleSMM.MapTrk[i].X * Zm + Zm, (H - 2) * Zm - ModuleSMM.MapTrk[i].Y * Zm, Zm, Zm);
                                }

                                if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + 1 - 1, ModuleSMM.MapTrk[i].Y + 1 - 1] == 1 && ModuleSMM.MapTrk[i].F1 == 0)
                                {
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), ModuleSMM.MapTrk[i].X * Zm - Zm, H * Zm - ModuleSMM.MapTrk[i].Y * Zm, Zm, Zm);
                                }

                                break;
                            }
                    }
                }
                else // Y轨道
                {
                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T" + ModuleSMM.MapTrk[i].Type.ToString() + ".PNG"), ModuleSMM.MapTrk[i].X * Zm - Zm, (H - 4) * Zm - ModuleSMM.MapTrk[i].Y * Zm, Zm * 5, Zm * 5);
                    // G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(i).X - 1 + TrackYPt(MapTrk(i).Type, 0).X) * Zm, H * Zm - (MapTrk(i).Y + TrackYPt(MapTrk(i).Type, 0).Y) * Zm, Zm, Zm)
                    // G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(i).X - 1 + TrackYPt(MapTrk(i).Type, 1).X) * Zm, H * Zm - (MapTrk(i).Y + TrackYPt(MapTrk(i).Type, 1).Y) * Zm, Zm, Zm)
                    // G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\T.PNG"), (MapTrk(i).X - 1 + TrackYPt(MapTrk(i).Type, 2).X) * Zm, H * Zm - (MapTrk(i).Y + TrackYPt(MapTrk(i).Type, 2).Y) * Zm, Zm, Zm)

                    if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 0].X, ModuleSMM.MapTrk[i].Y + ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 0].Y] == 1 && ModuleSMM.MapTrk[i].F0 == 0)
                    {
                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), (ModuleSMM.MapTrk[i].X - 1 + ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 0].X) * Zm, (H - 4) * Zm - (ModuleSMM.MapTrk[i].Y - ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 0].Y) * Zm, Zm, Zm);
                    }

                    if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 1].X, ModuleSMM.MapTrk[i].Y + ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 1].Y] == 1 && ModuleSMM.MapTrk[i].F1 == 0)
                    {
                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), (ModuleSMM.MapTrk[i].X - 1 + ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 1].X) * Zm, (H - 4) * Zm - (ModuleSMM.MapTrk[i].Y - ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 1].Y) * Zm, Zm, Zm);
                    }

                    if (ModuleSMM.TrackNode[ModuleSMM.MapTrk[i].X + ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 2].X, ModuleSMM.MapTrk[i].Y + ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 2].Y] == 1 && ModuleSMM.MapTrk[i].F2 == 0)
                    {
                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\T.PNG"), (ModuleSMM.MapTrk[i].X - 1 + ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 2].X) * Zm, (H - 4) * Zm - (ModuleSMM.MapTrk[i].Y - ModuleSMM.TrackYPt[ModuleSMM.MapTrk[i].Type, 2].Y) * Zm, Zm, Zm);
                    }
                }
            }
        }

        public void DrawSlope()
        {
            int H;
            int W;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            int i;
            int CX, CY;
            // GrdType
            // 0无 1方 2陡左上 3陡右上 4陡左下 5陡右下 6端左上 7端右上 8端左下 9端右下
            // 
            var loopTo = ModuleSMM.MapHdr.ObjCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                switch (ModuleSMM.MapObj[i].ID)
                {
                    case 87:
                        {
                            // 缓坡
                            CX = (int)Math.Round(-0.5d + ModuleSMM.MapObj[i].X / 160d);
                            CY = (int)Math.Round(-0.5d + ModuleSMM.MapObj[i].Y / 160d);
                            if (ModuleSMM.MapObj[i].Flag / 0x100000 % 0x2 == 0)
                            {
                                // 左斜
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\7.PNG"), CX * Zm, (H - 1) * Zm - (float)(CY * Zm), Zm, Zm);
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\7.PNG"), (float)((ModuleSMM.MapObj[i].W - 1.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 1) * Zm - (float)((CY + ModuleSMM.MapObj[i].H - 1) * Zm), Zm, Zm);
                                // G.DrawString(GroundNode(CX + 1, CY + 1), Me.Font, Brushes.Black, CSng(CX * Zm), (H - 1) * Zm - CSng(CY * Zm))
                                // G.DrawString(GroundNode(CX + MapObj(i).W, CY + MapObj(i).H), Me.Font, Brushes.Black, CSng((CX + MapObj(i).W - 1) * Zm), (H - 1) * Zm - CSng((CY + MapObj(i).H - 1) * Zm))

                                for (int j = 1, loopTo1 = ModuleSMM.MapObj[i].W - 2; j <= loopTo1; j += 2)
                                    // G.DrawString(GroundNode(CX + 1 + j, CY + 3 + (j \ 2)), Me.Font, Brushes.Black, CSng((CX + j) * Zm), (H - 1) * Zm - CSng((CY + 1 + (j \ 2)) * Zm))
                                    // G.DrawString(GroundNode(CX + 1 + j, CY + 2 + (j \ 2)), Me.Font, Brushes.Black, CSng((CX + j) * Zm), (H - 1) * Zm - CSng((CY + (j \ 2)) * Zm))

                                    // G.DrawString(GroundNode(CX + 2 + j, CY + 3 + (j \ 2)), Me.Font, Brushes.Black, CSng((CX + j + 1) * Zm), (H - 1) * Zm - CSng((CY + 1 + (j \ 2)) * Zm))
                                    // G.DrawString(GroundNode(CX + 2 + j, CY + 2 + (j \ 2)), Me.Font, Brushes.Black, CSng((CX + j + 1) * Zm), (H - 1) * Zm - CSng((CY + (j \ 2)) * Zm))
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\87.PNG"), (CX + j) * Zm, (H - 1) * Zm - (float)((j / 2d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * 2, Zm * 2);
                            }
                            else
                            {
                                // 右斜
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\7.PNG"), (float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 1) * Zm - (float)((ModuleSMM.MapObj[i].H - 1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\7.PNG"), (float)((ModuleSMM.MapObj[i].W - 1.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 1) * Zm - (float)((-0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                // G.DrawString(GroundNode(CX + 1, CY + MapObj(i).H), Me.Font, Brushes.Black, CSng(CX * Zm), (H - 1) * Zm - CSng((CY + MapObj(i).H - 1) * Zm))
                                // G.DrawString(GroundNode(CX + MapObj(i).W, CY + 1), Me.Font, Brushes.Black, CSng((CX + MapObj(i).W - 1) * Zm), (H - 1) * Zm - CSng(CY * Zm))
                                for (int j = 1, loopTo2 = ModuleSMM.MapObj[i].W - 2; j <= loopTo2; j += 2)
                                    // G.DrawString(GroundNode(CX + 1 + j, CY + MapObj(i).H - (j \ 2)), Me.Font, Brushes.Black,
                                    // CSng((CX + j) * Zm), (H - 1) * Zm - CSng((CY + MapObj(i).H - (j \ 2) - 1) * Zm))
                                    // G.DrawString(GroundNode(CX + 1 + j, CY + MapObj(i).H - (j \ 2) - 1), Me.Font, Brushes.Black,
                                    // CSng((CX + j) * Zm), (H - 1) * Zm - CSng((CY + MapObj(i).H - (j \ 2) - 2) * Zm))

                                    // G.DrawString(GroundNode(CX + 2 + j, CY + MapObj(i).H - (j \ 2)), Me.Font, Brushes.Black,
                                    // CSng((CX + j + 1) * Zm), (H - 1) * Zm - CSng((CY + MapObj(i).H - (j \ 2) - 1) * Zm))
                                    // G.DrawString(GroundNode(CX + 2 + j, CY + MapObj(i).H - (j \ 2) - 1), Me.Font, Brushes.Black,
                                    // CSng((CX + j + 1) * Zm), (H - 1) * Zm - CSng((CY + MapObj(i).H - (j \ 2) - 2) * Zm))
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\87A.PNG"), (float)((j - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 1) * Zm - (float)((-j / 2d + ModuleSMM.MapObj[i].H - 1d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * 2, Zm * 2);
                            }

                            break;
                        }

                    case 88:
                        {
                            // 陡坡
                            CX = (int)Math.Round(-0.5d + ModuleSMM.MapObj[i].X / 160d);
                            CY = (int)Math.Round(-0.5d + ModuleSMM.MapObj[i].Y / 160d);
                            if (ModuleSMM.MapObj[i].Flag / 0x100000 % 0x2 == 0)
                            {
                                // 左斜
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\7.PNG"), CX * Zm, (H - 1) * Zm - (float)(CY * Zm), Zm, Zm);
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\7.PNG"), (CX + ModuleSMM.MapObj[i].W - 1) * Zm, (H - 1) * Zm - (float)((CY + ModuleSMM.MapObj[i].H - 1) * Zm), Zm, Zm);
                                // G.DrawString(GroundNode(CX + 1, CY + 1), Me.Font, Brushes.Black, CSng((CX) * Zm), (H - 1) * Zm - CSng(CY * Zm))
                                // G.DrawString(GroundNode(CX + MapObj(i).W, CY + MapObj(i).H), Me.Font, Brushes.Black, CSng((CX + MapObj(i).W - 1) * Zm), (H - 1) * Zm - CSng((CY + MapObj(i).H - 1) * Zm))
                                for (int j = 1, loopTo3 = ModuleSMM.MapObj[i].W - 2; j <= loopTo3; j++)
                                    // G.DrawString(GroundNode(CX + 1 + j, CY + 1 + j), Me.Font, Brushes.Black, CSng((CX + j) * Zm), (H - 1) * Zm - CSng((CY + j) * Zm))
                                    // G.DrawString(GroundNode(CX + 1 + j, CY + j), Me.Font, Brushes.Black, CSng((CX + j) * Zm), (H - 1) * Zm - CSng((CY - 1 + j) * Zm))

                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\88.PNG"), (CX + j) * Zm, (H - 1) * Zm - (float)((CY + j) * Zm), Zm, Zm * 2);
                            }
                            else
                            {
                                // 右斜
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\7.PNG"), CX * Zm, (H - 1) * Zm - (float)((CY + ModuleSMM.MapObj[i].H - 1) * Zm), Zm, Zm);
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\7.PNG"), (CX + ModuleSMM.MapObj[i].W - 1) * Zm, (H - 1) * Zm - (float)(CY * Zm), Zm, Zm);
                                // G.DrawString(GroundNode(CX + MapObj(i).W, CY + 1), Me.Font, Brushes.Black, CSng((CX + MapObj(i).W - 1) * Zm), (H - 1) * Zm - CSng(CY * Zm))
                                // G.DrawString(GroundNode(CX + 1, CY + MapObj(i).W), Me.Font, Brushes.Black, CSng((CX) * Zm), (H - 1) * Zm - CSng((CY + MapObj(i).H - 1) * Zm))

                                for (int j = 1, loopTo4 = ModuleSMM.MapObj[i].W - 2; j <= loopTo4; j++)
                                    // G.DrawString(GroundNode(CX + 1 + j, CY - j - 1 + MapObj(i).W), Me.Font, Brushes.Black, CSng((CX + j) * Zm), (H - 1) * Zm - CSng((CY - j - 2 + MapObj(i).W) * Zm))
                                    // G.DrawString(GroundNode(CX + 1 + j, CY - j + MapObj(i).W), Me.Font, Brushes.Black, CSng((CX + j) * Zm), (H - 1) * Zm - CSng((CY - j - 1 + MapObj(i).W) * Zm))
                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\88A.PNG"), (CX + j) * Zm, (H - 1) * Zm - (float)((CY - j - 1 + ModuleSMM.MapObj[i].W) * Zm), Zm, Zm * 2);
                            }

                            break;
                        }
                }
            }
        }

        public int GetGrdBold(int x, int y)
        {
            int GetGrdBoldRet = default;
            if (ModuleSMM.GroundNode[x, y + 1] > 1) // U
            {
                GetGrdBoldRet = ModuleSMM.GroundNode[x, y + 1];
            }
            else if (ModuleSMM.GroundNode[x, y - 1] > 1) // D
            {
                GetGrdBoldRet = ModuleSMM.GroundNode[x, y - 1];
            }
            else
            {
                GetGrdBoldRet = 0;
            }

            return GetGrdBoldRet;
        }

        public int CalC(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            int CalCRet = default;
            CalCRet = Conversions.ToInteger(Operators.AddObject(Operators.AddObject(Operators.AddObject(Interaction.IIf(ModuleSMM.GroundNode[x1, y1] == 0, 0, 1000), Interaction.IIf(ModuleSMM.GroundNode[x2, y2] == 0, 0, 100)), Interaction.IIf(ModuleSMM.GroundNode[x3, y3] == 0, 0, 10)), Interaction.IIf(ModuleSMM.GroundNode[x4, y4] == 0, 0, 1)));
            return CalCRet;
        }

        public Point GetCorCode(int x, int y)
        {
            Point GetCorCodeRet = default;
            int C;
            switch (GetGrdBold(x, y))
            {
                case 2: // D陡左上
                    {
                        C = CalC(x - 1, y, x - 1, y + 1, x, y + 1, x + 1, y + 1);
                        switch (C)
                        {
                            case 0:
                                {
                                    GetCorCodeRet = new Point(5, 30);
                                    break;
                                }

                            case 1:
                            case 100:
                            case 101:
                                {
                                    GetCorCodeRet = new Point(10, 13);
                                    break;
                                }

                            case 10:
                                {
                                    GetCorCodeRet = new Point(15, 26);
                                    break;
                                }

                            case 11:
                            case 111:
                                {
                                    GetCorCodeRet = new Point(9, 27);
                                    break;
                                }

                            case 110:
                                {
                                    GetCorCodeRet = new Point(15, 27);
                                    break;
                                }

                            case 1000:
                            case 1001:
                            case 1100:
                            case 1101:
                                {
                                    GetCorCodeRet = new Point(5, 27);
                                    break;
                                }

                            case 1010:
                                {
                                    GetCorCodeRet = new Point(13, 27);
                                    break;
                                }

                            case 1011:
                                {
                                    GetCorCodeRet = new Point(7, 27);
                                    break;
                                }

                            case 1110:
                                {
                                    GetCorCodeRet = new Point(11, 27);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(1, 26);
                                    break;
                                }
                        }

                        break;
                    }

                case 3: // D陡右上
                    {
                        C = CalC(x - 1, y + 1, x, y + 1, x + 1, y + 1, x + 1, y);
                        switch (C)
                        {
                            case 0:
                            case 10:
                            case 1000:
                            case 1010:
                                {
                                    GetCorCodeRet = new Point(4, 30);
                                    break;
                                }

                            case 1:
                            case 11:
                            case 1001:
                            case 1011:
                                {
                                    GetCorCodeRet = new Point(4, 27);
                                    break;
                                }

                            case 100:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(14, 27);
                                    break;
                                }

                            case 101:
                                {
                                    GetCorCodeRet = new Point(12, 27);
                                    break;
                                }

                            case 111:
                                {
                                    GetCorCodeRet = new Point(10, 27);
                                    break;
                                }

                            case 1100:
                            case 1110:
                                {
                                    GetCorCodeRet = new Point(8, 27);
                                    break;
                                }

                            case 1101:
                                {
                                    GetCorCodeRet = new Point(6, 27);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(0, 26);
                                    break;
                                }
                        }

                        break;
                    }

                case 4: // U陡左下
                    {
                        C = CalC(x - 1, y, x - 1, y - 1, x, y - 1, x + 1, y - 1);
                        switch (C)
                        {
                            case 0:
                            case 1:
                            case 100:
                            case 101:
                                {
                                    GetCorCodeRet = new Point(5, 29);
                                    break;
                                }

                            case 10:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(15, 26);
                                    break;
                                }

                            case 11:
                            case 111:
                                {
                                    GetCorCodeRet = new Point(9, 26);
                                    break;
                                }

                            case 1000:
                            case 1001:
                            case 1100:
                            case 1101:
                                {
                                    GetCorCodeRet = new Point(5, 26);
                                    break;
                                }

                            case 1010:
                                {
                                    GetCorCodeRet = new Point(13, 26);
                                    break;
                                }

                            case 1011:
                                {
                                    GetCorCodeRet = new Point(7, 26);
                                    break;
                                }

                            case 1110:
                                {
                                    GetCorCodeRet = new Point(11, 26);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(1, 25);
                                    break;
                                }
                        }

                        break;
                    }

                case 5: // U陡右下
                    {
                        C = CalC(x - 1, y - 1, x, y - 1, x + 1, y - 1, x + 1, y);
                        switch (C)
                        {
                            case 0:
                            case 10:
                            case 1000:
                            case 1010:
                                {
                                    GetCorCodeRet = new Point(4, 29);
                                    break;
                                }

                            case 1:
                            case 11:
                            case 1001:
                            case 1011:
                                {
                                    GetCorCodeRet = new Point(4, 26);
                                    break;
                                }

                            case 100:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(14, 26);
                                    break;
                                }

                            case 101:
                                {
                                    GetCorCodeRet = new Point(12, 26);
                                    break;
                                }

                            case 111:
                                {
                                    GetCorCodeRet = new Point(10, 26);
                                    break;
                                }

                            case 1100:
                            case 1110:
                                {
                                    GetCorCodeRet = new Point(8, 26);
                                    break;
                                }

                            case 1101:
                                {
                                    GetCorCodeRet = new Point(6, 26);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(0, 25);
                                    break;
                                }
                        }

                        break;
                    }

                case 12: // D缓大左上
                    {
                        C = CalC(x - 1, y, x - 1, y + 1, x, y + 1, x + 1, y + 1);
                        switch (C)
                        {
                            case 0:
                            case 1:
                            case 100:
                            case 101:
                                {
                                    GetCorCodeRet = new Point(5, 30);
                                    break;
                                }

                            case 10:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(15, 30);
                                    break;
                                }

                            case 11:
                            case 111:
                                {
                                    GetCorCodeRet = new Point(13, 31);
                                    break;
                                }

                            case 1000:
                            case 1001:
                            case 1100:
                            case 1101:
                                {
                                    GetCorCodeRet = new Point(8, 31);
                                    break;
                                }

                            case 1010:
                                {
                                    GetCorCodeRet = new Point(13, 30);
                                    break;
                                }

                            case 1011:
                                {
                                    GetCorCodeRet = new Point(11, 31);
                                    break;
                                }

                            case 1110:
                                {
                                    GetCorCodeRet = new Point(11, 30);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(2, 30);
                                    break;
                                }
                        }

                        break;
                    }

                case 13: // D缓大右上
                    {
                        C = CalC(x - 1, y + 1, x, y + 1, x + 1, y + 1, x + 1, y);
                        switch (C)
                        {
                            case 0:
                            case 10:
                            case 1000:
                            case 1010:
                                {
                                    GetCorCodeRet = new Point(4, 30);
                                    break;
                                }

                            case 1:
                            case 11:
                            case 1001:
                            case 1011:
                                {
                                    GetCorCodeRet = new Point(7, 31);
                                    break;
                                }

                            case 100:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(14, 30);
                                    break;
                                }

                            case 101:
                                {
                                    GetCorCodeRet = new Point(12, 30);
                                    break;
                                }

                            case 111:
                                {
                                    GetCorCodeRet = new Point(10, 30);
                                    break;
                                }

                            case 1100:
                            case 1110:
                                {
                                    GetCorCodeRet = new Point(12, 31);
                                    break;
                                }

                            case 1101:
                                {
                                    GetCorCodeRet = new Point(10, 31);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(1, 30);
                                    break;
                                }
                        }

                        break;
                    }

                case 14: // U缓大左下
                    {
                        C = CalC(x - 1, y, x - 1, y - 1, x, y - 1, x + 1, y - 1);
                        switch (C)
                        {
                            case 0:
                            case 1:
                            case 100:
                            case 101:
                                {
                                    GetCorCodeRet = new Point(5, 29);
                                    break;
                                }

                            case 10:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(15, 29);
                                    break;
                                }

                            case 11:
                            case 111:
                                {
                                    GetCorCodeRet = new Point(13, 28);
                                    break;
                                }

                            case 1000:
                            case 1001:
                            case 1100:
                            case 1101:
                                {
                                    GetCorCodeRet = new Point(8, 28);
                                    break;
                                }

                            case 1010:
                                {
                                    GetCorCodeRet = new Point(13, 29);
                                    break;
                                }

                            case 1011:
                                {
                                    GetCorCodeRet = new Point(10, 29);
                                    break;
                                }

                            case 1110:
                                {
                                    GetCorCodeRet = new Point(11, 29);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(2, 29);
                                    break;
                                }
                        }

                        break;
                    }

                case 15: // U缓大右下
                    {
                        C = CalC(x - 1, y - 1, x, y - 1, x + 1, y - 1, x + 1, y);
                        switch (C)
                        {
                            case 0:
                            case 10:
                            case 1000:
                            case 1010:
                                {
                                    GetCorCodeRet = new Point(4, 29);
                                    break;
                                }

                            case 1:
                            case 11:
                            case 1001:
                            case 1011:
                                {
                                    GetCorCodeRet = new Point(7, 28);
                                    break;
                                }

                            case 100:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(14, 29);
                                    break;
                                }

                            case 101:
                                {
                                    GetCorCodeRet = new Point(12, 29);
                                    break;
                                }

                            case 111:
                                {
                                    GetCorCodeRet = new Point(10, 29);
                                    break;
                                }

                            case 1100:
                            case 1110:
                                {
                                    GetCorCodeRet = new Point(12, 28);
                                    break;
                                }

                            case 1101:
                                {
                                    GetCorCodeRet = new Point(11, 29);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(1, 29);
                                    break;
                                }
                        }

                        break;
                    }

                case 16: // D缓小左上
                    {
                        C = CalC(x - 1, y, x - 1, y + 1, x, y + 1, x + 1, y + 1);
                        switch (C)
                        {
                            case 0:
                            case 1:
                            case 100:
                            case 101:
                                {
                                    GetCorCodeRet = new Point(5, 30);
                                    break;
                                }

                            case 10:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(15, 27);
                                    break;
                                }

                            case 11:
                            case 111:
                                {
                                    GetCorCodeRet = new Point(9, 27);
                                    break;
                                }

                            case 1000:
                            case 1001:
                            case 1100:
                            case 1101:
                                {
                                    GetCorCodeRet = new Point(9, 31);
                                    break;
                                }

                            case 1010:
                                {
                                    GetCorCodeRet = new Point(9, 30);
                                    break;
                                }

                            case 1011:
                                {
                                    GetCorCodeRet = new Point(15, 31);
                                    break;
                                }

                            case 1110:
                                {
                                    GetCorCodeRet = new Point(7, 30);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(3, 30);
                                    break;
                                }
                        }

                        break;
                    }

                case 17: // D缓小右上
                    {
                        C = CalC(x - 1, y + 1, x, y + 1, x + 1, y + 1, x + 1, y);
                        switch (C)
                        {
                            case 0:
                            case 10:
                            case 1000:
                            case 1010:
                                {
                                    GetCorCodeRet = new Point(4, 30);
                                    break;
                                }

                            case 1:
                            case 11:
                            case 1001:
                            case 1011:
                                {
                                    GetCorCodeRet = new Point(6, 31);
                                    break;
                                }

                            case 100:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(14, 27);
                                    break;
                                }

                            case 101:
                                {
                                    GetCorCodeRet = new Point(8, 30);
                                    break;
                                }

                            case 111:
                                {
                                    GetCorCodeRet = new Point(6, 30);
                                    break;
                                }

                            case 1100:
                            case 1110:
                                {
                                    GetCorCodeRet = new Point(8, 27);
                                    break;
                                }

                            case 1101:
                                {
                                    GetCorCodeRet = new Point(14, 31);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(0, 30);
                                    break;
                                }
                        }

                        break;
                    }

                case 18: // U缓小左下
                    {
                        C = CalC(x - 1, y, x - 1, y - 1, x, y - 1, x + 1, y - 1);
                        switch (C)
                        {
                            case 0:
                            case 1:
                            case 100:
                            case 101:
                                {
                                    GetCorCodeRet = new Point(5, 29);
                                    break;
                                }

                            case 10:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(15, 26);
                                    break;
                                }

                            case 11:
                            case 111:
                                {
                                    GetCorCodeRet = new Point(9, 26);
                                    break;
                                }

                            case 1000:
                            case 1001:
                            case 1100:
                            case 1101:
                                {
                                    GetCorCodeRet = new Point(9, 28);
                                    break;
                                }

                            case 1010:
                                {
                                    GetCorCodeRet = new Point(9, 29);
                                    break;
                                }

                            case 1011:
                                {
                                    GetCorCodeRet = new Point(15, 28);
                                    break;
                                }

                            case 1110:
                                {
                                    GetCorCodeRet = new Point(7, 29);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(3, 29);
                                    break;
                                }
                        }

                        break;
                    }

                case 19: // U缓小右下
                    {
                        C = CalC(x - 1, y - 1, x, y - 1, x + 1, y - 1, x + 1, y);
                        switch (C)
                        {
                            case 0:
                            case 10:
                            case 1000:
                            case 1010:
                                {
                                    GetCorCodeRet = new Point(4, 29);
                                    break;
                                }

                            case 1:
                            case 11:
                            case 1001:
                            case 1011:
                                {
                                    GetCorCodeRet = new Point(6, 28);
                                    break;
                                }

                            case 100:
                            case 110:
                                {
                                    GetCorCodeRet = new Point(14, 26);
                                    break;
                                }

                            case 101:
                                {
                                    GetCorCodeRet = new Point(8, 29);
                                    break;
                                }

                            case 111:
                                {
                                    GetCorCodeRet = new Point(6, 29);
                                    break;
                                }

                            case 1100:
                            case 1110:
                                {
                                    GetCorCodeRet = new Point(8, 26);
                                    break;
                                }

                            case 1101:
                                {
                                    GetCorCodeRet = new Point(14, 28);
                                    break;
                                }

                            case 1111:
                                {
                                    GetCorCodeRet = new Point(0, 29);
                                    break;
                                }
                        }

                        break;
                    }

                default:
                    {
                        GetCorCodeRet = ModuleSMM.GrdLoc[GetGrdType(GetGrdCode(x - 1, y - 1))];
                        break;
                    }
            }

            return GetCorCodeRet;
        }

        public void DrawGrdCode()
        {
            // 绘制地形
            int i;
            int j;
            Point R;
            // 斜坡
            int H;
            int W;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            // GrdType
            // 0无 1方 2陡左上 3陡右上 4陡左下 5陡右下 6端左上 7端右上 8端左下 9端右下
            // 10  11  12缓大左上 13缓大右上 14缓大左下 15缓大右下  16缓小左上 17缓小右上 18缓小左下 19缓小右下
            // 20端左上 21端右上 22端左下 23端右下 24    25    26

            var loopTo = W + 1;
            for (i = 1; i <= loopTo; i++)
            {
                var loopTo1 = H + 1;
                for (j = 1; j <= loopTo1; j++)
                {
                    switch (ModuleSMM.GroundNode[i, j])
                    {
                        case 0:
                            {
                                break;
                            }
                        // G.DrawString(GroundNode(i, j), Me.Font, Brushes.Black, (i - 1) * Zm, (H - j) * Zm)
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                            {
                                R = ModuleSMM.GrdLoc[GetGrdType(GetGrdCode(i - 1, j - 1))];
                                G.DrawImage(GetTile(R.X, R.Y, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                ModuleSMM.GroundNode[i, j] = 1;
                                break;
                            }

                        case 2:
                            {
                                if (ModuleSMM.GroundNode[i, j + 1] == 5)
                                {
                                    G.DrawImage(GetTile(2, 25, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else if (ModuleSMM.GroundNode[i - 1, j + 1] == 1)
                                {
                                    G.DrawImage(GetTile(1, 27, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else
                                {
                                    G.DrawImage(GetTile(3, 27, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 3:
                            {
                                if (ModuleSMM.GroundNode[i, j + 1] == 4)
                                {
                                    G.DrawImage(GetTile(3, 25, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else if (ModuleSMM.GroundNode[i + 1, j + 1] == 1)
                                {
                                    G.DrawImage(GetTile(0, 27, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else
                                {
                                    G.DrawImage(GetTile(2, 27, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 4:
                            {
                                if (ModuleSMM.GroundNode[i, j - 1] == 3)
                                {
                                    G.DrawImage(GetTile(3, 24, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else if (ModuleSMM.GroundNode[i - 1, j - 1] == 1)
                                {
                                    G.DrawImage(GetTile(1, 24, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else
                                {
                                    G.DrawImage(GetTile(3, 26, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 5:
                            {
                                if (ModuleSMM.GroundNode[i, j - 1] == 2)
                                {
                                    G.DrawImage(GetTile(2, 24, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else if (ModuleSMM.GroundNode[i + 1, j - 1] == 1)
                                {
                                    G.DrawImage(GetTile(0, 24, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else
                                {
                                    G.DrawImage(GetTile(2, 26, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 12:
                            {
                                if (ModuleSMM.GroundNode[i, j + 1] == 19)
                                {
                                    G.DrawImage(GetTile(0, 33, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else if (ModuleSMM.GroundNode[i - 1, j + 1] == 1)
                                {
                                    G.DrawImage(GetTile(2, 31, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else
                                {
                                    G.DrawImage(GetTile(5, 31, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 13:
                            {
                                if (ModuleSMM.GroundNode[i, j + 1] == 18)
                                {
                                    G.DrawImage(GetTile(3, 33, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else if (ModuleSMM.GroundNode[i + 1, j + 1] == 1)
                                {
                                    G.DrawImage(GetTile(1, 31, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else
                                {
                                    G.DrawImage(GetTile(4, 31, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 14:
                            {
                                if (ModuleSMM.GroundNode[i, j - 1] == 17)
                                {
                                    G.DrawImage(GetTile(2, 32, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else if (ModuleSMM.GroundNode[i - 1, j - 1] == 1)
                                {
                                    G.DrawImage(GetTile(2, 28, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else
                                {
                                    G.DrawImage(GetTile(5, 28, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 15:
                            {
                                if (ModuleSMM.GroundNode[i, j - 1] == 16)
                                {
                                    G.DrawImage(GetTile(1, 32, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else if (ModuleSMM.GroundNode[i + 1, j - 1] == 1)
                                {
                                    G.DrawImage(GetTile(1, 28, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }
                                else
                                {
                                    G.DrawImage(GetTile(4, 28, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                }

                                break;
                            }

                        case 16:
                            {
                                G.DrawImage(GetTile(3, 31, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                break;
                            }

                        case 17:
                            {
                                G.DrawImage(GetTile(0, 31, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                break;
                            }

                        case 18:
                            {
                                G.DrawImage(GetTile(3, 28, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                break;
                            }

                        case 19:
                            {
                                G.DrawImage(GetTile(0, 28, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                                break;
                            }

                        default:
                            {
                                break;
                            }
                            // G.DrawString(GroundNode(i, j), Me.Font, Brushes.Black, (i - 1) * Zm, (H - j) * Zm)
                    }
                    // G.DrawString(GroundNode(i, j), Me.Font, Brushes.Black, (i - 1) * Zm, (H - j) * Zm)
                    // G.DrawString(GetCorCode(i, j).X & "," & GetCorCode(i, j).Y, Me.Font, Brushes.Black, (i - 1) * Zm, (H - j) * Zm + 10)
                }
            }

            var loopTo2 = W;
            for (i = 0; i <= loopTo2; i++)
            {
                var loopTo3 = H;
                for (j = 0; j <= loopTo3; j++)
                {
                    if (ModuleSMM.GroundNode[i, j] == 1)
                    {
                        R = GetCorCode(i, j);
                        G.DrawImage(GetTile(R.X, R.Y, 1, 1), (i - 1) * Zm, (H - j) * Zm, Zm, Zm);
                        // G.DrawString(GroundNode(i, j), Me.Font, Brushes.Black, (i - 1) * Zm, (H - j) * Zm)
                        // G.DrawString(GetCorCode(i, j).X & "," & GetCorCode(i, j).Y, Me.Font, Brushes.Black, (i - 1) * Zm, (H - j) * Zm + 10)

                    }
                }
            }
        }

        public void ReGrdCode()
        {
            int i;
            int j;
            int m;


            // 斜坡
            int H;
            int W;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            int CX, CY;
            // GrdType
            // 0无 1方 2陡左上 3陡右上 4陡左下 5陡右下 6端左上 7端右上 8端左下 9端右下
            // 10  11  12缓大左上 13缓大右上 14缓大左下 15缓大右下  16缓小左上 17缓小右上 18缓小左下 19缓小右下
            // 20端左上 21端右上 22端左下 23端右下 24    25    26
            var loopTo = ModuleSMM.MapHdr.ObjCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                switch (ModuleSMM.MapObj[i].ID)
                {
                    case 87:
                        {
                            // 缓坡
                            CX = (int)Math.Round(-0.5d + ModuleSMM.MapObj[i].X / 160d);
                            CY = (int)Math.Round(-0.5d + ModuleSMM.MapObj[i].Y / 160d);
                            if (ModuleSMM.MapObj[i].Flag / 0x100000 % 0x2 == 0)
                            {
                                // 左斜
                                switch (ModuleSMM.GroundNode[CX + 1, CY + 1])
                                {
                                    case 0:
                                        {
                                            ModuleSMM.GroundNode[CX + 1, CY + 1] = 23;
                                            break;
                                        }

                                    case 6:
                                    case 20:
                                    case 8:
                                    case 22:
                                        {
                                            ModuleSMM.GroundNode[CX + 1, CY + 1] = 1;
                                            break;
                                        }
                                }

                                switch (ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + ModuleSMM.MapObj[i].H])
                                {
                                    case 0:
                                        {
                                            ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + ModuleSMM.MapObj[i].H] = 20;
                                            break;
                                        }

                                    case 9:
                                    case 23:
                                    case 7:
                                    case 21:
                                        {
                                            ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + ModuleSMM.MapObj[i].H] = 1;
                                            break;
                                        }
                                }
                                // GroundNode(CX + 1, CY + 1) = 23
                                // GroundNode(CX + MapObj(i).W, CY + MapObj(i).H) = 20
                                var loopTo1 = ModuleSMM.MapObj[i].W - 2;
                                for (j = 1; j <= loopTo1; j += 2)
                                {
                                    ModuleSMM.GroundNode[CX + 1 + j, CY + 1 + j / 2 + 1] = 19;
                                    switch (ModuleSMM.GroundNode[CX + 1 + j, CY + 1 + j / 2])
                                    {
                                        case 0:
                                        case 6:
                                        case 20:
                                            {
                                                ModuleSMM.GroundNode[CX + 1 + j, CY + 1 + j / 2] = 12;
                                                break;
                                            }
                                    }
                                    // GroundNode(CX + 1 + j, CY + 1 + (j \ 2) + 1) = 12
                                    switch (ModuleSMM.GroundNode[CX + 2 + j, CY + 1 + j / 2 + 1])
                                    {
                                        case 0:
                                        case 9:
                                        case 23:
                                            {
                                                ModuleSMM.GroundNode[CX + 2 + j, CY + 1 + j / 2 + 1] = 15;
                                                break;
                                            }
                                    }
                                    // GroundNode(CX + 2 + j, CY + 1 + (j \ 2) + 2) = 15
                                    ModuleSMM.GroundNode[CX + 2 + j, CY + 1 + j / 2] = 16;
                                }
                            }
                            else
                            {
                                // 右斜
                                switch (ModuleSMM.GroundNode[CX + 1, CY + ModuleSMM.MapObj[i].H])
                                {
                                    case 0:
                                        {
                                            ModuleSMM.GroundNode[CX + 1, CY + ModuleSMM.MapObj[i].H] = 21;
                                            break;
                                        }

                                    case 6:
                                    case 8:
                                    case 20:
                                    case 22:
                                        {
                                            ModuleSMM.GroundNode[CX + 1, CY + ModuleSMM.MapObj[i].H] = 1;
                                            break;
                                        }
                                }

                                switch (ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + 1])
                                {
                                    case 0:
                                        {
                                            ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + 1] = 22;
                                            break;
                                        }

                                    case 9:
                                    case 7:
                                    case 23:
                                    case 21:
                                        {
                                            ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + 1] = 1;
                                            break;
                                        }
                                }
                                // GroundNode(CX + 1, CY + MapObj(i).H) = 21
                                // GroundNode(CX + MapObj(i).W, CY + 1) = 22
                                var loopTo2 = ModuleSMM.MapObj[i].W - 2;
                                for (j = 1; j <= loopTo2; j += 2)
                                {
                                    ModuleSMM.GroundNode[CX + 1 + j, CY + ModuleSMM.MapObj[i].H - j / 2 - 1] = 17;
                                    switch (ModuleSMM.GroundNode[CX + 1 + j, CY + ModuleSMM.MapObj[i].H - j / 2])
                                    {
                                        case 0:
                                        case 22:
                                        case 8:
                                            {
                                                ModuleSMM.GroundNode[CX + 1 + j, CY + ModuleSMM.MapObj[i].H - j / 2] = 14;
                                                break;
                                            }
                                    }
                                    // GroundNode(CX + 1 + j, CY + MapObj(i).H - (j \ 2)) = 14
                                    switch (ModuleSMM.GroundNode[CX + 2 + j, CY + ModuleSMM.MapObj[i].H - j / 2 - 1])
                                    {
                                        case 0:
                                        case 21:
                                        case 7:
                                            {
                                                ModuleSMM.GroundNode[CX + 2 + j, CY + ModuleSMM.MapObj[i].H - j / 2 - 1] = 13;
                                                break;
                                            }
                                    }
                                    // GroundNode(CX + 2 + j, CY + MapObj(i).H - (j \ 2) - 1) = 13
                                    ModuleSMM.GroundNode[CX + 2 + j, CY + ModuleSMM.MapObj[i].H - j / 2] = 18;
                                }
                            }

                            break;
                        }

                    case 88:
                        {
                            // 陡坡
                            CX = (int)Math.Round(-0.5d + ModuleSMM.MapObj[i].X / 160d);
                            CY = (int)Math.Round(-0.5d + ModuleSMM.MapObj[i].Y / 160d);
                            if (ModuleSMM.MapObj[i].Flag / 0x100000 % 0x2 == 0)
                            {
                                // 左斜
                                switch (ModuleSMM.GroundNode[CX + 1, CY + 1])
                                {
                                    case 0:
                                        {
                                            ModuleSMM.GroundNode[CX + 1, CY + 1] = 9;
                                            break;
                                        }

                                    case 6:
                                    case 8:
                                    case 20:
                                    case 22:
                                        {
                                            ModuleSMM.GroundNode[CX + 1, CY + 1] = 1;
                                            break;
                                        }
                                }

                                switch (ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + ModuleSMM.MapObj[i].H])
                                {
                                    case 0:
                                        {
                                            ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + ModuleSMM.MapObj[i].H] = 6;
                                            break;
                                        }

                                    case 9:
                                    case 7:
                                    case 23:
                                    case 21:
                                        {
                                            ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + ModuleSMM.MapObj[i].H] = 1;
                                            break;
                                        }
                                }
                                // GroundNode(CX + 1, CY + 1) = 9
                                // GroundNode(CX + MapObj(i).W, CY + MapObj(i).H) = 6
                                var loopTo3 = ModuleSMM.MapObj[i].W - 2;
                                for (j = 1; j <= loopTo3; j++)
                                {
                                    // GroundNode(CX + 1 + j, CY + 1 + j) = 5
                                    switch (ModuleSMM.GroundNode[CX + 1 + j, CY + 1 + j])
                                    {
                                        case 0:
                                        case 9:
                                        case 23:
                                            {
                                                ModuleSMM.GroundNode[CX + 1 + j, CY + 1 + j] = 5;
                                                break;
                                            }
                                    }
                                    // GroundNode(CX + 1 + j, CY + j) = 2
                                    switch (ModuleSMM.GroundNode[CX + 1 + j, CY + j])
                                    {
                                        case 0:
                                        case 6:
                                        case 20:
                                            {
                                                ModuleSMM.GroundNode[CX + 1 + j, CY + j] = 2;
                                                break;
                                            }
                                    }
                                }
                            }
                            else
                            {
                                // 右斜
                                switch (ModuleSMM.GroundNode[CX + 1, CY + ModuleSMM.MapObj[i].H])
                                {
                                    case 0:
                                        {
                                            ModuleSMM.GroundNode[CX + 1, CY + ModuleSMM.MapObj[i].H] = 7;
                                            break;
                                        }

                                    case 6:
                                    case 8:
                                    case 20:
                                    case 22:
                                        {
                                            ModuleSMM.GroundNode[CX + 1, CY + ModuleSMM.MapObj[i].H] = 1;
                                            break;
                                        }
                                }

                                switch (ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + 1])
                                {
                                    case 0:
                                        {
                                            ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + 1] = 8;
                                            break;
                                        }

                                    case 9:
                                    case 7:
                                    case 23:
                                    case 21:
                                        {
                                            ModuleSMM.GroundNode[CX + ModuleSMM.MapObj[i].W, CY + 1] = 1;
                                            break;
                                        }
                                }

                                // GroundNode(CX + MapObj(i).W, CY + 1) = 8
                                // GroundNode(CX + 1, CY + MapObj(i).W) = 7
                                var loopTo4 = ModuleSMM.MapObj[i].W - 2;
                                for (j = 1; j <= loopTo4; j++)
                                {
                                    // GroundNode(CX + 1 + j, CY + MapObj(i).W - j) = 4
                                    switch (ModuleSMM.GroundNode[CX + 1 + j, CY + ModuleSMM.MapObj[i].W - j])
                                    {
                                        case 0:
                                        case 8:
                                        case 22:
                                            {
                                                ModuleSMM.GroundNode[CX + 1 + j, CY + ModuleSMM.MapObj[i].W - j] = 4;
                                                break;
                                            }
                                    }
                                    // GroundNode(CX + 1 + j, CY + MapObj(i).W - 1 - j) = 3
                                    switch (ModuleSMM.GroundNode[CX + 1 + j, CY + ModuleSMM.MapObj[i].W - 1 - j])
                                    {
                                        case 0:
                                        case 7:
                                        case 21:
                                            {
                                                ModuleSMM.GroundNode[CX + 1 + j, CY + ModuleSMM.MapObj[i].W - 1 - j] = 3;
                                                break;
                                            }
                                    }
                                }
                            }

                            break;
                        }
                }
            }
            // GrdType
            // 0无 1方 2陡左上 3陡右上 4陡左下 5陡右下 6端左上 7端右上 8端左下 9端右下
            // 10  11  12缓大左上 13缓大右上 14缓大左下 15缓大右下  16缓小左上 17缓小右上 18缓小左下 19缓小右下
            // 20端左上 21端右上 22端左下 23端右下 24    25    26
            for (m = 0; m <= 2; m++)
            {
                var loopTo5 = W;
                for (i = 0; i <= loopTo5; i++)
                {
                    var loopTo6 = H;
                    for (j = 0; j <= loopTo6; j++)
                    {
                        switch (ModuleSMM.GroundNode[i + 1, j + 1])
                        {
                            case 2:
                            case 16: // UL
                                {
                                    if (ModuleSMM.GroundNode[i + 2, j + 1] > 0 | ModuleSMM.GroundNode[i + 1, j] > 0)
                                        ModuleSMM.GroundNode[i + 1, j + 1] = 1;
                                    break;
                                }

                            case 3:
                            case 17: // UR
                                {
                                    if (ModuleSMM.GroundNode[i, j + 1] > 0 | ModuleSMM.GroundNode[i + 1, j] > 0)
                                        ModuleSMM.GroundNode[i + 1, j + 1] = 1;
                                    break;
                                }

                            case 4:
                            case 18: // DL
                                {
                                    if (ModuleSMM.GroundNode[i + 2, j + 1] > 0 | ModuleSMM.GroundNode[i + 1, j + 2] > 0)
                                        ModuleSMM.GroundNode[i + 1, j + 1] = 1;
                                    break;
                                }

                            case 5:
                            case 19: // DR
                                {
                                    if (ModuleSMM.GroundNode[i, j + 1] > 0 | ModuleSMM.GroundNode[i + 1, j + 2] > 0)
                                        ModuleSMM.GroundNode[i + 1, j + 1] = 1;
                                    break;
                                }

                            case 12:
                                {
                                    if (ModuleSMM.GroundNode[i + 1, j] > 0 | ModuleSMM.GroundNode[i + 2, j + 1] == 1)
                                        ModuleSMM.GroundNode[i + 1, j + 1] = 1;
                                    break;
                                }

                            case 13:
                                {
                                    if (ModuleSMM.GroundNode[i + 1, j] > 0 | ModuleSMM.GroundNode[i, j + 1] == 1)
                                        ModuleSMM.GroundNode[i + 1, j + 1] = 1;
                                    break;
                                }

                            case 14:
                                {
                                    if (ModuleSMM.GroundNode[i + 1, j + 2] > 0 | ModuleSMM.GroundNode[i + 2, j + 1] == 1)
                                        ModuleSMM.GroundNode[i + 1, j + 1] = 1;
                                    break;
                                }

                            case 15:
                                {
                                    if (ModuleSMM.GroundNode[i + 1, j + 2] > 0 | ModuleSMM.GroundNode[i, j + 1] == 1)
                                        ModuleSMM.GroundNode[i + 1, j + 1] = 1;
                                    break;
                                }
                        }
                    }
                }
            }
        }

        public void DrawGrd(bool IO)
        {
            int i;
            Image K;
            K = GetTile(0, 12, 1, 1); // Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG")
            if (IO)
            {
                // 终点
                switch (ModuleSMM.LH.GameStyle)
                {
                    case 12621: // 1
                        {
                            if (ModuleSMM.MapHdr.Theme == 2)
                            {
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27A.PNG"), (float)((ModuleSMM.LH.GoalX / 10d - 0.5d) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 4) * Zm, Zm * 2, Zm * 4);
                                for (i = 0; i <= 13; i++)
                                    G.DrawImage(GetTile(15, 15, 1, 1), (float)((ModuleSMM.LH.GoalX / 10d - 14.5d + i) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY) * Zm, Zm, Zm);
                            }
                            else
                            {
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27.PNG"), (float)((ModuleSMM.LH.GoalX / 10d - 0.5d) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 11) * Zm, Zm, Zm * 11);
                            }

                            break;
                        }

                    case 13133: // 3
                        {
                            if (ModuleSMM.MapHdr.Theme == 2)
                            {
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27A.PNG"), (float)((ModuleSMM.LH.GoalX / 10d - 0.5d) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 4) * Zm, Zm * 2, Zm * 4);
                                for (i = 0; i <= 13; i++)
                                    G.DrawImage(GetTile(15, 15, 1, 1), (float)((ModuleSMM.LH.GoalX / 10d - 14.5d + i) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY) * Zm, Zm, Zm);
                            }
                            else
                            {
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27.PNG"), (float)((ModuleSMM.LH.GoalX / 10d - 0.5d) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 5) * Zm, Zm * 2, Zm * 2);
                            }

                            break;
                        }

                    case 22349: // W
                        {
                            if (ModuleSMM.MapHdr.Theme == 2)
                            {
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27A.PNG"), (float)((ModuleSMM.LH.GoalX / 10d - 0.5d) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 4) * Zm, Zm * 2, Zm * 4);
                                for (i = 0; i <= 13; i++)
                                    G.DrawImage(GetTile(15, 15, 1, 1), (float)((ModuleSMM.LH.GoalX / 10d - 14.5d + i) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY) * Zm, Zm, Zm);
                            }
                            else
                            {
                                // For i = 1 To 8
                                // G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27C.PNG"), CSng((LH.GoalX / 10 - 0.5) * Zm), (MapHdr.BorT \ 16 - LH.GoalY - i) * Zm, Zm, Zm)
                                // G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27E.PNG"), CSng((LH.GoalX / 10 + 1.5) * Zm), (MapHdr.BorT \ 16 - LH.GoalY - i) * Zm, Zm, Zm)
                                // Next
                                // G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27B.PNG"), CSng((LH.GoalX / 10 - 0.5) * Zm), (MapHdr.BorT \ 16 - LH.GoalY - 9) * Zm, Zm, Zm)
                                // G.DrawImage(Image.FromFile(PT & "\img\" & LH.GameStyle.ToString & "\obj\27D.PNG"), CSng((LH.GoalX / 10 + 1.5) * Zm), (MapHdr.BorT \ 16 - LH.GoalY - 9) * Zm, Zm, Zm)

                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27F.PNG"), (float)((ModuleSMM.LH.GoalX / 10d - 0.5d) * Zm), (float)(ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 8.5d) * Zm, Zm, Zm * 9);
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27.PNG"), (float)(ModuleSMM.LH.GoalX / 10d * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 8) * (float)Zm, Zm * 2, Zm);
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27G.PNG"), (float)((ModuleSMM.LH.GoalX / 10d + 1.5d) * Zm), (float)(ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 8.5d) * Zm, Zm, Zm * 9);
                            }

                            break;
                        }

                    case 21847: // U
                        {
                            if (ModuleSMM.MapHdr.Theme == 2)
                            {
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27A.PNG"), (float)((ModuleSMM.LH.GoalX / 10d - 0.5d) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 4) * Zm, Zm * 2, Zm * 4);
                                for (i = 0; i <= 13; i++)
                                    G.DrawImage(GetTile(15, 15, 1, 1), (float)((ModuleSMM.LH.GoalX / 10d - 14.5d + i) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY) * Zm, Zm, Zm);
                            }
                            else
                            {
                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27.PNG"), (float)((ModuleSMM.LH.GoalX / 10d - 0.5d) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 11) * Zm, Zm, Zm * 11);
                            }

                            break;
                        }

                    case 22323: // 3D
                        {
                            G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\27.PNG"), (float)((ModuleSMM.LH.GoalX / 10d - 0.5d) * Zm), (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.GoalY - 11) * Zm, Zm, Zm * 11);
                            break;
                        }
                }

                // 旧代码弃用
                // '终点地面 10*LH.GoalY 
                // For j = (LH.GoalX - 5) / 10 To (LH.GoalX - 5) / 10 + 9
                // For i = 0 To LH.GoalY - 1
                // R = GrdLoc(GetGrdType(GetGrdCode(j, i)))
                // G.DrawImage(GetTile(R.X, R.Y, 1, 1), j * Zm, (MapHdr.BorT \ 16 - 1) * Zm - i * Zm, Zm, Zm)
                // Next
                // Next

                // '起点地面 7*LH.StartY
                // For j = 0 To 6
                // For i = 0 To LH.StartY - 1
                // R = GrdLoc(GetGrdType(GetGrdCode(j, i)))
                // G.DrawImage(GetTile(R.X, R.Y, 1, 1), j * Zm, (MapHdr.BorT \ 16 - 1) * Zm - i * Zm, Zm, Zm)
                // Next
                // Next

                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\38.PNG"), 1 * Zm, (ModuleSMM.MapHdr.BorT / 16 - ModuleSMM.LH.StartY - 3) * Zm, Zm * 3, Zm * 3);
            }

            // '地面
            // For i = 0 To MapHdr.GroundCount - 1
            // R = GrdLoc(GetGrdType(GetGrdCode(MapGrd(i).X, MapGrd(i).Y)))
            // G.DrawImage(GetTile(R.X, R.Y, 1, 1), MapGrd(i).X * Zm, (MapHdr.BorT \ 16 - 1) * Zm - MapGrd(i).Y * Zm, Zm, Zm)
            // Next
        }

        public string GetGrdCode(int x, int y)
        {
            string GetGrdCodeRet = default;
            string S = "";
            // C
            S = Conversions.ToString(S + Interaction.IIf(ModuleSMM.GroundNode[x, y + 2] == 0, "0", "1"));
            S = Conversions.ToString(S + Interaction.IIf(ModuleSMM.GroundNode[x + 2, y + 2] == 0, "0", "1"));
            S = Conversions.ToString(S + Interaction.IIf(ModuleSMM.GroundNode[x, y] == 0, "0", "1"));
            S = Conversions.ToString(S + Interaction.IIf(ModuleSMM.GroundNode[x + 2, y] == 0, "0", "1"));

            // E ULRD
            S = Conversions.ToString(S + Interaction.IIf(ModuleSMM.GroundNode[x + 1, y + 2] == 0, "0", "1"));
            S = Conversions.ToString(S + Interaction.IIf(ModuleSMM.GroundNode[x, y + 1] == 0, "0", "1"));
            S = Conversions.ToString(S + Interaction.IIf(ModuleSMM.GroundNode[x + 2, y + 1] == 0, "0", "1"));
            S = Conversions.ToString(S + Interaction.IIf(ModuleSMM.GroundNode[x + 1, y] == 0, "0", "1"));
            GetGrdCodeRet = S;
            return GetGrdCodeRet;
        }

        public int GetGrdType(string A)
        {
            int GetGrdTypeRet = default;
            int p;
            int i;
            p = 0;
            for (i = 0; i <= 7; i++)
                p = Conversions.ToInteger(p + ((Strings.Mid(A, 8 - i, 1) == "0"? 0: 1)* Math.Pow(2d, (double)i)));
            GetGrdTypeRet = p;
            return GetGrdTypeRet;
        }

        public void SetGrdLoc()
        {
            string GL = @"0D,4D,1D,AD,3D,9D,2D,CD,6D,5D,8D,ED,7D,DD,BD,FD,
							0D,4D,1D,2F,3D,9D,2D,4E,6D,5D,8D,0E,7D,DD,BD,8E,
							0D,4D,1D,AD,3D,4F,2D,5E,6D,5D,8D,ED,7D,1E,BD,9E,
							0D,4D,1D,2F,3D,4F,2D,3F,6D,5D,8D,0E,7D,1E,BD,CE,
							0D,4D,1D,AD,3D,9D,2D,CD,6D,5D,8F,2E,7D,DD,6E,AE,
							0D,4D,1D,2F,3D,9D,2D,4E,6D,5D,8F,5F,7D,DD,6E,EE,
							0D,4D,1D,AD,3D,4F,2D,5E,6D,5D,8F,2E,AF,1E,6E,1F,
							0D,4D,1D,2F,3D,4F,2D,3F,6D,5D,8F,5F,7D,1E,6E,BF,
							0D,4D,1D,AD,3D,9D,2D,CD,6D,5D,8D,ED,AF,3E,7E,BE,
							0D,4D,1D,2F,3D,9D,2D,4E,6D,5D,8D,0E,7D,3E,7E,0F,
							0D,4D,1D,AD,3D,4F,2D,5E,6D,5D,8D,ED,AF,7F,7E,FE,
							0D,4D,1D,2F,3D,4F,2D,3F,6D,5D,8D,0E,AF,7F,7E,CF,
							0D,4D,1D,AD,3D,9D,2D,CD,6D,5D,8F,2E,AF,3E,9F,DE,
							0D,4D,1D,2F,3D,9D,2D,4E,6D,5D,8F,5F,AF,3E,9F,DF,
							0D,4D,1D,AD,3D,4F,2D,5E,6D,5D,8F,2E,AF,7F,9F,EF,
							0D,4D,1D,2F,3D,4F,2D,3F,6D,5D,8F,5F,AF,7F,9F,6F";
            var GS = GL.Split(',');
            int I;
            for (I = 0; I <= 255; I++)
            {
                ModuleSMM.GrdLoc[I].X = (int)Math.Round(Conversion.Val("&H" + Strings.Left(GS[I], 1)));
                ModuleSMM.GrdLoc[I].Y = (int)Math.Round(Conversion.Val("&H" + Strings.Right(GS[I], 1)));
            }
        }

        private int NowIO;

        public void DrawItem(string K, bool L)
        {
            // On Error Resume Next
            int i;
            int j;
            int j2;
            int H;
            int W;
            string PR;
            int LX = default, LY = default, KX, KY = default;
            int PP;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            string P = ModuleSMM.PT;
            var loopTo = ModuleSMM.MapHdr.ObjCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                PR = "";
                if (Strings.InStr(K, "/" + ModuleSMM.MapObj[i].ID.ToString() + "/") > 0)
                {
                    if (ModuleSMM.MapObj[i].ID == 105)
                    {
                        if (ModuleSMM.MapObj[i].Flag / 0x400 % 2 == 1)
                        {
                            KY = 0;
                        }
                        else
                        {
                            KY = -3 * Zm;
                        }

                        ModuleSMM.ObjLinkType[ModuleSMM.MapObj[i].LID + 1] = 105;
                        if (ModuleSMM.MapObj[i].Flag / 0x80 % 2 == 1)
                        {
                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\105A.PNG"), (float)(-1.5d + ModuleSMM.MapObj[i].X / 160d) * Zm, H * Zm - (float)(0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm + KY, Zm * 3, Zm * 5);
                        }
                        else
                        {
                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\105.PNG"), (float)(-1.5d + ModuleSMM.MapObj[i].X / 160d) * Zm, H * Zm - (float)(0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm + KY, Zm * 3, Zm * 5);
                        }
                        // CID
                        if (ModuleSMM.MapObj[i].CID != -1)
                        {
                            // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.Tostring & "\CID\C.PNG"),
                            // CSNG((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm),
                            // H * Zm - CSNG((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm),
                            // Zm, Zm)
                            if (ModuleSMM.MapObj[i].CFlag / 0x4 % 2 == 1)
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + "A.PNG"), LX, LY + KY, Zm, Zm);
                            }
                            else
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + ".PNG"), LX, LY + KY, Zm, Zm);
                            }
                        }
                    }
                    else
                    {
                        switch (ModuleSMM.ObjLinkType[ModuleSMM.MapObj[i].LID + 1])
                        {
                            case 9: // 管道L
                                {
                                    KX = 0;
                                    KY = (int)Math.Round((Math.Min(ModuleSMM.MapObj[i].W, ModuleSMM.MapObj[i].H) - 1) / 2d * Zm);
                                    break;
                                }

                            case 105: // 夹子L
                                {
                                    KX = 0;
                                    KY = (int)Math.Round(-Zm / 4d);
                                    break;
                                }

                            case 59: // 轨道
                                {
                                    KX = 0;
                                    KY = (int)Math.Round((Math.Min(ModuleSMM.MapObj[i].W, ModuleSMM.MapObj[i].H) - 1) / 2d * Zm);
                                    break;
                                }

                            case 31:
                                {
                                    KX = 0;
                                    KY = 0; // 3 * Zm
                                    break;
                                }

                            case 106: // 树
                                {
                                    KX = 0;
                                    KY = 0;
                                    break;
                                }

                            case 0:
                                {
                                    KX = 0;
                                    KY = 0;
                                    break;
                                }
                        }

                        if (ModuleSMM.MapObj[i].LID + 1 == 0 & !L | ModuleSMM.MapObj[i].LID + 1 > 0 & L | ModuleSMM.MapObj[i].ID == 9)
                        {
                            switch (ModuleSMM.MapObj[i].ID)
                            {
                                case 89: // 卷轴相机
                                    {
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\CMR\1.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 14:
                                    {
                                        // 蘑菇平台
                                        if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                        {
                                            j2 = 3;
                                        }
                                        else if (ModuleSMM.MapObj[i].Flag / 0x80000 % 2 == 1)
                                        {
                                            j2 = 4;
                                        }
                                        else
                                        {
                                            j2 = 2;
                                        }

                                        var loopTo1 = ModuleSMM.MapObj[i].W - 1;
                                        for (j = 0; j <= loopTo1; j++)
                                        {
                                            if (j == 0)
                                            {
                                                G.DrawImage(GetTile(3, j2, 1, 1), (j + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                            }
                                            else if (j == ModuleSMM.MapObj[i].W - 1)
                                            {
                                                G.DrawImage(GetTile(5, j2, 1, 1), (j + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                            }
                                            else
                                            {
                                                G.DrawImage(GetTile(4, j2, 1, 1), (j + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                            }
                                        }

                                        break;
                                    }

                                case 16:
                                    {
                                        // 半碰撞地形
                                        if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                        {
                                            j2 = 10;
                                        }
                                        else if (ModuleSMM.MapObj[i].Flag / 0x80000 % 2 == 1)
                                        {
                                            j2 = 13;
                                        }
                                        else
                                        {
                                            j2 = 7;
                                        }

                                        var loopTo2 = ModuleSMM.MapObj[i].W - 1;
                                        for (j = 0; j <= loopTo2; j++)
                                        {
                                            if (j == 0)
                                            {
                                                G.DrawImage(GetTile(j2, 3, 1, 1), (j + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                            }
                                            else if (j == ModuleSMM.MapObj[i].W - 1)
                                            {
                                                G.DrawImage(GetTile(j2 + 2, 3, 1, 1), (j + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                            }
                                            else
                                            {
                                                G.DrawImage(GetTile(j2 + 1, 3, 1, 1), (j + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                            }
                                        }

                                        break;
                                    }

                                case 71:
                                    {
                                        // 3D半碰撞地形
                                        string TL, TM, TR;
                                        var loopTo3 = ModuleSMM.MapObj[i].H - 1;
                                        for (j2 = 0; j2 <= loopTo3; j2++)
                                        {
                                            switch (j2)
                                            {
                                                case 0:
                                                    {
                                                        TL = "71";
                                                        TM = "71A";
                                                        TR = "71B";
                                                        break;
                                                    }

                                                case var @case when @case == ModuleSMM.MapObj[i].H - 1:
                                                    {
                                                        TL = "71F";
                                                        TM = "71G";
                                                        TR = "71H";
                                                        break;
                                                    }

                                                default:
                                                    {
                                                        TL = "71C";
                                                        TM = "71D";
                                                        TR = "71E";
                                                        break;
                                                    }
                                            }

                                            var loopTo4 = ModuleSMM.MapObj[i].W - 1;
                                            for (j = 0; j <= loopTo4; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + TL + ".PNG"), (j + ModuleSMM.MapObj[i].X / 160) * Zm, (H + j2) * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                                }
                                                else if (j == ModuleSMM.MapObj[i].W - 1)
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + TR + ".PNG"), (j + ModuleSMM.MapObj[i].X / 160) * Zm, (H + j2) * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                                }
                                                else
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + TM + ".PNG"), (j + ModuleSMM.MapObj[i].X / 160) * Zm, (H + j2) * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                                }
                                            }
                                        }

                                        break;
                                    }

                                case 17:
                                    {
                                        // 桥
                                        var loopTo5 = ModuleSMM.MapObj[i].W - 1;
                                        for (j = 0; j <= loopTo5; j++)
                                        {
                                            if (j == 0)
                                            {
                                                G.DrawImage(GetTile(0, 2, 1, 2), (j + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm * 2);
                                            }
                                            else if (j == ModuleSMM.MapObj[i].W - 1)
                                            {
                                                G.DrawImage(GetTile(2, 2, 1, 2), (j + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm * 2);
                                            }
                                            else
                                            {
                                                G.DrawImage(GetTile(1, 2, 1, 2), (j + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm * 2);
                                            }
                                        }

                                        break;
                                    }

                                case 113:
                                case 132:
                                    {
                                        // 蘑菇跳台 开关跳台
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            var loopTo6 = ModuleSMM.MapObj[i].W - 1;
                                            for (j = 0; j <= loopTo6; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + "D.PNG"), (float)((j - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                                }
                                                else if (j == ModuleSMM.MapObj[i].W - 1)
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + "E.PNG"), (float)((j - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                                }
                                                else
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + "C.PNG"), (float)((j - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var loopTo7 = ModuleSMM.MapObj[i].W - 1;
                                            for (j = 0; j <= loopTo7; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + "A.PNG"), (float)((j - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                                }
                                                else if (j == ModuleSMM.MapObj[i].W - 1)
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + "B.PNG"), (float)((j - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                                }
                                                else
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + ".PNG"), (float)((j - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                                }
                                            }
                                        }

                                        break;
                                    }

                                case 66:
                                case 67:
                                case 90:
                                    {
                                        // 箭头 单向板 中间旗 
                                        switch (ModuleSMM.MapObj[i].Flag)
                                        {
                                            case 0x6000040:
                                                {
                                                    PR = "";
                                                    break;
                                                }

                                            case 0x6400040:
                                                {
                                                    PR = "A";
                                                    break;
                                                }

                                            case 0x6800040:
                                                {
                                                    PR = "B";
                                                    break;
                                                }

                                            case 0x6C00040:
                                                {
                                                    PR = "C";
                                                    break;
                                                }

                                            case 0x7000040:
                                                {
                                                    PR = "D";
                                                    break;
                                                }

                                            case 0x7400040:
                                                {
                                                    PR = "E";
                                                    break;
                                                }

                                            case 0x7800040:
                                                {
                                                    PR = "F";
                                                    break;
                                                }

                                            case 0x7C00040:
                                                {
                                                    PR = "G";
                                                    break;
                                                }
                                        }

                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H / 2d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 83: // 狼牙棒
                                    {
                                        switch (ModuleSMM.MapObj[i].Flag)
                                        {
                                            case 0x6000040:
                                                {
                                                    PR = "";
                                                    break;
                                                }

                                            case 0x6400040:
                                                {
                                                    PR = "A";
                                                    break;
                                                }

                                            case 0x6800040:
                                                {
                                                    PR = "B";
                                                    break;
                                                }

                                            case 0x6C00040:
                                                {
                                                    PR = "C";
                                                    break;
                                                }

                                            case 0x7000040:
                                                {
                                                    PR = "D";
                                                    break;
                                                }

                                            case 0x7400040:
                                                {
                                                    PR = "E";
                                                    break;
                                                }

                                            case 0x7800040:
                                                {
                                                    PR = "F";
                                                    break;
                                                }

                                            case 0x7C00040:
                                                {
                                                    PR = "G";
                                                    break;
                                                }
                                        }

                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H / 2d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                        G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), 0.7d), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 64:
                                    {
                                        // 藤蔓
                                        var loopTo8 = (int)ModuleSMM.MapObj[i].H;
                                        for (j = 1; j <= loopTo8; j++)
                                        {
                                            if (j == 1)
                                            {
                                                G.DrawImage(GetTile(13, 7, 1, 1), (float)(-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm, H * Zm - (float)((j + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                            }
                                            else if (j == ModuleSMM.MapObj[i].H)
                                            {
                                                G.DrawImage(GetTile(15, 7, 1, 1), (float)(-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm, H * Zm - (float)((j + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                            }
                                            else
                                            {
                                                G.DrawImage(GetTile(14, 7, 1, 1), (float)(-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm, H * Zm - (float)((j + ModuleSMM.MapObj[i].Y / 160) * Zm), Zm, Zm);
                                            }
                                        }

                                        break;
                                    }

                                case 4:
                                case 5:
                                case 6:
                                case 21:
                                case 22:
                                case 23:
                                case 29:
                                case 63:
                                case 79:
                                case 99:
                                case 100:
                                case 43:
                                case 8:
                                    {
                                        // 4,4A 5 6 8 8A 21 22 23 23A 29 43 49 63 79 79A 92 99 100 100A
                                        // 砖 问号 硬砖 地面 竹轮 云砖 刺 金币
                                        // 音符  隐藏 
                                        // 冰砖  P砖 开关 开关砖

                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PP = 1;
                                        }
                                        else
                                        {
                                            PP = 0;
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(GetTile(ModuleSMM.TileLoc[ModuleSMM.MapObj[i].ID, PP].X, ModuleSMM.TileLoc[ModuleSMM.MapObj[i].ID, PP].Y, 1, 1), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 108:
                                    {
                                        // 闪烁砖
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 106: // 树
                                    {
                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H + 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\106.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * 4, Zm * 4);
                                        var loopTo9 = ModuleSMM.MapObj[i].H - 1;
                                        for (j = 4; j <= loopTo9; j++)
                                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\106A.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + 1.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + j) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm, Zm);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\106B.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + 1d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((-0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * 2, Zm);
                                        break;
                                    }

                                case 85:
                                case 119:
                                    {
                                        // 机动砖 轨道砖
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        DrawMoveBlock((byte)ModuleSMM.MapObj[i].ID, (byte)ModuleSMM.MapObj[i].Ex, ModuleSMM.MapObj[i].X, ModuleSMM.MapObj[i].Y);
                                        break;
                                    }

                                case 94:
                                    {
                                        // 斜传送带
                                        Point C1, C2;
                                        if (ModuleSMM.MapObj[i].Flag / 0x400000 % 2 == 0)
                                        {
                                            C1 = new Point(8, 0);
                                            C2 = new Point(4, 16);
                                        }
                                        else
                                        {
                                            C1 = new Point(13, 24);
                                            C2 = new Point(10, 22);
                                        }

                                        if (ModuleSMM.MapObj[i].Flag / 0x200000 % 0x2 == 0)
                                        {
                                            // 左斜
                                            LX = (int)Math.Round((float)((-1 + ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                            LY = (int)Math.Round((H - 0.5d - ModuleSMM.MapObj[i].H / 2d) * Zm - (float)((-0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                            G.DrawImage(GetTile(C1.X, C1.Y, 1, 1), (float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 1) * Zm - (float)((-0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            G.DrawImage(GetTile(C1.X + 2, C1.Y, 1, 1), (float)((ModuleSMM.MapObj[i].W - 1.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 1) * Zm - (float)((ModuleSMM.MapObj[i].H - 1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            var loopTo10 = ModuleSMM.MapObj[i].W - 2;
                                            for (j = 1; j <= loopTo10; j++)
                                                G.DrawImage(GetTile(C2.X + 1, C2.Y, 1, 2), (float)((j - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 1) * Zm - (float)((j - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm * 2);
                                        }
                                        else
                                        {
                                            // 右斜
                                            LX = (int)Math.Round((float)((-1 + ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                            LY = (int)Math.Round((H - 0.5d - ModuleSMM.MapObj[i].H / 2d) * Zm - (float)((-0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                            G.DrawImage(GetTile(C1.X, C1.Y, 1, 1), (float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 1) * Zm - (float)((ModuleSMM.MapObj[i].H - 1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            G.DrawImage(GetTile(C1.X + 2, C1.Y, 1, 1), (float)((ModuleSMM.MapObj[i].W - 1.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 1) * Zm - (float)((-0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            var loopTo11 = ModuleSMM.MapObj[i].W - 2;
                                            for (j = 1; j <= loopTo11; j++)
                                                G.DrawImage(GetTile(C2.X + 4, C2.Y, 1, 2), (float)((j - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 1) * Zm - (float)((-0.5d - j + ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm * 2);
                                        }

                                        if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 0)
                                        {
                                            if (ModuleSMM.MapObj[i].Flag / 0x8 % 2 == 1)
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\CMN\A1.PNG"), LX, LY, Zm, Zm);
                                            }
                                            else
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\CMN\A0.PNG"), LX, LY, Zm, Zm);
                                            }
                                        }
                                        else if (ModuleSMM.MapObj[i].Flag / 0x8 % 2 == 1)
                                        {
                                            G.DrawImage(Image.FromFile(P + @"\img\CMN\A3.PNG"), LX, LY, Zm, Zm);
                                        }
                                        else
                                        {
                                            G.DrawImage(Image.FromFile(P + @"\img\CMN\A2.PNG"), LX, LY, Zm, Zm);
                                        }

                                        break;
                                    }

                                // Case 87
                                // '缓坡
                                // If (MapObj(i).Flag \ &H100000) Mod &H2 = 0 Then
                                // '左斜
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                // CSng((-0.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((-0.5 + MapObj(i).Y / 160) * Zm), Zm, Zm)
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                // CSng((MapObj(i).W - 1.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((MapObj(i).H - 1.5 + MapObj(i).Y / 160) * Zm), Zm, Zm)
                                // For j = 1 To MapObj(i).W - 2 Step 2
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\87.PNG"),
                                // CSng((j - 0.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((j / 2 + MapObj(i).Y / 160) * Zm), Zm * 2, Zm * 2)
                                // Next
                                // Else
                                // '右斜
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                // CSng((-0.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((MapObj(i).H - 1.5 + MapObj(i).Y / 160) * Zm), Zm, Zm)
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                // CSng((MapObj(i).W - 1.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((-0.5 + MapObj(i).Y / 160) * Zm), Zm, Zm)
                                // For j = 1 To MapObj(i).W - 2 Step 2
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\87A.PNG"),
                                // CSng((j - 0.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((-j / 2 + MapObj(i).H - 1 + MapObj(i).Y / 160) * Zm), Zm * 2, Zm * 2)
                                // Next
                                // End If
                                // Case 88
                                // '陡坡
                                // If (MapObj(i).Flag \ &H100000) Mod &H2 = 0 Then
                                // '左斜
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                // CSng((-0.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((-0.5 + MapObj(i).Y / 160) * Zm), Zm, Zm)
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                // CSng((MapObj(i).W - 1.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((MapObj(i).H - 1.5 + MapObj(i).Y / 160) * Zm), Zm, Zm)
                                // For j = 1 To MapObj(i).W - 2
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\88.PNG"),
                                // CSng((j - 0.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((j - 0.5 + MapObj(i).Y / 160) * Zm), Zm, Zm * 2)
                                // Next
                                // Else
                                // '右斜
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                // CSng((-0.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((MapObj(i).H - 1.5 + MapObj(i).Y / 160) * Zm), Zm, Zm)
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\7.PNG"),
                                // CSng((MapObj(i).W - 1.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((-0.5 + MapObj(i).Y / 160) * Zm), Zm, Zm)
                                // For j = 1 To MapObj(i).W - 2
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\88A.PNG"),
                                // CSng((j - 0.5 + MapObj(i).X / 160) * Zm),
                                // (H - 1) * Zm - CSng((-0.5 - j + MapObj(i).H + MapObj(i).Y / 160) * Zm), Zm, Zm * 2)
                                // Next
                                // End If
                                case 53:
                                    {
                                        // 传送带
                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                        Point C1;
                                        if (ModuleSMM.MapObj[i].Flag / 0x400000 % 2 == 0)
                                        {
                                            C1 = new Point(8, 0);
                                        }
                                        else
                                        {
                                            C1 = new Point(13, 24);
                                        }

                                        var loopTo12 = ModuleSMM.MapObj[i].W - 1;
                                        for (j = 0; j <= loopTo12; j++)
                                        {
                                            if (j == 0)
                                            {
                                                G.DrawImage(GetTile(C1.X, C1.Y, 1, 1), (float)((j - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            }
                                            else if (j == ModuleSMM.MapObj[i].W - 1)
                                            {
                                                G.DrawImage(GetTile(C1.X + 2, C1.Y, 1, 1), (float)((j - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            }
                                            else
                                            {
                                                G.DrawImage(GetTile(C1.X + 1, C1.Y, 1, 1), (float)((j - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            }

                                            if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 0)
                                            {
                                                if (ModuleSMM.MapObj[i].Flag / 0x8 % 2 == 1)
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\CMN\A1.PNG"), LX + (int)Math.Round((-0.5d + ModuleSMM.MapObj[i].W / 2d) * Zm), LY, Zm, Zm);
                                                }
                                                else
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\CMN\A0.PNG"), LX + (int)Math.Round((-0.5d + ModuleSMM.MapObj[i].W / 2d) * Zm), LY, Zm, Zm);
                                                }
                                            }
                                            else if (ModuleSMM.MapObj[i].Flag / 0x8 % 2 == 1)
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\CMN\A3.PNG"), LX + (int)Math.Round((-0.5d + ModuleSMM.MapObj[i].W / 2d) * Zm), LY, Zm, Zm);
                                            }
                                            else
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\CMN\A2.PNG"), LX + (int)Math.Round((-0.5d + ModuleSMM.MapObj[i].W / 2d) * Zm), LY, Zm, Zm);
                                            }
                                        }

                                        break;
                                    }

                                case 9:
                                    {
                                        // 管道
                                        ModuleSMM.ObjLinkType[ModuleSMM.MapObj[i].LID + 1] = 9;
                                        // 0绿 4红 8蓝 C橙
                                        PP = ModuleSMM.MapObj[i].Flag / 0x10000 % 0x10 / 4;
                                        // Select Case (MapObj(i).Flag \ &H10000) Mod &H10
                                        // Case &H0
                                        // PR = "9"
                                        // Case &H4
                                        // PR = "9R"
                                        // Case &H8
                                        // PR = "9U"
                                        // Case &HC
                                        // PR = "9P"
                                        // End Select
                                        // 00右 20左 40上 60下
                                        // 以相对左下角为准
                                        switch (ModuleSMM.MapObj[i].Flag % 0x80)
                                        {
                                            case 0x0: // R
                                                {
                                                    LX = (int)Math.Round((float)((ModuleSMM.MapObj[i].H - 1 - 1 - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                    LY = (int)Math.Round(H * Zm - (float)(ModuleSMM.MapObj[i].Y / 160d * Zm));
                                                    var loopTo13 = ModuleSMM.MapObj[i].H - 2;
                                                    for (j = 0; j <= loopTo13; j++)
                                                        G.DrawImage(GetTile(ModuleSMM.PipeLoc[PP, 4].X, ModuleSMM.PipeLoc[PP, 4].Y, 1, 2), (float)((j - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, 2 * Zm);
                                                    G.DrawImage(GetTile(ModuleSMM.PipeLoc[PP, 3].X, ModuleSMM.PipeLoc[PP, 3].Y, 1, 2), (float)((j - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, 2 * Zm);
                                                    break;
                                                }

                                            case 0x20: // L
                                                {
                                                    LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].H + 1 + 1 - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                    LY = (int)Math.Round(H * Zm - (float)((1d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                                    var loopTo14 = ModuleSMM.MapObj[i].H - 2;
                                                    for (j = 0; j <= loopTo14; j++)
                                                        G.DrawImage(GetTile(ModuleSMM.PipeLoc[PP, 4].X, ModuleSMM.PipeLoc[PP, 4].Y, 1, 2), (float)((-j - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, 2 * Zm);
                                                    G.DrawImage(GetTile(ModuleSMM.PipeLoc[PP, 2].X, ModuleSMM.PipeLoc[PP, 2].Y, 1, 2), (float)((-j - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, 2 * Zm);
                                                    break;
                                                }

                                            case 0x40: // U
                                                {
                                                    LX = (int)Math.Round((float)(+ModuleSMM.MapObj[i].X / 160d * Zm));
                                                    LY = (int)Math.Round((H - ModuleSMM.MapObj[i].H + 1 + 1) * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                                    var loopTo15 = ModuleSMM.MapObj[i].H - 2;
                                                    for (j = 0; j <= loopTo15; j++)
                                                        G.DrawImage(GetTile(ModuleSMM.PipeLoc[PP, 5].X, ModuleSMM.PipeLoc[PP, 5].Y, 2, 1), (float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - j) * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), 2 * Zm, Zm);
                                                    G.DrawImage(GetTile(ModuleSMM.PipeLoc[PP, 0].X, ModuleSMM.PipeLoc[PP, 0].Y, 2, 1), (float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - j) * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), 2 * Zm, Zm);
                                                    break;
                                                }

                                            case 0x60: // D
                                                {
                                                    LX = (int)Math.Round((float)((-1 + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                    LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H - 1 - 1) * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                                    var loopTo16 = ModuleSMM.MapObj[i].H - 2;
                                                    for (j = 0; j <= loopTo16; j++)
                                                        G.DrawImage(GetTile(ModuleSMM.PipeLoc[PP, 5].X, ModuleSMM.PipeLoc[PP, 5].Y, 2, 1), (float)((-1.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + j) * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), 2 * Zm, Zm);
                                                    G.DrawImage(GetTile(ModuleSMM.PipeLoc[PP, 1].X, ModuleSMM.PipeLoc[PP, 1].Y, 2, 1), (float)((-1.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + j) * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), 2 * Zm, Zm);
                                                    break;
                                                }
                                        }

                                        PR = (ModuleSMM.MapObj[i].Flag % 0x1000000 / 0x100000 - 1).ToString();
                                        if (PR != "-1")
                                        {
                                            G.DrawImage(Image.FromFile(P + @"\img\CMN\C" + PR + ".PNG"), LX, LY, Zm, Zm);
                                        }

                                        break;
                                    }

                                case 55:
                                    {
                                        // 门
                                        if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else if (ModuleSMM.MapObj[i].Flag / 0x80000 % 2 == 1)
                                        {
                                            PR = "B";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\55" + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        PR = (ModuleSMM.MapObj[i].Flag % 0x800000 / 0x200000).ToString();
                                        G.DrawImage(Image.FromFile(P + @"\img\CMN\C" + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + 1) * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                        break;
                                    }

                                case 97:
                                    {
                                        // 传送箱
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\97" + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        PR = (ModuleSMM.MapObj[i].Flag % 0x800000 / 0x200000).ToString();
                                        G.DrawImage(Image.FromFile(P + @"\img\CMN\C" + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                        break;
                                    }

                                case 84:
                                    {
                                        // 蛇
                                        var loopTo17 = ModuleSMM.MapObj[i].W - 1;
                                        for (j = 0; j <= loopTo17; j++)
                                        {
                                            if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\84A.PNG"), (float)((j - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            }
                                            else
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\84.PNG"), (float)((j - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            }
                                        }
                                        // &H10方向
                                        DrawSnake((byte)ModuleSMM.MapObj[i].Ex, ModuleSMM.MapObj[i].X, ModuleSMM.MapObj[i].Y, ModuleSMM.MapObj[i].W, ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 68:
                                case 82:
                                    {
                                        // 齿轮 甜甜圈

                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H - 1.5d) * Zm - (float)(ModuleSMM.MapObj[i].Y / 160d * Zm) + KY);
                                        G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + ".PNG"), 0.7d), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 0:
                                case 10:
                                case 15:
                                case 19:
                                case 20:
                                case 35:
                                case 48:
                                case 56:
                                case 57:
                                case 60:
                                case 76:
                                case 92:
                                case 95:
                                case 102:
                                case 72:
                                case 50:
                                case 51:
                                case 65:
                                case 80:
                                case 114:
                                case var case1 when case1 == 119:
                                case 77:
                                case 104:
                                case 120:
                                case 121:
                                case 122:
                                case 123:
                                case 124:
                                case 125:
                                case 126:
                                case 112:
                                case 127:
                                case 128:
                                case 129:
                                case 130:
                                case 131:
                                case 96:
                                case 117:
                                case 86:
                                    {
                                        // 板栗  金币 弹簧 炸弹 P POW 蘑菇 
                                        // 无敌星 鱿鱼 鱼
                                        // 黑花 火球  风  红币 钥匙  地鼠 慢慢龟汽车 跳跳怪 跳跳鼠 蜜蜂 冲刺砖块 尖刺鱼 !方块
                                        // 奔奔  太阳 库巴七人 木箱 纸糊道具
                                        // 蚂蚁 斗斗 乌卡
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 33:
                                    {
                                        // 1UP 
                                        if (ModuleSMM.MapHdr.Theme == 0 & ModuleSMM.MapHdr.Flag == 2)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 74:
                                    {
                                        // 加邦 
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            if (ModuleSMM.MapHdr.Theme == 6)
                                            {
                                                PR = "B";
                                            }
                                            else
                                            {
                                                PR = "A";
                                            }
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 42:
                                    {
                                        // 飞机
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1 || ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H - 2) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + ModuleSMM.MapObj[i].H - 2) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * 2, Zm * 2);
                                        break;
                                    }

                                case 34:
                                    {
                                        // 火花 
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                            {
                                                PR = "C";
                                            }
                                            else
                                            {
                                                PR = "A";
                                            }
                                        }
                                        else if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                        {
                                            PR = "B";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 81:
                                case 116:
                                    {
                                        // USA  锤子

                                        if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 44:
                                    {
                                        // 大蘑菇

                                        if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 12:
                                    {
                                        // 咚咚
                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2d - 0.5d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\12.PNG"), 0.7d), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        if (ModuleSMM.MapObj[i].LID == -1)
                                        {
                                            switch (ModuleSMM.MapObj[i].Flag % 0x100)
                                            {
                                                case 0x40:
                                                case 0x42:
                                                case 0x44:
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\CMN\E1.PNG"), LX, LY, Zm, Zm);
                                                        break;
                                                    }

                                                case 0x48:
                                                case 0x4A:
                                                case 0x4C:
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\CMN\E2.PNG"), LX, LY, Zm, Zm);
                                                        break;
                                                    }

                                                case 0x50:
                                                case 0x52:
                                                case 0x54:
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\CMN\E0.PNG"), LX, LY, Zm, Zm);
                                                        break;
                                                    }

                                                case 0x58:
                                                case 0x5A:
                                                case 0x5C:
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\CMN\E3.PNG"), LX, LY, Zm, Zm);
                                                        break;
                                                    }
                                            }
                                        }

                                        break;
                                    }

                                case 41:
                                    {
                                        // 幽灵
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        switch (ModuleSMM.LH.GameStyle)
                                        {
                                            case 22323:
                                                {
                                                    if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\41D.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    }
                                                    else
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\41.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    }

                                                    break;
                                                }

                                            default:
                                                {
                                                    if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\41A.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    }
                                                    else if (ModuleSMM.MapObj[i].Flag / 0x1000000 % 0x8 == 0x4)
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\41C.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    }
                                                    else if (ModuleSMM.MapObj[i].Flag / 0x100 % 2 == 1)
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\41B.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    }
                                                    else
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\41.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    }

                                                    break;
                                                }
                                        }

                                        break;
                                    }

                                case 28:
                                case 25:
                                case 18:
                                    {
                                        // 钢盔 刺龟 P
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + "A.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        }
                                        else if (ModuleSMM.MapObj[i].Flag / 0x1000000 % 8 == 0x6)
                                        {
                                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        }
                                        else
                                        {
                                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + "B.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        }

                                        break;
                                    }

                                case 40:
                                    {
                                        // 小刺龟
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].W) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            switch (ModuleSMM.MapObj[i].Flag / 0x1000000 % 8)
                                            {
                                                // 方向6上 4下 0左 2右
                                                case 0x0: // L
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\40B0.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                        break;
                                                    }

                                                case 0x2: // R
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\40B2.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                        break;
                                                    }

                                                case 0x4: // D
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\40B4.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                        break;
                                                    }

                                                case 0x6: // U
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\40B6.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                        break;
                                                    }
                                            }
                                        }
                                        else
                                        {
                                            switch (ModuleSMM.MapObj[i].Flag / 0x1000000 % 8)
                                            {
                                                // 方向6上 4下 0左 2右
                                                case 0x0: // L
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\40A0.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                        break;
                                                    }

                                                case 0x2: // R
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\40A2.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                        break;
                                                    }

                                                case 0x4: // D
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\40A4.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                        break;
                                                    }

                                                case 0x6: // U
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\40A6.PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                        break;
                                                    }
                                            }
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        break;
                                    }

                                case 2:
                                    {
                                        // 绿花
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "2B";
                                        }
                                        else
                                        {
                                            PR = "2A";
                                        }

                                        switch (ModuleSMM.MapObj[i].Flag / 0x1000000 % 0x8)
                                        {
                                            // 方向6上 4下 0左 2右
                                            case 0x0: // L
                                                {
                                                    LX = (int)Math.Round((float)((ModuleSMM.MapObj[i].H / 2d - 1d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                    LY = (int)Math.Round((H + ModuleSMM.MapObj[i].W + ModuleSMM.MapObj[i].W / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + PR + "0.PNG"), (float)((-ModuleSMM.MapObj[i].W * 3 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + ModuleSMM.MapObj[i].W) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W * 2, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }

                                            case 0x2: // R
                                                {
                                                    LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                    LY = (int)Math.Round((H + ModuleSMM.MapObj[i].W + ModuleSMM.MapObj[i].W / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + PR + "2.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + ModuleSMM.MapObj[i].W) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W * 2, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }

                                            case 0x4: // D
                                                {
                                                    LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                    LY = (int)Math.Round((H + ModuleSMM.MapObj[i].W) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + PR + "4.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + ModuleSMM.MapObj[i].W) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H * 2);
                                                    break;
                                                }

                                            case 0x6: // U
                                                {
                                                    LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                    LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].W / 2) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + PR + "6.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H * 2);
                                                    break;
                                                }
                                        }

                                        break;
                                    }

                                case 107:
                                    {
                                        // 长长吞食花
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "E";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        switch (ModuleSMM.MapObj[i].Flag / 0x1000000)
                                        {
                                            case 0x0:
                                                {
                                                    PR += "C";
                                                    break;
                                                }

                                            case 0x2:
                                                {
                                                    PR += "A";
                                                    break;
                                                }

                                            case 0x4:
                                                {
                                                    PR += "B";
                                                    break;
                                                }

                                            case 0x6:
                                                {
                                                    PR += "";
                                                    break;
                                                }
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\107" + PR + ".PNG"), LX, LY, Zm * 2, Zm * 2);
                                        DrawCrp((byte)ModuleSMM.MapObj[i].Ex, ModuleSMM.MapObj[i].X, ModuleSMM.MapObj[i].Y);
                                        break;
                                    }

                                case 32:
                                    {
                                        // 大炮弹

                                        switch (ModuleSMM.MapObj[i].Flag)
                                        {
                                            case 0x6000040:
                                                {
                                                    PR = "";
                                                    break;
                                                }

                                            case 0x6400040:
                                                {
                                                    PR = "A";
                                                    break;
                                                }

                                            case 0x6800040:
                                                {
                                                    PR = "B";
                                                    break;
                                                }

                                            case 0x6C00040:
                                                {
                                                    PR = "C";
                                                    break;
                                                }

                                            case 0x6000044:
                                                {
                                                    PR = "D";
                                                    break;
                                                }

                                            case 0x6400044:
                                                {
                                                    PR = "E";
                                                    break;
                                                }

                                            case 0x6800044:
                                                {
                                                    PR = "F";
                                                    break;
                                                }

                                            case 0x6C00044:
                                                {
                                                    PR = "G";
                                                    break;
                                                }

                                            case 0x7000040:
                                                {
                                                    PR = "H";
                                                    break;
                                                }
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\32" + PR + ".PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 1:
                                case 46:
                                case 52:
                                case 58:
                                    {
                                        // 慢慢龟，碎碎龟，花花，扳手
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H * 2);
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        break;
                                    }

                                case 30:
                                    {
                                        // 裁判
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 1 + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\30.PNG"), LX, LY, Zm, Zm * 2);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\31.PNG"), LX - Zm / 2, LY + Zm / 2, Zm * 2, Zm);
                                        break;
                                    }

                                case 31:
                                    {
                                        // 裁判云
                                        ModuleSMM.ObjLinkType[ModuleSMM.MapObj[i].LID + 1] = 31;
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d - 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\31.PNG"), LX, LY, Zm * 2, Zm);
                                        break;
                                    }

                                case 45: // 鞋 耀西
                                    {
                                        switch (ModuleSMM.LH.GameStyle)
                                        {
                                            case 21847:
                                            case 22349: // U W
                                                {
                                                    if (ModuleSMM.MapObj[i].W == 2)
                                                    {
                                                        PR = "A";
                                                    }
                                                    else
                                                    {
                                                        PR = "";
                                                    }

                                                    break;
                                                }

                                            default:
                                                {
                                                    if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                                    {
                                                        PR = "A";
                                                    }
                                                    else
                                                    {
                                                        PR = "";
                                                    }

                                                    break;
                                                }
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H * 2);
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].W / 2 / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + ModuleSMM.MapObj[i].H / 2 / 2d) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        break;
                                    }

                                case 62:
                                    {
                                        // 库巴
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        switch (ModuleSMM.LH.GameStyle)
                                        {
                                            case 22323:
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + "A.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }

                                            default:
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }
                                        }

                                        break;
                                    }

                                case 3:
                                    {
                                        // 德莱文
                                        switch (ModuleSMM.LH.GameStyle)
                                        {
                                            case 22323:
                                                {
                                                    if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                                    {
                                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                        LY = (int)Math.Round(H * Zm - (float)((1d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\3B.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    }
                                                    else
                                                    {
                                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\3.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + 2) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    }

                                                    break;
                                                }

                                            default:
                                                {
                                                    if (ModuleSMM.MapObj[i].Flag / 0x4000 % 2 == 1)
                                                    {
                                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                        LY = (int)Math.Round(H * Zm - (float)((1d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\3A.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    }
                                                    else
                                                    {
                                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 1.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\3.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, 2 * Zm * ModuleSMM.MapObj[i].H);
                                                    }

                                                    break;
                                                }
                                        }

                                        break;
                                    }

                                case 13:
                                    {
                                        // 炮台
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "B";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\13" + PR + ".PNG"), 0.7d), LX, LY, Zm * ModuleSMM.MapObj[i].W, Zm * 2);
                                        var loopTo18 = ModuleSMM.MapObj[i].H - 1;
                                        for (j = 2; j <= loopTo18; j++)
                                        {
                                            if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                            {
                                                G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\13C.PNG"), 0.7d), LX, LY + j * Zm, Zm, Zm);
                                            }
                                            else
                                            {
                                                G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\13A.PNG"), 0.7d), LX, LY + j * Zm, Zm, Zm);
                                            }
                                        }

                                        break;
                                    }

                                case 39:
                                    {
                                        // 魔法师
                                        LX = (int)Math.Round((float)((2d - ModuleSMM.MapObj[i].W / 2d - ModuleSMM.MapObj[i].W + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + 1) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\39.PNG"), LX - Zm - Zm, LY - Zm + KY, 2 * Zm * ModuleSMM.MapObj[i].W + KY, 2 * Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 47:
                                    {
                                        // 小炮
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "E";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        float ANG;
                                        string D;
                                        switch (ModuleSMM.MapObj[i].Flag / 0x100000)
                                        {
                                            case 0x0:
                                            case 0x4:
                                            case 0x8:
                                            case 0xC:
                                            case 0x10:
                                                {
                                                    D = "D";
                                                    break;
                                                }

                                            case 0x2:
                                            case 0x24:
                                            case 0x28:
                                            case 0x2C:
                                            case 0x30:
                                                {
                                                    D = "B";
                                                    break;
                                                }

                                            case 0x40:
                                            case 0x44:
                                            case 0x48:
                                            case 0x4C:
                                            case 0x50:
                                                {
                                                    D = "C";
                                                    break;
                                                }

                                            case 0x60:
                                            case 0x64:
                                            case 0x68:
                                            case 0x6C:
                                            case 0x70:
                                                {
                                                    D = "A";
                                                    break;
                                                }

                                            default:
                                                {
                                                    D = "A";
                                                    break;
                                                }
                                        }

                                        switch (ModuleSMM.MapObj[i].Flag / 0x100000)
                                        {
                                            // UDLR
                                            case 0xC:
                                            case 0x30:
                                            case 0x64:
                                                {
                                                    ANG = 0f;
                                                    break;
                                                }

                                            case 0x10:
                                            case 0x2C:
                                            case 0x44:
                                                {
                                                    ANG = 180f;
                                                    break;
                                                }

                                            case 0x4:
                                            case 0x4C:
                                            case 0x70:
                                                {
                                                    ANG = 270f;
                                                    break;
                                                }

                                            case 0x24:
                                            case 0x50:
                                            case 0x6C:
                                                {
                                                    ANG = 90f;
                                                    break;
                                                }
                                            // UL UR DL DR
                                            case 0x8:
                                            case 0x60:
                                                {
                                                    ANG = 315f;
                                                    break;
                                                }

                                            case 0x20:
                                            case 0x68:
                                                {
                                                    ANG = 45f;
                                                    break;
                                                }

                                            case 0x0:
                                            case 0x48:
                                                {
                                                    ANG = 225f;
                                                    break;
                                                }

                                            case 0x28:
                                            case 0x40:
                                                {
                                                    ANG = 135f;
                                                    break;
                                                }

                                            default:
                                                {
                                                    ANG = 0f;
                                                    break;
                                                }
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                        G.TranslateTransform(LX + ModuleSMM.MapObj[i].W * Zm / 2, LY + ModuleSMM.MapObj[i].H * Zm / 2);
                                        G.RotateTransform(ANG);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\47" + PR + ".PNG"), -ModuleSMM.MapObj[i].W * Zm / 2, -ModuleSMM.MapObj[i].H * Zm / 2, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        G.RotateTransform(-ANG);
                                        G.TranslateTransform(-LX - ModuleSMM.MapObj[i].W * Zm / 2, -LY - ModuleSMM.MapObj[i].H * Zm / 2);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\47" + PR + D + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 61:
                                    {
                                        // 汪汪
                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 0)
                                        {
                                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\61A.PNG"), (float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm, Zm);
                                        }

                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\61.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 78:
                                    {
                                        // 仙人掌
                                        LX = (int)Math.Round((float)(-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm);
                                        LY = (int)Math.Round((H + 1) * Zm - (float)((ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160) * Zm) + KY);
                                        var loopTo19 = ModuleSMM.MapObj[i].H - 1;
                                        for (j = 0; j <= loopTo19; j++)
                                        {
                                            if (j == ModuleSMM.MapObj[i].H - 1)
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\78.PNG"), (float)(-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm, (H - 1) * Zm - (float)((j + ModuleSMM.MapObj[i].Y / 160) * Zm) + KY, Zm, Zm);
                                            }
                                            else
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\78A.PNG"), (float)(-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm, (H - 1) * Zm - (float)((j + ModuleSMM.MapObj[i].Y / 160) * Zm) + KY, Zm, Zm);
                                            }
                                        }

                                        break;
                                    }

                                case 111:
                                    {
                                        // 机械库巴
                                        if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                        {
                                            PR = "B";
                                        }
                                        else if (ModuleSMM.MapObj[i].Flag / 0x80000 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W + 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + 1) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\111" + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, 2 * Zm * ModuleSMM.MapObj[i].W, 2 * Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 70:
                                    {
                                        // 大金币 
                                        if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else if (ModuleSMM.MapObj[i].Flag / 0x80000 % 2 == 1)
                                        {
                                            PR = "B";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W / 2d + 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 1 + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        break;
                                    }

                                case 110:
                                    {
                                        // 刺方块
                                        if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else if (ModuleSMM.MapObj[i].Flag / 0x80000 % 2 == 1)
                                        {
                                            PR = "B";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\" + ModuleSMM.MapObj[i].ID.ToString() + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 98:
                                    {
                                        // 小库巴
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W + 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round((H + 1) * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\98" + PR + ".PNG"), (float)((-ModuleSMM.MapObj[i].W + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H * 2 - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, 2 * Zm * ModuleSMM.MapObj[i].W, 2 * Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 103:
                                    {
                                        // 骨鱼
                                        LX = (int)Math.Round((float)((-ModuleSMM.MapObj[i].W + 0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\103.PNG"), (float)((-ModuleSMM.MapObj[i].W + ModuleSMM.MapObj[i].X / 160d) * Zm) + KY, LY, 2 * Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                        break;
                                    }

                                case 91:
                                    {
                                        // 跷跷板
                                        var loopTo20 = ModuleSMM.MapObj[i].W - 1;
                                        for (j = 0; j <= loopTo20; j++)
                                        {
                                            if (j == 0)
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\91A.PNG"), (j - ModuleSMM.MapObj[i].W / 2 + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            }
                                            else if (j == ModuleSMM.MapObj[i].W - 1)
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\91B.PNG"), (j - ModuleSMM.MapObj[i].W / 2 + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            }
                                            else
                                            {
                                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\91.PNG"), (j - ModuleSMM.MapObj[i].W / 2 + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                            }
                                        }

                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\91C.PNG"), (float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm, Zm);
                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                                        break;
                                    }

                                case 36:
                                    {
                                        // 熔岩台
                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                                        {
                                            PR = "A";
                                        }
                                        else
                                        {
                                            PR = "";
                                        }

                                        if (ModuleSMM.MapObj[i].LID != -1)
                                        {
                                            ModuleSMM.MapObj[i].W = 1;
                                        }

                                        var loopTo21 = ModuleSMM.MapObj[i].W - 1;
                                        for (j = 0; j <= loopTo21; j++)
                                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\36" + PR + ".PNG"), (j - ModuleSMM.MapObj[i].W / 2 + ModuleSMM.MapObj[i].X / 160) * Zm, H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm, Zm);
                                        LX = (int)Math.Round((float)((j - 1 - ModuleSMM.MapObj[i].W / 2 + ModuleSMM.MapObj[i].X / 160) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        break;
                                    }

                                case 11:
                                    {
                                        // 升降台
                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        var loopTo22 = ModuleSMM.MapObj[i].W - 1;
                                        for (j = 0; j <= loopTo22; j++)
                                        {
                                            if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 0)
                                            {
                                                if (j == 0)
                                                {
                                                    PR = "A";
                                                }
                                                else if (j == ModuleSMM.MapObj[i].W - 1)
                                                {
                                                    PR = "B";
                                                }
                                                else
                                                {
                                                    PR = "";
                                                }

                                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\11" + PR + ".PNG"), (float)((j - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm, Zm);
                                            }
                                            else
                                            {
                                                if (j == 0)
                                                {
                                                    PR = "D";
                                                }
                                                else if (j == ModuleSMM.MapObj[i].W - 1)
                                                {
                                                    PR = "E";
                                                }
                                                else
                                                {
                                                    PR = "C";
                                                }

                                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\11" + PR + ".PNG"), (float)((j - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm, Zm);
                                            }
                                        }

                                        // 'If MapObj(i).LID >= 0 Then
                                        // '	'PR = ((MapObj(i).Flag Mod &H400000) \ &H100000).ToString
                                        // '	'G.DrawImage(Image.FromFile(P & "\img\CMN\D" & PR & ".PNG"), LX, LY, Zm, Zm)
                                        // 'ELSE IF    ///END IF

                                        if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 0)
                                        {
                                            switch (ModuleSMM.MapObj[i].Flag % 0x100)
                                            {
                                                case 0x40:
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\CMN\D1.PNG"), LX, LY, Zm, Zm);
                                                        break;
                                                    }

                                                case 0x48:
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\CMN\D2.PNG"), LX, LY, Zm, Zm);
                                                        break;
                                                    }

                                                case 0x50:
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\CMN\D0.PNG"), LX, LY, Zm, Zm);
                                                        break;
                                                    }

                                                case 0x58:
                                                    {
                                                        G.DrawImage(Image.FromFile(P + @"\img\CMN\D3.PNG"), LX, LY, Zm, Zm);
                                                        break;
                                                    }
                                            }
                                        }

                                        break;
                                    }

                                case 54:
                                    {
                                        // 喷枪
                                        LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                                        LY = (int)Math.Round(H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY);
                                        switch (ModuleSMM.MapObj[i].Flag % 0x100)
                                        {
                                            case 0x40:
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }
                                            // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A1.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), (H - 3) * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, 3 * Zm * MapObj(i).H)
                                            case 0x48:
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54A.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }
                                            // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A3.PNG"), CSng((-MapObj(i).W / 2 + 1 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, 3 * Zm * MapObj(i).W, Zm * MapObj(i).H)
                                            case 0x50:
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54B.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }
                                            // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A5.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), (H + 1) * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, 3 * Zm * MapObj(i).H)
                                            case 0x58:
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54C.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }
                                            // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A7.PNG"), CSng((-MapObj(i).W / 2 - 3 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, 3 * Zm * MapObj(i).W, Zm * MapObj(i).H)
                                            case 0x44:
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }
                                            // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A2.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), (H - 3) * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, 3 * Zm * MapObj(i).H)
                                            case 0x4C:
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54A.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }
                                            // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A4.PNG"), CSng((-MapObj(i).W / 2 + 1 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, 3 * Zm * MapObj(i).W, Zm * MapObj(i).H)
                                            case 0x54:
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54B.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }
                                            // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A6.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), (H + 1) * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, 3 * Zm * MapObj(i).H)
                                            case 0x5C:
                                                {
                                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54C.PNG"), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm) + KY, Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                                    break;
                                                }
                                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A8.PNG"), CSng((-MapObj(i).W / 2 - 3 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, 3 * Zm * MapObj(i).W, Zm * MapObj(i).H)
                                        }

                                        break;
                                    }

                                case 24:
                                    {
                                        // 火棍
                                        LX = (int)Math.Round((float)(-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm);
                                        LY = (int)Math.Round(H * Zm - (float)(ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm);
                                        G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\24.PNG"), LX, LY, Zm, Zm);
                                        break;
                                    }

                                case 105:
                                    {
                                        // 夹子
                                        if (ModuleSMM.MapObj[i].Flag % 0x400 >= 0x100)
                                        {
                                            KY = Zm * 3;
                                            ModuleSMM.ObjLinkType[ModuleSMM.MapObj[i].LID + 1] = 105;
                                        }
                                        else
                                        {
                                            KY = 0;
                                            ModuleSMM.ObjLinkType[ModuleSMM.MapObj[i].LID + 1] = 105;
                                        }

                                        LX = (int)Math.Round((float)(-1.5d + ModuleSMM.MapObj[i].X / 160d) * Zm);
                                        LY = (int)Math.Round(H * Zm - (float)(3.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm + KY);
                                        if (ModuleSMM.MapObj[i].Flag / 0x80 % 2 == 1)
                                        {
                                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\105A.PNG"), LX, LY, Zm * 3, Zm * 5);
                                        }
                                        else
                                        {
                                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\105.PNG"), LX, LY, Zm * 3, Zm * 5);
                                        }

                                        break;
                                    }
                            }

                            PR = "";
                            // PR += IIf((MapObj(i).Flag \ &H4000) Mod 2 = 1, "M", "")
                            PR = Conversions.ToString(PR + Interaction.IIf(ModuleSMM.MapObj[i].Flag / 0x8000 % 2 == 1, "P", ""));
                            PR = Conversions.ToString(PR + Interaction.IIf(ModuleSMM.MapObj[i].Flag / 2 % 2 == 1, "W", ""));
                            if (PR == "PW")
                                PR = "B";
                            if (PR.Length > 0)
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + PR + ".PNG"), LX, LY, Zm / 2, Zm / 2);
                            }

                            if (L & ModuleSMM.ObjLinkType[ModuleSMM.MapObj[i].LID + 1] == 59)
                            {
                                PR = (ModuleSMM.MapObj[i].Flag % 0x400000 / 0x100000).ToString();
                                G.DrawImage(Image.FromFile(P + @"\img\CMN\D" + PR + ".PNG"), LX, LY, Zm, Zm);
                            }
                        }
                    }
                }
            }
        }

        public void DrawCID()
        {
            int i;
            int H;
            int W;
            string PR;
            int LX, LY;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            string P = ModuleSMM.PT;
            var loopTo = ModuleSMM.MapHdr.ObjCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                LY = (int)Math.Round(H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                switch (ModuleSMM.MapObj[i].CID)
                {
                    case -1:// 无
                        {
                            break;
                        }

                    case 44:
                    case 81:
                    case 116: // 状态
                        {
                            if (ModuleSMM.MapObj[i].CFlag / 0x40000 % 2 == 1)
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + "A.PNG"), LX, LY, Zm, Zm);
                            }
                            else
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + ".PNG"), LX, LY, Zm, Zm);
                            }

                            G.DrawImage(Image.FromFile(P + @"\img\CMN\F1.PNG"), LX, LY, Zm, Zm);
                            break;
                        }

                    case 34: // 状态火花
                        {
                            if (ModuleSMM.MapObj[i].CFlag / 0x4 % 2 == 1)
                            {
                                if (ModuleSMM.MapObj[i].CFlag / 0x40000 % 2 == 1)
                                {
                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + "C.PNG"), LX, LY, Zm, Zm);
                                }
                                else
                                {
                                    G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + "A.PNG"), LX, LY, Zm, Zm);
                                }
                            }
                            else if (ModuleSMM.MapObj[i].CFlag / 0x40000 % 2 == 1)
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + "B.PNG"), LX, LY, Zm, Zm);
                            }
                            else
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + ".PNG"), LX, LY, Zm, Zm);
                            }

                            G.DrawImage(Image.FromFile(P + @"\img\CMN\F1.PNG"), LX, LY, Zm, Zm);
                            break;
                        }

                    case 111: // 机械库巴
                        {
                            if (ModuleSMM.MapObj[i].CFlag / 0x40000 % 2 == 1)
                            {
                                PR = "B";
                            }
                            else if (ModuleSMM.MapObj[i].CFlag / 0x80000 % 2 == 1)
                            {
                                PR = "A";
                            }
                            else
                            {
                                PR = "";
                            }

                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + PR + ".PNG"), LX, LY, Zm, Zm);
                            G.DrawImage(Image.FromFile(P + @"\img\CMN\F1.PNG"), LX, LY, Zm, Zm);
                            break;
                        }

                    case 76: // 加邦
                        {
                            if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                            {
                                if (ModuleSMM.MapHdr.Theme == 6)
                                {
                                    PR = "B";
                                }
                                else
                                {
                                    PR = "A";
                                }
                            }
                            else
                            {
                                PR = "";
                            }

                            G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + PR + ".PNG"), LX, LY, Zm, Zm);
                            G.DrawImage(Image.FromFile(P + @"\img\CMN\F1.PNG"), LX, LY, Zm, Zm);
                            break;
                        }

                    case 33: // 1UP
                        {
                            if (ModuleSMM.MapHdr.Theme == 1 & ModuleSMM.MapHdr.Flag == 2)
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + "A.PNG"), LX, LY, Zm, Zm);
                            }
                            else
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + ".PNG"), LX, LY, Zm, Zm);
                            }

                            G.DrawImage(Image.FromFile(P + @"\img\CMN\F1.PNG"), LX, LY, Zm, Zm);
                            break;
                        }

                    default:
                        {
                            if (ModuleSMM.MapObj[i].CFlag / 0x4 % 2 == 1)
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + "A.PNG"), LX, LY, Zm, Zm);
                            }
                            else
                            {
                                G.DrawImage(Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\CID\" + ModuleSMM.MapObj[i].CID.ToString() + ".PNG"), LX, LY, Zm, Zm);
                            }

                            G.DrawImage(Image.FromFile(P + @"\img\CMN\F1.PNG"), LX, LY, Zm, Zm);
                            break;
                        }
                }
            }
        }

        public void DrawFireBar()
        {
            int i, j, LX, LY;
            string P = ModuleSMM.PT;
            int H;
            int W;
            float FR;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            // '火棍
            // '长度&H40 0000，角度EX/&H38E 38E0
            var loopTo = ModuleSMM.MapHdr.ObjCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (ModuleSMM.MapObj[i].ID == 24)
                {
                    // If MapObj(i).LID = 0 And Not L Or MapObj(i).LID > 0 And L Then
                    LX = (int)Math.Round((float)(-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm);
                    LY = (int)Math.Round(H * Zm - (float)(ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm);
                    FR = (float)(ModuleSMM.MapObj[i].Ex / 59652320d);
                    G.TranslateTransform(LX + Zm / 2, LY + Zm / 2);
                    G.RotateTransform(-FR * 5f);
                    var loopTo1 = (int)Math.Round((ModuleSMM.MapObj[i].Flag - 0x6000000) / 4194304d + 1d);
                    for (j = 0; j <= loopTo1; j++)
                        G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\24A.PNG"), 0.5d), -Zm / 4 + j * Zm, -Zm / 4, Zm, Zm / 2);
                    G.RotateTransform(FR * 5f);
                    G.TranslateTransform(-LX - Zm / 2, -LY - Zm / 2);
                    // End If

                    if (ModuleSMM.MapObj[i].Flag / 0x8 % 2 == 1)
                    {
                        G.DrawImage(Image.FromFile(P + @"\img\CMN\B0.PNG"), LX, LY, Zm, Zm);
                    }
                    else
                    {
                        G.DrawImage(Image.FromFile(P + @"\img\CMN\B1.PNG"), LX, LY, Zm, Zm);
                    }
                }
            }
        }

        public void DrawFire()
        {
            int i, LX, LY;
            string P = ModuleSMM.PT;
            int H;
            int W;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            var loopTo = ModuleSMM.MapHdr.ObjCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (ModuleSMM.MapObj[i].ID == 54)
                {
                    LX = (int)Math.Round((float)((-0.5d + ModuleSMM.MapObj[i].X / 160d) * Zm));
                    LY = (int)Math.Round(H * Zm - (float)((0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm));
                    switch (ModuleSMM.MapObj[i].Flag % 0x100)
                    {
                        case 0x40:
                            {
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, Zm * MapObj(i).H)
                                G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54A1.PNG"), 0.5d), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 3) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * ModuleSMM.MapObj[i].W, 3 * Zm * ModuleSMM.MapObj[i].H);
                                break;
                            }

                        case 0x48:
                            {
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, Zm * MapObj(i).H)
                                G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54A3.PNG"), 0.5d), (float)((-ModuleSMM.MapObj[i].W / 2d + 1d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), 3 * Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                break;
                            }

                        case 0x50:
                            {
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54B.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, Zm * MapObj(i).H)
                                G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54A5.PNG"), 0.5d), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + 1) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * ModuleSMM.MapObj[i].W, 3 * Zm * ModuleSMM.MapObj[i].H);
                                break;
                            }

                        case 0x58:
                            {
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54C.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, Zm * MapObj(i).H)
                                G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54A7.PNG"), 0.5d), (float)((-ModuleSMM.MapObj[i].W / 2d - 3d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), 3 * Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                break;
                            }

                        case 0x44:
                            {
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, Zm * MapObj(i).H)
                                G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54A2.PNG"), 0.5d), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H - 3) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * ModuleSMM.MapObj[i].W, 3 * Zm * ModuleSMM.MapObj[i].H);
                                break;
                            }

                        case 0x4C:
                            {
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54A.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, Zm * MapObj(i).H)
                                G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54A4.PNG"), 0.5d), (float)((-ModuleSMM.MapObj[i].W / 2d + 1d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), 3 * Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                break;
                            }

                        case 0x54:
                            {
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54B.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, Zm * MapObj(i).H)
                                G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54A6.PNG"), 0.5d), (float)((-ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d) * Zm), (H + 1) * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), Zm * ModuleSMM.MapObj[i].W, 3 * Zm * ModuleSMM.MapObj[i].H);
                                break;
                            }

                        case 0x5C:
                            {
                                // G.DrawImage(Image.FromFile(P & "\img\" & LH.GameStyle.ToString & "\obj\54C.PNG"), CSng((-MapObj(i).W / 2 + MapObj(i).X / 160) * Zm), H * Zm - CSng((MapObj(i).H - 0.5 + MapObj(i).Y / 160) * Zm) + KY, Zm * MapObj(i).W, Zm * MapObj(i).H)
                                G.DrawImage(ModuleSMM.SetOpacity((Bitmap)Image.FromFile(P + @"\img\" + ModuleSMM.LH.GameStyle.ToString() + @"\obj\54A8.PNG"), 0.5d), (float)((-ModuleSMM.MapObj[i].W / 2d - 3d + ModuleSMM.MapObj[i].X / 160d) * Zm), H * Zm - (float)((ModuleSMM.MapObj[i].H - 0.5d + ModuleSMM.MapObj[i].Y / 160d) * Zm), 3 * Zm * ModuleSMM.MapObj[i].W, Zm * ModuleSMM.MapObj[i].H);
                                break;
                            }
                    }
                }
            }
        }

        public void DrawObject(bool IO)
        {

            // 3D平台
            // 半碰撞
            // 蘑菇平台
            // 桥 
            // 蘑菇跳台 
            // 开关跳台
            DrawItem("/132/", false);
            DrawItem("/16/", false);
            DrawItem("/14/", false);
            DrawItem("/17/", false);
            DrawItem("/113/", false);
            DrawItem("/71/", false);


            // 箭头 单向板 中间旗 藤蔓 

            DrawItem("/66/67/106/", false);
            DrawItem("/64/", false);
            DrawItem("/90/", false);

            // 树 长长吞食花
            DrawItem("/106/107/", false);

            // 地面 传送带 开关 开关砖 P砖 冰锥
            // 斜坡单独
            ReGrdCode();
            DrawGrd(IO);
            // DrawSlope()
            DrawGrdCode();
            DrawItem("/53/94/99/100/79/", false);
            DrawIce();



            // 无LINKE
            // 管道 门 蛇 传送箱
            DrawItem("/9/55/84/97/", false);
            // 机动砖 轨道砖
            DrawItem("/85/119/", false);
            // 夹子
            DrawItem("/105/", false);
            // 轨道
            DrawTrack();
            // 软砖 问号 硬砖 竹轮 云 音符 隐藏 刺 冰块 闪烁砖 
            DrawItem("/4/5/6/21/22/23/29/43/63/110/108/", false);

            // 跷跷板 熔岩台 升降台 
            DrawItem("/91/36/11/", false);

            // 狼牙棒
            DrawItem("/83/", false);

            // 齿轮 甜甜圈
            DrawItem("/68/82/", false);

            // 道具
            DrawItem("/0/1/2/3/8/10/12/13/15/18/19/20/25/28/30/31/32/33/34/35/39/", false);
            DrawItem("/40/41/42/44/45/46/47/48/52/56/57/58/60/61/62/70/74/76/77/78/81/92/95/98/102/103/104/", false);
            DrawItem("/111/120/121/122/123/124/125/126/112/127/128/129/130/131/72/50/51/65/80/114/116/", false);
            DrawItem("/96/117/86/", false);
            // 喷枪 火棍
            DrawItem("/24/54/", false);
            // DrawFireBar(False)
            // DrawFire(False)
            // 夹子
            DrawItem("/105/", false);
            // 轨道
            DrawTrack();
            // 夹子
            DrawItem("/105/", true);
            // 卷轴相机
            // DrawItem("/89/", False)

            // LINK
            // 软砖 问号 硬砖 竹轮 云 音符 隐藏 刺 冰块
            DrawItem("/4/5/6/21/22/23/29/43/63/", true);


            // 跷跷板 熔岩台 升降台
            DrawItem("/91/36/11/", true);

            // 齿轮 甜甜圈
            DrawItem("/68/82/", true);

            // 道具
            DrawItem("/0/1/2/3/8/10/12/13/15/18/19/20/25/28/30/31/32/33/34/35/39/", true);
            DrawItem("/40/41/42/44/45/46/47/48/52/56/57/58/60/61/62/70/74/76/77/78/81/92/95/98/102/103/104/", true);
            DrawItem("/111/120/121/122/123/124/125/126/112/127/128/129/130/131/72/50/51/65/80/114/116/", true);
            DrawItem("/96/117/86/", true);
            DrawCID();

            // 喷枪 火棍
            DrawItem("/24/54/", true);
            DrawFireBar();
            DrawFire();

            // 透明管
            DrawCPipe();
        }

        public void DrawCPipe()
        {
            int i;
            int J;
            int K;
            int H;
            int W;
            string CP;
            H = ModuleSMM.MapHdr.BorT / 16;
            W = ModuleSMM.MapHdr.BorR / 16;
            var loopTo = ModuleSMM.MapHdr.ClearPipCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                var loopTo1 = ModuleSMM.MapCPipe[i].NodeCount - 1;
                for (J = 0; J <= loopTo1; J++)
                {
                    switch (J)
                    {
                        case 0:
                            {
                                var loopTo2 = ModuleSMM.MapCPipe[i].Node[J].H - 1;
                                for (K = 0; K <= loopTo2; K++)
                                {
                                    switch (ModuleSMM.MapCPipe[i].Node[J].Dir)
                                    {
                                        case 0: // R
                                            {
                                                if (K == 0)
                                                {
                                                    CP = "C";
                                                }
                                                else if (ModuleSMM.MapCPipe[i].NodeCount == 1 & K == ModuleSMM.MapCPipe[i].Node[J].H - 1)
                                                {
                                                    CP = "E";
                                                }
                                                else
                                                {
                                                    CP = "D";
                                                }

                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93" + CP + ".PNG"), (ModuleSMM.MapCPipe[i].Node[J].X + K) * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y) * Zm, Zm, 2 * Zm);
                                                break;
                                            }

                                        case 1: // L
                                            {
                                                if (K == 0)
                                                {
                                                    CP = "E";
                                                }
                                                else if (ModuleSMM.MapCPipe[i].NodeCount == 1 & K == ModuleSMM.MapCPipe[i].Node[J].H - 1)
                                                {
                                                    CP = "C";
                                                }
                                                else
                                                {
                                                    CP = "D";
                                                }

                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93" + CP + ".PNG"), (ModuleSMM.MapCPipe[i].Node[J].X - K) * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y - 1) * Zm, Zm, 2 * Zm);
                                                break;
                                            }

                                        case 2: // U
                                            {
                                                if (K == 0)
                                                {
                                                    CP = "";
                                                }
                                                else if (ModuleSMM.MapCPipe[i].NodeCount == 1 & K == ModuleSMM.MapCPipe[i].Node[J].H - 1)
                                                {
                                                    CP = "B";
                                                }
                                                else
                                                {
                                                    CP = "A";
                                                }

                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93" + CP + ".PNG"), ModuleSMM.MapCPipe[i].Node[J].X * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y - K) * Zm, 2 * Zm, Zm);
                                                break;
                                            }

                                        case 3: // D
                                            {
                                                if (K == 0)
                                                {
                                                    CP = "B";
                                                }
                                                else if (ModuleSMM.MapCPipe[i].NodeCount == 1 & K == ModuleSMM.MapCPipe[i].Node[J].H - 1)
                                                {
                                                    CP = "";
                                                }
                                                else
                                                {
                                                    CP = "A";
                                                }

                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93" + CP + ".PNG"), (ModuleSMM.MapCPipe[i].Node[J].X - 1) * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y + K) * Zm, 2 * Zm, Zm);
                                                break;
                                            }
                                    }
                                }

                                break;
                            }

                        case var @case when @case == ModuleSMM.MapCPipe[i].NodeCount - 1:
                            {
                                var loopTo3 = ModuleSMM.MapCPipe[i].Node[J].H - 1;
                                for (K = 0; K <= loopTo3; K++)
                                {
                                    switch (ModuleSMM.MapCPipe[i].Node[J].Dir)
                                    {
                                        case 0: // R
                                            {
                                                CP = Conversions.ToString(Interaction.IIf(K == ModuleSMM.MapCPipe[i].Node[J].H - 1, "E", "D"));
                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93" + CP + ".PNG"), (ModuleSMM.MapCPipe[i].Node[J].X + K) * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y) * Zm, Zm, 2 * Zm);
                                                break;
                                            }

                                        case 1: // L
                                            {
                                                CP = Conversions.ToString(Interaction.IIf(K == ModuleSMM.MapCPipe[i].Node[J].H - 1, "C", "D"));
                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93" + CP + ".PNG"), (ModuleSMM.MapCPipe[i].Node[J].X - K) * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y - 1) * Zm, Zm, 2 * Zm);
                                                break;
                                            }

                                        case 2: // U
                                            {
                                                CP = Conversions.ToString(Interaction.IIf(K == ModuleSMM.MapCPipe[i].Node[J].H - 1, "B", "A"));
                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93" + CP + ".PNG"), ModuleSMM.MapCPipe[i].Node[J].X * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y - K) * Zm, 2 * Zm, Zm);
                                                break;
                                            }

                                        case 3: // D
                                            {
                                                CP = Conversions.ToString(Interaction.IIf(K == ModuleSMM.MapCPipe[i].Node[J].H - 1, "", "A"));
                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93" + CP + ".PNG"), (ModuleSMM.MapCPipe[i].Node[J].X - 1) * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y + K) * Zm, 2 * Zm, Zm);
                                                break;
                                            }
                                    }
                                }

                                break;
                            }

                        default:
                            {
                                if (ModuleSMM.MapCPipe[i].Node[J].type >= 3 & ModuleSMM.MapCPipe[i].Node[J].type <= 10)
                                {
                                    switch (ModuleSMM.MapCPipe[i].Node[J].type)
                                    {
                                        case 3:
                                        case 7: // RU DL
                                            {
                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93G.PNG"), ModuleSMM.MapCPipe[i].Node[J].X * Zm, (H - 2 - ModuleSMM.MapCPipe[i].Node[J].Y) * Zm, 2 * Zm, 2 * Zm);
                                                break;
                                            }

                                        case 4:
                                        case 9: // RD UL
                                            {
                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93H.PNG"), ModuleSMM.MapCPipe[i].Node[J].X * Zm, (H - 2 - ModuleSMM.MapCPipe[i].Node[J].Y) * Zm, 2 * Zm, 2 * Zm);
                                                break;
                                            }

                                        case 6:
                                        case 10: // UR LD
                                            {
                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93J.PNG"), ModuleSMM.MapCPipe[i].Node[J].X * Zm, (H - 2 - ModuleSMM.MapCPipe[i].Node[J].Y) * Zm, 2 * Zm, 2 * Zm);
                                                break;
                                            }

                                        case 5:
                                        case 8: // DR LU
                                            {
                                                G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93F.PNG"), ModuleSMM.MapCPipe[i].Node[J].X * Zm, (H - 2 - ModuleSMM.MapCPipe[i].Node[J].Y) * Zm, 2 * Zm, 2 * Zm);
                                                break;
                                            }
                                    }
                                }
                                // G.DrawString(MapCPipe(i).Node(J).type.ToString, Me.Font, Brushes.Black, (MapCPipe(i).Node(J).X) * Zm, (H - 2 - MapCPipe(i).Node(J).Y) * Zm)

                                else
                                {
                                    var loopTo4 = ModuleSMM.MapCPipe[i].Node[J].H - 1;
                                    for (K = 0; K <= loopTo4; K++)
                                    {
                                        switch (ModuleSMM.MapCPipe[i].Node[J].Dir)
                                        {
                                            case 0: // R
                                                {
                                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93D.PNG"), (ModuleSMM.MapCPipe[i].Node[J].X + K) * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y) * Zm, Zm, 2 * Zm);
                                                    break;
                                                }

                                            case 1: // L
                                                {
                                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93D.PNG"), (ModuleSMM.MapCPipe[i].Node[J].X - K) * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y - 1) * Zm, Zm, 2 * Zm);
                                                    break;
                                                }

                                            case 2: // U
                                                {
                                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93A.PNG"), ModuleSMM.MapCPipe[i].Node[J].X * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y - K) * Zm, 2 * Zm, Zm);
                                                    break;
                                                }

                                            case 3: // D
                                                {
                                                    G.DrawImage(Image.FromFile(ModuleSMM.PT + @"\IMG\" + ModuleSMM.LH.GameStyle.ToString() + @"\OBJ\93A.PNG"), (ModuleSMM.MapCPipe[i].Node[J].X - 1) * Zm, (H - 1 - ModuleSMM.MapCPipe[i].Node[J].Y + K) * Zm, 2 * Zm, Zm);
                                                    break;
                                                }
                                        }
                                    }
                                }

                                break;
                            }
                    }
                }
            }
        }

        private bool MapMove = false;
        private int MMX;
        private int MMY;
        private bool MapMove2 = false;
        private int MMX2;
        private int MMY2;

        private void PicMap_MouseDown(object sender, MouseEventArgs e)
        {
            MMX = e.X;
            MMY = e.Y;
            MapMove = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            My.MyProject.Forms.Form2.P.Image.Save(ModuleSMM.PT + @"\" + TextBox9.Text + "-0.PNG", System.Drawing.Imaging.ImageFormat.Png);
            My.MyProject.Forms.Form3.P.Image.Save(ModuleSMM.PT + @"\" + TextBox9.Text + "-1.PNG", System.Drawing.Imaging.ImageFormat.Png);
            Interaction.MsgBox("已保存地图至" + ModuleSMM.PT + @"\" + TextBox9.Text);
        }

        private void RefPic()
        {
            My.MyProject.Forms.Form2.P.Left = 0;
            My.MyProject.Forms.Form2.P.Top = 0;
            ListBox1.Items.Clear();
            ListBox2.Items.Clear();
            NowIO = 0;
            LoadEFile(true);
            ModuleSMM.MapWidth[0] = ModuleSMM.MapHdr.BorR / 16;
            ModuleSMM.MapHeight[0] = ModuleSMM.MapHdr.BorT / 16;
            string DN = Conversions.ToString(Interaction.IIf(ModuleSMM.MapHdr.Flag == 2, "A", ""));
            Tile = Image.FromFile(ModuleSMM.PT + @"\img\TILE\" + ModuleSMM.LH.GameStyle + "-" + ModuleSMM.MapHdr.Theme.ToString() + DN + ".png");
            TileW = Tile.Width / 16;
            InitPng();
            DrawObject(true);
            RefList(ListBox1, true);
            // FORM2.P.Image = B
            My.MyProject.Forms.Form2.P.Image = B;


            // ObjInfo()

            NowIO = 1;
            My.MyProject.Forms.Form3.P.Left = 0;
            My.MyProject.Forms.Form3.P.Top = 0;
            LoadEFile(false);
            ModuleSMM.MapWidth[1] = ModuleSMM.MapHdr.BorR / 16;
            ModuleSMM.MapHeight[1] = ModuleSMM.MapHdr.BorT / 16;
            DN = Conversions.ToString(Interaction.IIf(ModuleSMM.MapHdr.Flag == 2, "A", ""));
            Tile = Image.FromFile(ModuleSMM.PT + @"\img\TILE\" + ModuleSMM.LH.GameStyle + "-" + ModuleSMM.MapHdr.Theme.ToString() + DN + ".png");
            TileW = Tile.Width / 16;
            InitPng2();
            DrawObject(false);
            RefList(ListBox2, false);
            My.MyProject.Forms.Form3.P.Image = B;
            // ObjInfo()

            // GetLvlInfo()
        }

        [DllImport("shdocvw.dll")]
        private static extern int DoFileDownload(string lpszFile);
        [DllImport("urlmon", EntryPoint = "URLDownloadToFileA")]
        private static extern int URLDownloadToFile(int pCaller, string szURL, string szFileName, int dwReserved, int lpfnCB);

        private bool isMapIO = true;

        private void Button5_Click(object sender, EventArgs e)
        {
            var I = default(int);
            TextBox9.Text = TextBox9.Text.Replace("-", "");
            TextBox9.Text = TextBox9.Text.Replace(" ", "");
            TextBox9.Text = Strings.Left(TextBox9.Text, 9);
            // Button2.Enabled = False
            // Button8.Enabled = False
            Label2.Text = "马里奥制造2关卡机器人 v010";
            Label2.Text += Constants.vbCrLf + DateTime.Now.ToString() + " 加载地图(L00)";
            if (TextBox9.Text.Length == 9)
            {
                if (File.Exists(ModuleSMM.PT + @"\MAP\" + TextBox9.Text))
                {
                    // 存在解密文件
                    Label2.Text += Constants.vbCrLf + DateTime.Now.ToString() + " 已加载地图(L01)";
                    TextBox1.Text = ModuleSMM.PT + @"\MAP\" + TextBox9.Text;
                    // Button2.Enabled = True
                    // Button8.Enabled = True
                    isMapIO = true;
                    RefPic();
                }
                else if (File.Exists(ModuleSMM.PT + @"\MAP\" + TextBox9.Text + ".DAT"))
                {
                    // 存在地图文件
                    Label2.Text += Constants.vbCrLf + DateTime.Now.ToString() + " 解析地图(L02)";
                    DeMap(TextBox9.Text + ".DAT", TextBox9.Text);
                    System.Threading.Thread.Sleep(3000);
                    Label2.Text += Constants.vbCrLf + DateTime.Now.ToString() + " 已加载地图(L03)";
                    TextBox1.Text = ModuleSMM.PT + @"\MAP\" + TextBox9.Text;
                    // Button2.Enabled = True
                    // Button8.Enabled = True
                    isMapIO = true;
                    RefPic();
                }
                else
                {
                    Label2.Text += Constants.vbCrLf + DateTime.Now.ToString() + " 从服务器加载地图(L04)";
                    string argszURL = UR2 + TextBox9.Text;
                    string argszFileName = ModuleSMM.PT + @"\MAP\" + TextBox9.Text + ".DAT";
                    Form1.URLDownloadToFile(0, argszURL, argszFileName, 0, 0);
                    do
                    {
                        if (File.Exists(ModuleSMM.PT + @"\MAP\" + TextBox9.Text + ".DAT"))
                        {
                            DeMap(TextBox9.Text + ".DAT", TextBox9.Text);
                            I = 0;
                            do
                            {
                                I += 1;
                                Label2.Text += Constants.vbCrLf + DateTime.Now.ToString() + " 解析地图(L05)";
                                System.Threading.Thread.Sleep(1000);
                                if (File.Exists(ModuleSMM.PT + @"\MAP\" + TextBox9.Text))
                                {
                                    break;
                                }
                            }
                            while (I <= 10);
                            Label2.Text += Constants.vbCrLf + DateTime.Now.ToString() + " 已加载地图(L06)";
                            TextBox1.Text = ModuleSMM.PT + @"\MAP\" + TextBox9.Text;
                            isMapIO = true;
                            RefPic();
                            return;
                        }
                        else
                        {
                            I += 1;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    while (I <= 5);
                    Label2.Text += Constants.vbCrLf + DateTime.Now.ToString() + " 加载地图超时(Error01)";
                }
            }
            else
            {
                Label2.Text += Constants.vbCrLf + DateTime.Now.ToString() + " 图号错误(Error02)";
            }
        }

        public void DeMap(string P1, string P2)
        {
            ProcessStartInfo info;
            info = new ProcessStartInfo()
            {
                FileName = ModuleSMM.PT + @"\MAP\D.EXE",
                Arguments = ModuleSMM.PT + @"\MAP\" + P1 + " " + ModuleSMM.PT + @"\MAP\" + P2
            };
            Process Proc;
            try
            {
                Proc = Process.Start(info);
                Proc.WaitForExit();
                if (Proc.HasExited == false)
                {
                    Proc.Kill();
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            My.MyProject.Forms.Form2.P.Image = GetTile(0, 0, 1, 1);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            My.MyProject.Forms.Form3.Show();
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            Label1.Text = "缩放Zoom:" + Math.Pow(2d, TrackBar1.Value);
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            FileSystem.FileCopy(@"D:\yuzu-windows-msvc-20200531-5ead55df7\user\save\0000000000000000\6D74F859658CF0E9DC9DD5D96A655C71\01009B90006DC000\course_data_000.BCD", ModuleSMM.PT + @"\MAP\Course_data_000.BCD");
            DeMap("course_data_000.BCD", "Course_data_000");
            System.Threading.Thread.Sleep(3000);
            Text = "已加载地图_11";
            TextBox1.Text = ModuleSMM.PT + @"\MAP\Course_data_000";
            isMapIO = true;
            RefPic();
        }

        private ModuleSMM.ObjStr SetObjInfo(int idx, int flag)
        {
            ModuleSMM.ObjStr SetObjInfoRet = default;
            string PR, PB;
            SetObjInfoRet.Obj = "";
            SetObjInfoRet.Flag = "";
            SetObjInfoRet.State = "";
            SetObjInfoRet.SubObj = "";
            SetObjInfoRet.SubFlag = "";
            SetObjInfoRet.SubState = "";
            switch (idx)
            {
                case -1: // NOTHING啥也妹有
                    {
                        break;
                    }

                case 89:// 卷轴相机
                    {
                        break;
                    }

                case 9: // 管道
                    {
                        break;
                    }

                case 93:// 透明管
                    {
                        break;
                    }

                case 14:
                case 16:
                case 71: // 平台
                    {
                        SetObjInfoRet.Obj = idx.ToString();
                        break;
                    }

                case 17: // 桥
                    {
                        SetObjInfoRet.Obj = idx.ToString();
                        break;
                    }

                case 87:
                case 88:// 斜坡
                    {
                        break;
                    }

                case 53:
                case 94:// 传送带
                    {
                        break;
                    }

                case 105:// 夹子
                    {
                        break;
                    }

                case 55:
                case 97: // 门 传送箱
                    {
                        SetObjInfoRet.Obj = idx.ToString();
                        SetObjInfoRet.Flag = Strings.Mid("ABCDEFGHJ", flag % 0x800000 / 0x200000 + 1, 1);
                        break;
                    }

                case 34: // 火花 
                    {
                        if (flag / 0x4 % 2 == 1)
                        {
                            if (flag / 0x40000 % 2 == 1)
                            {
                                PB = "C";
                            }
                            else
                            {
                                PB = "A";
                            }
                        }
                        else if (flag / 0x40000 % 2 == 1)
                        {
                            PB = "B";
                        }
                        else
                        {
                            PB = "";
                        }

                        PR = "";
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x8000 % 2 == 1, "P", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 2 % 2 == 1, "W", ""));
                        SetObjInfoRet.Obj = idx.ToString() + PB;
                        SetObjInfoRet.Flag = PR;
                        break;
                    }

                case 44:
                case 81:
                case 116: // 弹力球 大蘑菇 锤子
                    {
                        if (flag / 0x40000 % 2 == 1)
                        {
                            PB = "A";
                        }
                        else
                        {
                            PB = "";
                        }

                        PR = "";
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x8000 % 2 == 1, "P", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 2 % 2 == 1, "W", ""));
                        SetObjInfoRet.Obj = idx.ToString() + PB;
                        SetObjInfoRet.Flag = PR;
                        break;
                    }

                case 74: // 加邦
                    {
                        if (flag / 0x4 % 2 == 1)
                        {
                            if (ModuleSMM.MapHdr.Theme == 6)
                            {
                                PB = "B";
                            }
                            else
                            {
                                PB = "A";
                            }
                        }
                        else
                        {
                            PB = "";
                        }

                        PR = "";
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x4000 % 2 == 1, "M", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x8000 % 2 == 1, "P", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 2 % 2 == 1, "W", ""));
                        SetObjInfoRet.Obj = idx.ToString() + PB;
                        SetObjInfoRet.Flag = PR;
                        break;
                    }

                case 78: // 仙人掌
                    {
                        if (ModuleSMM.MapHdr.Theme == 6)
                        {
                            PB = "A";
                        }
                        else
                        {
                            PB = "";
                        }

                        PR = "";
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x4000 % 2 == 1, "M", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x8000 % 2 == 1, "P", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 2 % 2 == 1, "W", ""));
                        SetObjInfoRet.Obj = idx.ToString() + PB;
                        SetObjInfoRet.Flag = PR;
                        break;
                    }

                case 3: // 德莱文
                    {
                        PB = Conversions.ToString(Interaction.IIf(flag / 0x4000 % 2 == 1, "A", ""));
                        PR = "";
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x8000 % 2 == 1, "P", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 2 % 2 == 1, "W", ""));
                        SetObjInfoRet.Obj = idx.ToString() + PB;
                        SetObjInfoRet.Flag = PR;
                        break;
                    }

                case 70:
                case 111:
                case 110: // 大金币 机械酷巴 尖刺砖块
                    {
                        if (flag / 0x40000 % 2 == 1)
                        {
                            PB = "A";
                        }
                        else if (flag / 0x80000 % 2 == 1)
                        {
                            PB = "B";
                        }
                        else
                        {
                            PB = "";
                        }

                        PR = "";
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x4000 % 2 == 1, "M", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x8000 % 2 == 1, "P", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 2 % 2 == 1, "W", ""));
                        SetObjInfoRet.Obj = idx.ToString() + PB;
                        SetObjInfoRet.Flag = PR;
                        break;
                    }

                case 33: // 1UP
                    {
                        if (ModuleSMM.MapHdr.Theme == 0 & ModuleSMM.MapHdr.Flag == 2)
                        {
                            PB = "A";
                        }
                        else
                        {
                            PB = "";
                        }

                        PR = "";
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x4000 % 2 == 1, "M", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x8000 % 2 == 1, "P", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 2 % 2 == 1, "W", ""));
                        SetObjInfoRet.Obj = idx.ToString() + PB;
                        SetObjInfoRet.Flag = PR;
                        break;
                    }

                case 45: // 鞋 耀西
                    {
                        switch (ModuleSMM.LH.GameStyle)
                        {
                            case 21847:
                            case 22349: // U W
                                {
                                    if (flag / 0x4000 % 2 == 1)
                                    {
                                        PB = "A";
                                    }
                                    else
                                    {
                                        PB = "";
                                    }

                                    PR = "";
                                    break;
                                }

                            default:
                                {
                                    if (flag / 0x4 % 2 == 1)
                                    {
                                        PB = "A";
                                    }
                                    else
                                    {
                                        PB = "";
                                    }

                                    PR = "";
                                    PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x4000 % 2 == 1, "M", ""));
                                    break;
                                }
                        }

                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x8000 % 2 == 1, "P", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 2 % 2 == 1, "W", ""));
                        SetObjInfoRet.Obj = idx.ToString() + PB;
                        SetObjInfoRet.Flag = PR;
                        break;
                    }

                default:
                    {
                        if (flag / 0x4 % 2 == 1)
                        {
                            PB = "A";
                        }
                        else
                        {
                            PB = "";
                        }

                        PR = "";
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x4000 % 2 == 1, "M", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 0x8000 % 2 == 1, "P", ""));
                        PR = Conversions.ToString(PR + Interaction.IIf(flag / 2 % 2 == 1, "W", ""));
                        SetObjInfoRet.Obj = idx.ToString() + PB;
                        SetObjInfoRet.Flag = PR;
                        break;
                    }
            }

            return SetObjInfoRet;
        }

        private void RefList(ListBox L, bool IO)
        {
            int i;
            int P;
            string PB;
            ModuleSMM.ObjStr J1, J2;
            var loopTo = ModuleSMM.MapHdr.ObjCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (ModuleSMM.MapObj[i].CID == -1)
                {
                    L.Items.Add(ModuleSMM.GetItemName(ModuleSMM.MapObj[i].ID, ModuleSMM.LH.GameStyle).ToString() + " " + (0.5d + ModuleSMM.MapObj[i].X / 160d).ToString() + "," + (0.5d + ModuleSMM.MapObj[i].Y / 160d).ToString());
                }
                else
                {
                    L.Items.Add(ModuleSMM.GetItemName(ModuleSMM.MapObj[i].ID, ModuleSMM.LH.GameStyle).ToString() + "(" + ModuleSMM.GetItemName(ModuleSMM.MapObj[i].CID, ModuleSMM.LH.GameStyle) + ") " + (0.5d + ModuleSMM.MapObj[i].X / 160d).ToString() + "," + (0.5d + ModuleSMM.MapObj[i].Y / 160d).ToString());
                }

                switch (ModuleSMM.MapObj[i].ID)
                {
                    case 9: // 管道PIPE
                        {
                            break;
                        }

                    case 93:// 透明管道CLEAR PIPE
                        {
                            break;
                        }

                    case 14:
                    case 16:
                    case 71: // 平台
                        {
                            for (int W = 1, loopTo1 = ModuleSMM.MapObj[i].W; W <= loopTo1; W++)
                            {
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160d))].Obj += ModuleSMM.MapObj[i].ID.ToString() + ",";
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160d))].Flag += ",";
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160d))].SubObj += ",";
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160d))].SubFlag += ",";
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160d))].State += ",";
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H + ModuleSMM.MapObj[i].Y / 160d))].SubState += ",";
                            }

                            break;
                        }

                    case 17: // 桥
                        {
                            for (int W = 1, loopTo2 = ModuleSMM.MapObj[i].W; W <= loopTo2; W++)
                            {
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H - 1 + ModuleSMM.MapObj[i].Y / 160d))].Obj += ModuleSMM.MapObj[i].ID.ToString() + ",";
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H - 1 + ModuleSMM.MapObj[i].Y / 160d))].Flag += ",";
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H - 1 + ModuleSMM.MapObj[i].Y / 160d))].SubObj += ",";
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H - 1 + ModuleSMM.MapObj[i].Y / 160d))].SubFlag += ",";
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H - 1 + ModuleSMM.MapObj[i].Y / 160d))].State += ",";
                                ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(ModuleSMM.MapObj[i].H - 1 + ModuleSMM.MapObj[i].Y / 160d))].SubState += ",";
                            }

                            break;
                        }

                    case 87:
                    case 88: // 斜坡 SLOPE
                        {
                            break;
                        }

                    case 53:
                    case 94: // 传送带
                        {
                            break;
                        }

                    case 105: // 夹子
                        {
                            break;
                        }

                    case 97: // 传送箱
                        {
                            P = ModuleSMM.MapObj[i].Flag % 0x800000 / 0x200000;
                            if (ModuleSMM.MapObj[i].Flag / 0x4 % 2 == 1)
                            {
                                PB = "A";
                            }
                            else
                            {
                                PB = "";
                            }

                            for (int W = 1, loopTo3 = ModuleSMM.MapObj[i].W; W <= loopTo3; W++)
                            {
                                for (int H = 1, loopTo4 = ModuleSMM.MapObj[i].H; H <= loopTo4; H++)
                                {
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].Obj += ModuleSMM.MapObj[i].ID.ToString() + PB + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].Flag += Strings.Mid("ABCDEFGHJ", P + 1, 1) + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubObj += ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubFlag += ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].State += ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubState += ",";
                                }
                            }

                            break;
                        }

                    case 55: // 门 
                        {
                            P = ModuleSMM.MapObj[i].Flag % 0x800000 / 0x200000;
                            if (ModuleSMM.MapObj[i].Flag / 0x40000 % 2 == 1)
                            {
                                PB = "A";
                            }
                            else if (ModuleSMM.MapObj[i].Flag / 0x80000 % 2 == 1)
                            {
                                PB = "B";
                            }
                            else
                            {
                                PB = "";
                            }

                            for (int W = 1, loopTo5 = ModuleSMM.MapObj[i].W; W <= loopTo5; W++)
                            {
                                for (int H = 1, loopTo6 = ModuleSMM.MapObj[i].H; H <= loopTo6; H++)
                                {
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].Obj += ModuleSMM.MapObj[i].ID.ToString() + PB + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].Flag += Strings.Mid("ABCDEFGHJ", P + 1, 1) + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubObj += ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubFlag += ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].State += ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - 0.5d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubState += ",";
                                }
                            }

                            break;
                        }

                    case 34:
                    case 44:
                    case 81: // 花 弹力球 USA 大蘑菇
                        {
                            J1 = SetObjInfo(ModuleSMM.MapObj[i].ID, ModuleSMM.MapObj[i].Flag);
                            for (int W = 1, loopTo7 = ModuleSMM.MapObj[i].W; W <= loopTo7; W++)
                            {
                                for (int H = 1, loopTo8 = ModuleSMM.MapObj[i].H; H <= loopTo8; H++)
                                {
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].Obj += J1.Obj + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].Flag += J1.Flag + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].State += J1.State + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubObj += ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubFlag += ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubState += ",";
                                }
                            }

                            break;
                        }

                    default:
                        {
                            J1 = SetObjInfo(ModuleSMM.MapObj[i].ID, ModuleSMM.MapObj[i].Flag);
                            J2 = SetObjInfo(ModuleSMM.MapObj[i].CID, ModuleSMM.MapObj[i].CFlag);
                            for (int W = 1, loopTo9 = ModuleSMM.MapObj[i].W; W <= loopTo9; W++)
                            {
                                for (int H = 1, loopTo10 = ModuleSMM.MapObj[i].H; H <= loopTo10; H++)
                                {
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].Obj += J1.Obj + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].Flag += J1.Flag + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].State += J1.State + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubObj += J2.Obj + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubFlag += J2.Flag + ",";
                                    ModuleSMM.ObjLocData[NowIO, (int)Math.Round(Conversion.Int(W - ModuleSMM.MapObj[i].W / 2d + ModuleSMM.MapObj[i].X / 160d)), (int)Math.Round(Conversion.Int(H + ModuleSMM.MapObj[i].Y / 160d))].SubState += J2.State + ",";
                                }
                            }

                            break;
                        }
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            TextBox3.Text = "";
            ObjInfo();
        }

        public void ObjInfo()
        {
            int i;
            int M;
            string s = "==OBJ==" + Constants.vbCrLf;
            // OBJ参数详情
            s += "ID" + Constants.vbTab + "ID" + Constants.vbTab + "X" + Constants.vbTab + "Y" + Constants.vbTab + "FLAG" + Constants.vbTab + "CID" + Constants.vbTab + "CFLAG" + Constants.vbTab + "HEX" + Constants.vbTab + "LID" + Constants.vbTab + "SID" + Constants.vbTab + "W" + Constants.vbTab + "H" + Constants.vbCrLf;
            var loopTo = ModuleSMM.MapHdr.ObjCount - 1;
            for (i = 0; i <= loopTo; i++)
            {
                s += ModuleSMM.ObjEng[ModuleSMM.MapObj[i].ID] + Constants.vbTab + ModuleSMM.MapObj[i].ID + Constants.vbTab + ModuleSMM.MapObj[i].X + Constants.vbTab + ModuleSMM.MapObj[i].Y + Constants.vbTab;
                s += Conversion.Hex(ModuleSMM.MapObj[i].Flag) + Constants.vbTab + ModuleSMM.MapObj[i].CID + Constants.vbTab + Conversion.Hex(ModuleSMM.MapObj[i].CFlag) + Constants.vbTab + Conversion.Hex(ModuleSMM.MapObj[i].Ex) + Constants.vbTab;
                s += ModuleSMM.MapObj[i].LID + Constants.vbTab + ModuleSMM.MapObj[i].SID + Constants.vbTab + ModuleSMM.MapObj[i].W + Constants.vbTab + ModuleSMM.MapObj[i].H + Constants.vbCrLf;
            }

            s += "==TRACK==" + Constants.vbCrLf;
            s += "UN" + Constants.vbTab + "X" + Constants.vbTab + "Y" + Constants.vbTab + "HFLAG" + Constants.vbTab + "TYPE" + Constants.vbTab + "LID" + Constants.vbTab + "HK0,HK1" + Constants.vbCrLf;
            var loopTo1 = ModuleSMM.MapHdr.TrackCount - 1;
            for (i = 0; i <= loopTo1; i++)
                s += ModuleSMM.MapTrk[i].UN + Constants.vbTab + ModuleSMM.MapTrk[i].X + Constants.vbTab + ModuleSMM.MapTrk[i].Y + Constants.vbTab + Conversion.Hex(ModuleSMM.MapTrk[i].Flag) + Constants.vbTab + ModuleSMM.MapTrk[i].Type + Constants.vbTab + ModuleSMM.MapTrk[i].LID + Constants.vbTab + Conversion.Hex(ModuleSMM.MapTrk[i].K0) + Constants.vbTab + Conversion.Hex(ModuleSMM.MapTrk[i].K1) + Constants.vbCrLf;
            s += "==CPIPE==" + Constants.vbCrLf;
            var loopTo2 = ModuleSMM.MapHdr.ClearPipCount - 1;
            for (M = 0; M <= loopTo2; M++)
            {
                s += "INDEX" + Constants.vbTab + "NC" + Constants.vbTab + "N" + Constants.vbTab + "HFLAG" + Constants.vbTab + "TYPE" + Constants.vbTab + "HLID" + Constants.vbTab + "HK0,HK1" + Constants.vbCrLf;
                s += ModuleSMM.MapCPipe[M].Index + Constants.vbTab + ModuleSMM.MapCPipe[M].NodeCount + Constants.vbCrLf;
                s += "TYPE" + Constants.vbTab + "INDEX" + Constants.vbTab + "X" + Constants.vbTab + "Y" + Constants.vbTab + "W" + Constants.vbTab + "H" + Constants.vbTab + "DIR" + Constants.vbCrLf;
                var loopTo3 = ModuleSMM.MapCPipe[M].NodeCount - 1;
                for (i = 0; i <= loopTo3; i++)
                    s += ModuleSMM.MapCPipe[M].Node[i].type + Constants.vbTab + ModuleSMM.MapCPipe[M].Node[i].index + Constants.vbTab + ModuleSMM.MapCPipe[M].Node[i].X + Constants.vbTab + ModuleSMM.MapCPipe[M].Node[i].Y + Constants.vbTab + ModuleSMM.MapCPipe[M].Node[i].W + Constants.vbTab + ModuleSMM.MapCPipe[M].Node[i].H + Constants.vbTab + ModuleSMM.MapCPipe[M].Node[i].Dir + Constants.vbCrLf;
            }

            TextBox4.Text += s;
        }

        private void PicMap2_MouseUp(object sender, MouseEventArgs e)
        {
            MapMove2 = false;
        }

        public string GetPage(string url)
        {
            string GetPageRet = default;
            HttpWebRequest hRqst = (HttpWebRequest)WebRequest.Create(url);
            hRqst.ContentType = "application/x-www-form-urlencoded";
            hRqst.Method = "GET";
            Stream streamData;
            HttpWebResponse hRsp = (HttpWebResponse)hRqst.GetResponse();
            streamData = hRsp.GetResponseStream();
            var readStream = new StreamReader(streamData, System.Text.Encoding.UTF8);
            GetPageRet = readStream.ReadToEnd();
            streamData.Close();
            return GetPageRet;
            Err:
            ;
            GetPageRet = "";
        }

        private JObject ConInfo(string a)
        {
            JObject ConInfoRet = default;
            ConInfoRet = JObject.Parse(a);
            return ConInfoRet;
        }

        private Graphics G2;
        private Bitmap B2 = new Bitmap(1, 1);

        public Bitmap GetImageFromByteArray(byte[] bytes)
        {
            return (Bitmap)Image.FromStream(new MemoryStream(bytes));
        }

        public byte[] GetByteArrayFromImage(Bitmap img)
        {
            var ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            var outBytes = new byte[(int)(ms.Length - 1L) + 1];
            ms.Seek(0L, SeekOrigin.Begin);
            ms.Read(outBytes, 0, (int)ms.Length);
            return outBytes;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Commented out code randomly");
            //PictureBox1.Image.Save(@"E:\OBS录像\极难团\" + Strings.Left(TextBox9.Text, 9) + ".PNG");
            Text = "已保存";
        }

        private Graphics G0;
        private Bitmap B0 = new Bitmap(300, 30);
        private string UR2;

        private void Form1_Load(object sender, EventArgs e)
        {
            ModuleSMM.PT = Application.StartupPath;
            int I, J, K;
            UR2 = "";
            var loopTo = ModuleSMM.UR.Length;
            for (I = 1; I <= loopTo; I += 6)
            {
                J = Conversions.ToInteger(Conversion.Int(Strings.Mid(ModuleSMM.UR, I, 3)));
                K = Conversions.ToInteger(Conversion.Int(Strings.Mid(ModuleSMM.UR, I + 3, 3)));
                UR2 += Conversions.ToString(Strings.Chr(J ^ K));
            }

            G2 = Graphics.FromImage(B2);
            G0 = Graphics.FromImage(B0);
            AllowDrop = true;
            My.MyProject.Forms.Form2.P.AllowDrop = true;
            My.MyProject.Forms.Form3.P.AllowDrop = true;
            SetTileLoc();
            SetGrdLoc();
            MiiB[0] = new Bitmap(128, 128);
            MiiB[1] = new Bitmap(128, 128);
            MiiB[2] = new Bitmap(128, 128);
            MiiG[0] = Graphics.FromImage(MiiB[0]);
            MiiG[1] = Graphics.FromImage(MiiB[1]);
            MiiG[2] = Graphics.FromImage(MiiB[2]);
            ModuleSMM.TrackYPt[8, 0] = new Point(4, 0);
            ModuleSMM.TrackYPt[8, 1] = new Point(0, 2);
            ModuleSMM.TrackYPt[8, 2] = new Point(4, 4);
            ModuleSMM.TrackYPt[12, 0] = new Point(4, 0);
            ModuleSMM.TrackYPt[12, 1] = new Point(0, 2);
            ModuleSMM.TrackYPt[12, 2] = new Point(4, 4);
            ModuleSMM.TrackYPt[9, 0] = new Point(0, 0);
            ModuleSMM.TrackYPt[9, 1] = new Point(0, 4);
            ModuleSMM.TrackYPt[9, 2] = new Point(4, 2);
            ModuleSMM.TrackYPt[13, 0] = new Point(0, 0);
            ModuleSMM.TrackYPt[13, 1] = new Point(0, 4);
            ModuleSMM.TrackYPt[13, 2] = new Point(4, 2);
            ModuleSMM.TrackYPt[10, 0] = new Point(0, 0);
            ModuleSMM.TrackYPt[10, 1] = new Point(4, 0);
            ModuleSMM.TrackYPt[10, 2] = new Point(2, 4);
            ModuleSMM.TrackYPt[14, 0] = new Point(0, 0);
            ModuleSMM.TrackYPt[14, 1] = new Point(4, 0);
            ModuleSMM.TrackYPt[14, 2] = new Point(2, 4);
            ModuleSMM.TrackYPt[11, 0] = new Point(2, 0);
            ModuleSMM.TrackYPt[11, 1] = new Point(0, 4);
            ModuleSMM.TrackYPt[11, 2] = new Point(4, 4);
            ModuleSMM.TrackYPt[15, 0] = new Point(2, 0);
            ModuleSMM.TrackYPt[15, 1] = new Point(0, 4);
            ModuleSMM.TrackYPt[15, 2] = new Point(4, 4);
        }

        public void SetTileLoc()
        {
            // 4,4A 5 6 8 8A 21 22 23 23A 29 43 49 63 79 79A 92 99 100 100A
            ModuleSMM.TileLoc[4, 0] = new Point(1, 0);
            ModuleSMM.TileLoc[4, 1] = new Point(2, 43);
            ModuleSMM.TileLoc[5, 0] = new Point(2, 0);
            ModuleSMM.TileLoc[6, 0] = new Point(6, 0);
            ModuleSMM.TileLoc[8, 0] = new Point(7, 0);
            ModuleSMM.TileLoc[8, 1] = new Point(0, 17);
            ModuleSMM.TileLoc[21, 0] = new Point(0, 4);
            ModuleSMM.TileLoc[22, 0] = new Point(6, 6);
            ModuleSMM.TileLoc[23, 0] = new Point(4, 0);
            ModuleSMM.TileLoc[23, 1] = new Point(6, 5);
            ModuleSMM.TileLoc[29, 0] = new Point(3, 0);
            ModuleSMM.TileLoc[43, 0] = new Point(2, 4);
            ModuleSMM.TileLoc[49, 0] = new Point(15, 15);
            ModuleSMM.TileLoc[63, 0] = new Point(8, 7);
            ModuleSMM.TileLoc[79, 0] = new Point(1, 43);
            ModuleSMM.TileLoc[79, 1] = new Point(0, 43);
            ModuleSMM.TileLoc[92, 0] = new Point(0, 16);
            ModuleSMM.TileLoc[99, 0] = new Point(2, 23);
            ModuleSMM.TileLoc[100, 0] = new Point(3, 22);
            ModuleSMM.TileLoc[100, 1] = new Point(2, 21);

            // pipe loc
            // UDLRVH
            // GRBO
            ModuleSMM.PipeLoc[0, 0] = new Point(14, 0);
            ModuleSMM.PipeLoc[0, 1] = new Point(14, 2);
            ModuleSMM.PipeLoc[0, 2] = new Point(11, 0);
            ModuleSMM.PipeLoc[0, 3] = new Point(13, 0);
            ModuleSMM.PipeLoc[0, 4] = new Point(12, 0);
            ModuleSMM.PipeLoc[0, 5] = new Point(14, 1);
            ModuleSMM.PipeLoc[1, 0] = new Point(6, 37);
            ModuleSMM.PipeLoc[1, 1] = new Point(12, 37);
            ModuleSMM.PipeLoc[1, 2] = new Point(4, 24);
            ModuleSMM.PipeLoc[1, 3] = new Point(6, 24);
            ModuleSMM.PipeLoc[1, 4] = new Point(5, 24);
            ModuleSMM.PipeLoc[1, 5] = new Point(6, 38);
            ModuleSMM.PipeLoc[2, 0] = new Point(10, 37);
            ModuleSMM.PipeLoc[2, 1] = new Point(12, 38);
            ModuleSMM.PipeLoc[2, 2] = new Point(3, 37);
            ModuleSMM.PipeLoc[2, 3] = new Point(5, 37);
            ModuleSMM.PipeLoc[2, 4] = new Point(4, 37);
            ModuleSMM.PipeLoc[2, 5] = new Point(10, 38);
            ModuleSMM.PipeLoc[3, 0] = new Point(8, 37);
            ModuleSMM.PipeLoc[3, 1] = new Point(14, 37);
            ModuleSMM.PipeLoc[3, 2] = new Point(0, 37);
            ModuleSMM.PipeLoc[3, 3] = new Point(2, 37);
            ModuleSMM.PipeLoc[3, 4] = new Point(1, 37);
            ModuleSMM.PipeLoc[3, 5] = new Point(8, 38);
        }

        private Color GetColor(short x, short y)
        {
            Color GetColorRet = default;
            G2.CopyFromScreen(new Point(x, y), new Point(0, 0), new Size(1, 1));
            GetColorRet = B2.GetPixel(0, 0);
            return GetColorRet;
        }

        private void PicMap_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void PicMap_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var path in files)
                TextBox1.Text = path;
            RefPic();
        }

        private void PicMap2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void PicMap2_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var path in files)
                TextBox1.Text = path;
            RefPic();
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var path in files)
                TextBox1.Text = path;
            RefPic();
        }

        private string GetD(string P)
        {
            string GetDRet = default;
            var S = P.Split('|');
            GetDRet = S[0].Replace(Conversions.ToString('\n'), "");
            return GetDRet;
        }

        public string GetToken(JObject JSON, string a)
        {
            string GetTokenRet = default;
            GetTokenRet = Constants.vbCrLf;
            GetTokenRet += "昵称:" + JSON.SelectToken(a).SelectToken("name").ToString() + Constants.vbCrLf;
            GetTokenRet += "ID:" + JSON.SelectToken(a).SelectToken("code").ToString() + Constants.vbCrLf;
            GetTokenRet += "===关卡游玩===" + Constants.vbCrLf;
            GetTokenRet += "游玩数:" + JSON.SelectToken(a).SelectToken("courses_played").ToString() + Constants.vbCrLf;
            GetTokenRet += "通过数:" + JSON.SelectToken(a).SelectToken("courses_cleared").ToString() + Constants.vbCrLf;
            GetTokenRet += "尝试数:" + JSON.SelectToken(a).SelectToken("courses_attempted").ToString() + Constants.vbCrLf;
            GetTokenRet += "死亡数:" + JSON.SelectToken(a).SelectToken("courses_deaths").ToString() + Constants.vbCrLf;
            GetTokenRet += "首插数:" + JSON.SelectToken(a).SelectToken("first_clears").ToString() + Constants.vbCrLf;
            GetTokenRet += "纪录数:" + JSON.SelectToken(a).SelectToken("world_records").ToString() + Constants.vbCrLf;
            GetTokenRet += "===工匠点数===" + Constants.vbCrLf;
            GetTokenRet += "工匠点数:" + JSON.SelectToken(a).SelectToken("maker_points").ToString() + Constants.vbCrLf;
            GetTokenRet += "赞:" + JSON.SelectToken(a).SelectToken("likes").ToString() + Constants.vbCrLf;
            GetTokenRet += "上传关卡:" + JSON.SelectToken(a).SelectToken("uploaded_levels").ToString() + Constants.vbCrLf;
            GetTokenRet += "===耐力挑战===" + Constants.vbCrLf;
            GetTokenRet += "简单:" + JSON.SelectToken(a).SelectToken("easy_highscore").ToString() + Constants.vbCrLf;
            GetTokenRet += "普通:" + JSON.SelectToken(a).SelectToken("normal_highscore").ToString() + Constants.vbCrLf;
            GetTokenRet += "困难:" + JSON.SelectToken(a).SelectToken("expert_highscore").ToString() + Constants.vbCrLf;
            GetTokenRet += "极难:" + JSON.SelectToken(a).SelectToken("super_expert_highscore").ToString() + Constants.vbCrLf;
            GetTokenRet += "===多人对战===" + Constants.vbCrLf;
            GetTokenRet += "对战等级:" + JSON.SelectToken(a).SelectToken("versus_rank_name").ToString() + "(" + JSON.SelectToken(a).SelectToken("versus_rating").ToString() + ")" + Constants.vbCrLf;
            GetTokenRet += "对战游玩:" + JSON.SelectToken(a).SelectToken("versus_plays").ToString() + Constants.vbCrLf;
            GetTokenRet += "对战胜场:" + JSON.SelectToken(a).SelectToken("versus_won").ToString() + Constants.vbCrLf;
            GetTokenRet += "对战负场:" + JSON.SelectToken(a).SelectToken("versus_lost").ToString() + Constants.vbCrLf;
            GetTokenRet += "近期表现:" + JSON.SelectToken(a).SelectToken("recent_performance").ToString() + Constants.vbCrLf;
            GetTokenRet += "当前连胜:" + JSON.SelectToken(a).SelectToken("versus_win_streak").ToString() + Constants.vbCrLf;
            GetTokenRet += "当前连负:" + JSON.SelectToken(a).SelectToken("versus_lose_streak").ToString() + Constants.vbCrLf;
            GetTokenRet += "对战击杀:" + JSON.SelectToken(a).SelectToken("versus_kills").ToString() + Constants.vbCrLf;
            GetTokenRet += "对战被杀:" + JSON.SelectToken(a).SelectToken("versus_killed_by_others").ToString() + Constants.vbCrLf;
            GetTokenRet += "对战断连:" + JSON.SelectToken(a).SelectToken("versus_disconnected").ToString() + Constants.vbCrLf;
            GetTokenRet += "===多人合作===" + Constants.vbCrLf;
            GetTokenRet += "合作通关:" + JSON.SelectToken(a).SelectToken("coop_plays").ToString() + Constants.vbCrLf;
            GetTokenRet += "合作游玩:" + JSON.SelectToken(a).SelectToken("coop_clears").ToString() + Constants.vbCrLf;
            GetTokenRet += "===拥有奖牌===" + Constants.vbCrLf;
            if (JSON.SelectToken(a).SelectToken("badges").Count() > 0)
            {
                foreach (JObject K in JSON.SelectToken(a).SelectToken("badges"))
                    // GetToken += K.SelectToken("type").ToString & ":" & K.SelectToken("rank").ToString & vbCrLf
                    GetTokenRet += ModuleSMM.BadgesType[(int)Math.Round(Conversion.Val(K.SelectToken("type").ToString()))] + ":" + ModuleSMM.Badges[(int)Math.Round(Conversion.Val(K.SelectToken("rank").ToString()))] + Constants.vbCrLf;
            }

            GetTokenRet += "=====服装=====" + Constants.vbCrLf;
            GetTokenRet += "帽子:" + JSON.SelectToken(a).SelectToken("hat").ToString() + Constants.vbCrLf;
            GetTokenRet += "衣服:" + JSON.SelectToken(a).SelectToken("shirt").ToString() + Constants.vbCrLf;
            GetTokenRet += "裤子:" + JSON.SelectToken(a).SelectToken("pants").ToString() + Constants.vbCrLf;
            return GetTokenRet;
        }

        public void LoadLvlInfo()
        {
            // On Error Resume Next
            string S;
            JObject JSON;
            var B = new Bitmap(260, 760);
            var G = Graphics.FromImage(B);
            Timer2.Enabled = false;
            Label2.Text = "";
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            TextBox9.Text = TextBox9.Text.Replace("-", "");
            if (TextBox9.Text.Length == 9)
            {
                S = GetPage("http://tgrcode.com/mm2/level_info/" + TextBox9.Text);
                TextBox2.Text = S;
                if (string.IsNullOrEmpty(S) | Strings.InStr(S, "Code corresponds to a maker") > 0)
                {
                    Text = "ID错误，请输入关卡ID";
                    return;
                }

                JSON = JObject.Parse(S);
                Label2.Text += "标题:" + JSON.SelectToken("name").ToString() + Constants.vbCrLf;
                Label2.Text += "描述:" + JSON.SelectToken("description").ToString() + Constants.vbCrLf;
                Label2.Text += "上传日期:" + JSON.SelectToken("uploaded_pretty").ToString() + Constants.vbCrLf;
                Label2.Text += "风格:" + JSON.SelectToken("game_style_name").ToString() + Constants.vbCrLf;
                Label2.Text += "主题:" + JSON.SelectToken("theme_name").ToString() + Constants.vbCrLf;
                Label2.Text += "难度:" + JSON.SelectToken("difficulty_name").ToString() + Constants.vbCrLf;
                Label2.Text += "标签:" + JSON.SelectToken("tags_name").ToString().Replace(Constants.vbCrLf, "") + Constants.vbCrLf;
                Label2.Text += "最短时间:" + JSON.SelectToken("world_record_pretty").ToString() + Constants.vbCrLf;
                Label2.Text += "上传时间:" + JSON.SelectToken("upload_time_pretty").ToString() + Constants.vbCrLf;
                // Label2.Text += "过关条件:" & JSON.SelectToken("clear_condition_name").ToString & vbCrLf
                Label2.Text += "过关:" + JSON.SelectToken("clears").ToString() + Constants.vbCrLf;
                Label2.Text += "尝试:" + JSON.SelectToken("attempts").ToString() + Constants.vbCrLf;
                Label2.Text += "过关率：" + JSON.SelectToken("clear_rate").ToString() + Constants.vbCrLf;
                Label2.Text += "游玩次数:" + JSON.SelectToken("plays").ToString() + Constants.vbCrLf;
                Label2.Text += "游玩人数:" + JSON.SelectToken("unique_players_and_versus").ToString() + Constants.vbCrLf;
                Label2.Text += "赞:" + JSON.SelectToken("likes").ToString() + Constants.vbCrLf;
                Label2.Text += "孬:" + JSON.SelectToken("boos").ToString() + Constants.vbCrLf;
                Label2.Text += "对战游玩:" + JSON.SelectToken("versus_matches").ToString() + Constants.vbCrLf;
                Label2.Text += "合作游玩:" + JSON.SelectToken("coop_matches").ToString() + Constants.vbCrLf;
                Label2.Text += "本周游玩:" + JSON.SelectToken("weekly_plays").ToString() + Constants.vbCrLf;
                Label2.Text += "本周点赞:" + JSON.SelectToken("weekly_likes").ToString() + Constants.vbCrLf;
                Label3.Text = "关卡作者" + GetToken(JSON, "uploader");
                Label4.Text = "最先通过" + GetToken(JSON, "first_completer");
                Label5.Text = "最短时间" + GetToken(JSON, "record_holder");
                if (File.Exists(ModuleSMM.PT + @"\MII\" + JSON.SelectToken("uploader").SelectToken("code").ToString()))
                {
                    PicM0.Image = Image.FromFile(ModuleSMM.PT + @"\MII\" + JSON.SelectToken("uploader").SelectToken("code").ToString());
                }
                else
                {
                    string argszURL = JSON.SelectToken("uploader").SelectToken("mii_image").ToString().Replace("width=512&instanceCount=1", "width=128&instanceCount=16");
                    string argszFileName = ModuleSMM.PT + @"\MII\" + JSON.SelectToken("uploader").SelectToken("code").ToString();
                    Form1.URLDownloadToFile(0, argszURL, argszFileName, 0, 0);
                    // PicM0.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("uploader").SelectToken("code").ToString)
                }

                if (File.Exists(ModuleSMM.PT + @"\MII\" + JSON.SelectToken("first_completer").SelectToken("code").ToString()))
                {
                }
                // PicM1.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("first_completer").SelectToken("code").ToString)
                else
                {
                    string argszURL1 = JSON.SelectToken("first_completer").SelectToken("mii_image").ToString().Replace("width=512&instanceCount=1", "width=128&instanceCount=16");
                    string argszFileName1 = ModuleSMM.PT + @"\MII\" + JSON.SelectToken("first_completer").SelectToken("code").ToString();
                    Form1.URLDownloadToFile(0, argszURL1, argszFileName1, 0, 0);
                    // PicM1.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("first_completer").SelectToken("code").ToString)
                }

                if (File.Exists(ModuleSMM.PT + @"\MII\" + JSON.SelectToken("record_holder").SelectToken("code").ToString()))
                {
                }
                // PicM2.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("record_holder").SelectToken("code").ToString)
                else
                {
                    string argszURL2 = JSON.SelectToken("record_holder").SelectToken("mii_image").ToString().Replace("width=512&instanceCount=1", "width=128&instanceCount=16");
                    string argszFileName2 = ModuleSMM.PT + @"\MII\" + JSON.SelectToken("record_holder").SelectToken("code").ToString();
                    Form1.URLDownloadToFile(0, argszURL2, argszFileName2, 0, 0);
                    // PicM2.Image = Image.FromFile(PT & "\MII\" & JSON.SelectToken("record_holder").SelectToken("code").ToString)
                }

                MiiCache[0] = Image.FromFile(ModuleSMM.PT + @"\MII\" + JSON.SelectToken("uploader").SelectToken("code").ToString());
                MiiCache[1] = Image.FromFile(ModuleSMM.PT + @"\MII\" + JSON.SelectToken("first_completer").SelectToken("code").ToString());
                MiiCache[2] = Image.FromFile(ModuleSMM.PT + @"\MII\" + JSON.SelectToken("record_holder").SelectToken("code").ToString());
                Timer2.Enabled = true;
            }
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            LoadLvlInfo();
        }

        public float GetStrW(string s)
        {
            float GetStrWRet = default;
            var B = new Bitmap(300, 100);
            var G = Graphics.FromImage(B);
            SizeF SZ;
            //SZ = G.MeasureString(s, Button11.Font);
            //GetStrWRet = SZ.Width;
            //return GetStrWRet;
            Console.WriteLine("Error here");
            return 0;
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            LoadLvlInfo();
        }

        private int MiiF;
        private Image[] MiiCache = new Image[3];
        private Bitmap[] MiiB = new Bitmap[3];
        private Graphics[] MiiG = new Graphics[3];

        private void Button15_Click(object sender, EventArgs e)
        {
            Timer2.Enabled = true;
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            MiiG[0].Clear(Color.Transparent);
            MiiG[0].DrawImage(MiiCache[0], new Rectangle(0, 0, 128, 128), new Rectangle(MiiF * 128, 0, 128, 128), GraphicsUnit.Pixel);
            PicM0.Image = MiiB[0];
            MiiG[1].Clear(Color.Transparent);
            MiiG[1].DrawImage(MiiCache[1], new Rectangle(0, 0, 128, 128), new Rectangle(MiiF * 128, 0, 128, 128), GraphicsUnit.Pixel);
            PicM1.Image = MiiB[1];
            MiiG[2].Clear(Color.Transparent);
            MiiG[2].DrawImage(MiiCache[2], new Rectangle(0, 0, 128, 128), new Rectangle(MiiF * 128, 0, 128, 128), GraphicsUnit.Pixel);
            PicM2.Image = MiiB[2];
            MiiF += 1;
            MiiF = MiiF % 16;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private Graphics GG;
        private Bitmap GB;
        private Image Tile;
        private int TileW;

        public Image GetTile(int x, int y, int w, int h)
        {
            Image GetTileRet = default;
            GB = new Bitmap(TileW * w, TileW * h);
            GG = Graphics.FromImage(GB);
            GG.DrawImage(Tile, new Rectangle(0, 0, TileW * w, TileW * h), new Rectangle(TileW * x, TileW * y, TileW * w, TileW * h), GraphicsUnit.Pixel);
            GetTileRet = GB;
            return GetTileRet;
        }
    }
}