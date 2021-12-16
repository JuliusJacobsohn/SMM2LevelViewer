using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace SMM2VIEWER
{
    [DesignerGenerated()]
    public partial class Form1 : Form
    {

        // Form 重写 Dispose，以清理组件列表。
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Windows 窗体设计器所必需的
        private System.ComponentModel.IContainer components;

        // 注意: 以下过程是 Windows 窗体设计器所必需的
        // 可以使用 Windows 窗体设计器修改它。  
        // 不要使用代码编辑器修改它。
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            _Button1 = new Button();
            _Button1.Click += new EventHandler(Button1_Click);
            TextBox1 = new TextBox();
            TextBox2 = new TextBox();
            TextBox3 = new TextBox();
            TextBox4 = new TextBox();
            _Button2 = new Button();
            _Button2.Click += new EventHandler(Button2_Click);
            GroupBox1 = new GroupBox();
            _Button4 = new Button();
            _Button4.Click += new EventHandler(Button4_Click);
            TextBox9 = new TextBox();
            _Button5 = new Button();
            _Button5.Click += new EventHandler(Button5_Click);
            GroupBox2 = new GroupBox();
            _Button8 = new Button();
            _Button8.Click += new EventHandler(Button8_Click);
            _TrackBar1 = new TrackBar();
            _TrackBar1.Scroll += new EventHandler(TrackBar1_Scroll);
            Label1 = new Label();
            ListBox1 = new ListBox();
            ListBox2 = new ListBox();
            Label2 = new Label();
            PicBot = new PictureBox();
            Label3 = new Label();
            Label4 = new Label();
            Label5 = new Label();
            PicM0 = new PictureBox();
            PicM1 = new PictureBox();
            PicM2 = new PictureBox();
            _Timer2 = new Timer(components);
            _Timer2.Tick += new EventHandler(Timer2_Tick);
            GroupBox1.SuspendLayout();
            GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_TrackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBot).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicM0).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicM1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicM2).BeginInit();
            SuspendLayout();
            // 
            // Button1
            // 
            _Button1.Location = new Point(6, 20);
            _Button1.Name = "_Button1";
            _Button1.Size = new Size(70, 21);
            _Button1.TabIndex = 0;
            _Button1.Text = "LOAD";
            _Button1.UseVisualStyleBackColor = true;
            // 
            // TextBox1
            // 
            TextBox1.Location = new Point(82, 19);
            TextBox1.Name = "TextBox1";
            TextBox1.Size = new Size(327, 21);
            TextBox1.TabIndex = 1;
            TextBox1.Text = @"E:\VB\SMM2VIEWER\bin\Debug\MAP\Course_data_000";
            // 
            // TextBox2
            // 
            TextBox2.Location = new Point(6, 19);
            TextBox2.Multiline = true;
            TextBox2.Name = "TextBox2";
            TextBox2.Size = new Size(173, 300);
            TextBox2.TabIndex = 2;
            // 
            // TextBox3
            // 
            TextBox3.Location = new Point(185, 20);
            TextBox3.Multiline = true;
            TextBox3.Name = "TextBox3";
            TextBox3.Size = new Size(173, 300);
            TextBox3.TabIndex = 8;
            // 
            // TextBox4
            // 
            TextBox4.Location = new Point(364, 20);
            TextBox4.Multiline = true;
            TextBox4.Name = "TextBox4";
            TextBox4.Size = new Size(173, 300);
            TextBox4.TabIndex = 9;
            // 
            // Button2
            // 
            _Button2.Location = new Point(12, 39);
            _Button2.Name = "_Button2";
            _Button2.Size = new Size(100, 21);
            _Button2.TabIndex = 19;
            _Button2.Text = "Map A";
            _Button2.UseVisualStyleBackColor = true;
            // 
            // GroupBox1
            // 
            GroupBox1.Controls.Add(TextBox2);
            GroupBox1.Controls.Add(TextBox3);
            GroupBox1.Controls.Add(TextBox4);
            GroupBox1.Location = new Point(522, 66);
            GroupBox1.Name = "GroupBox1";
            GroupBox1.Size = new Size(549, 327);
            GroupBox1.TabIndex = 22;
            GroupBox1.TabStop = false;
            GroupBox1.Text = "GroupBox1";
            GroupBox1.Visible = false;
            // 
            // Button4
            // 
            _Button4.Location = new Point(260, 39);
            _Button4.Name = "_Button4";
            _Button4.Size = new Size(181, 21);
            _Button4.TabIndex = 23;
            _Button4.Text = "保存地图 Save image";
            _Button4.UseVisualStyleBackColor = true;
            // 
            // TextBox9
            // 
            TextBox9.BackColor = Color.FromArgb(Conversions.ToInteger(Conversions.ToByte(128)), Conversions.ToInteger(Conversions.ToByte(255)), Conversions.ToInteger(Conversions.ToByte(255)));
            TextBox9.CharacterCasing = CharacterCasing.Upper;
            TextBox9.Location = new Point(12, 12);
            TextBox9.Name = "TextBox9";
            TextBox9.Size = new Size(100, 21);
            TextBox9.TabIndex = 24;
            TextBox9.Text = "SQG-9NT-9GF";
            TextBox9.TextAlign = HorizontalAlignment.Center;
            TextBox9.WordWrap = false;
            // 
            // Button5
            // 
            _Button5.Location = new Point(118, 12);
            _Button5.Name = "_Button5";
            _Button5.Size = new Size(136, 21);
            _Button5.TabIndex = 25;
            _Button5.Text = "加载地图 Load Level";
            _Button5.UseVisualStyleBackColor = true;
            // 
            // GroupBox2
            // 
            GroupBox2.Controls.Add(_Button1);
            GroupBox2.Controls.Add(TextBox1);
            GroupBox2.Location = new Point(522, 8);
            GroupBox2.Name = "GroupBox2";
            GroupBox2.Size = new Size(497, 52);
            GroupBox2.TabIndex = 27;
            GroupBox2.TabStop = false;
            GroupBox2.Text = "test";
            GroupBox2.Visible = false;
            // 
            // Button8
            // 
            _Button8.Location = new Point(118, 39);
            _Button8.Name = "_Button8";
            _Button8.Size = new Size(100, 21);
            _Button8.TabIndex = 36;
            _Button8.Text = "Map B";
            _Button8.UseVisualStyleBackColor = true;
            // 
            // TrackBar1
            // 
            _TrackBar1.Location = new Point(260, 12);
            _TrackBar1.Maximum = 6;
            _TrackBar1.Minimum = 2;
            _TrackBar1.Name = "_TrackBar1";
            _TrackBar1.Size = new Size(104, 45);
            _TrackBar1.TabIndex = 39;
            _TrackBar1.Value = 4;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(370, 15);
            Label1.Name = "Label1";
            Label1.Size = new Size(71, 12);
            Label1.TabIndex = 40;
            Label1.Text = "缩放Zoom:16";
            // 
            // ListBox1
            // 
            ListBox1.FormattingEnabled = true;
            ListBox1.ItemHeight = 12;
            ListBox1.Location = new Point(12, 66);
            ListBox1.Name = "ListBox1";
            ListBox1.Size = new Size(100, 208);
            ListBox1.TabIndex = 42;
            // 
            // ListBox2
            // 
            ListBox2.FormattingEnabled = true;
            ListBox2.ItemHeight = 12;
            ListBox2.Location = new Point(118, 66);
            ListBox2.Name = "ListBox2";
            ListBox2.Size = new Size(100, 208);
            ListBox2.TabIndex = 43;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(229, 66);
            Label2.Name = "Label2";
            Label2.Size = new Size(47, 12);
            Label2.TabIndex = 48;
            Label2.Text = "LvlInfo";
            // 
            // PicBot
            // 
            PicBot.BackColor = Color.LightGray;
            PicBot.Location = new Point(12, 448);
            PicBot.Name = "PicBot";
            PicBot.Size = new Size(100, 100);
            PicBot.SizeMode = PictureBoxSizeMode.AutoSize;
            PicBot.TabIndex = 50;
            PicBot.TabStop = false;
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Location = new Point(12, 681);
            Label3.Name = "Label3";
            Label3.Size = new Size(47, 12);
            Label3.TabIndex = 51;
            Label3.Text = "LvlInfo";
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.Location = new Point(146, 681);
            Label4.Name = "Label4";
            Label4.Size = new Size(47, 12);
            Label4.TabIndex = 52;
            Label4.Text = "LvlInfo";
            // 
            // Label5
            // 
            Label5.AutoSize = true;
            Label5.Location = new Point(280, 681);
            Label5.Name = "Label5";
            Label5.Size = new Size(47, 12);
            Label5.TabIndex = 53;
            Label5.Text = "LvlInfo";
            // 
            // PicM0
            // 
            PicM0.BackColor = Color.Transparent;
            PicM0.Location = new Point(14, 550);
            PicM0.Name = "PicM0";
            PicM0.Size = new Size(128, 128);
            PicM0.SizeMode = PictureBoxSizeMode.StretchImage;
            PicM0.TabIndex = 55;
            PicM0.TabStop = false;
            // 
            // PicM1
            // 
            PicM1.BackColor = Color.Transparent;
            PicM1.Location = new Point(148, 550);
            PicM1.Name = "PicM1";
            PicM1.Size = new Size(128, 128);
            PicM1.SizeMode = PictureBoxSizeMode.StretchImage;
            PicM1.TabIndex = 56;
            PicM1.TabStop = false;
            // 
            // PicM2
            // 
            PicM2.BackColor = Color.Transparent;
            PicM2.Location = new Point(282, 550);
            PicM2.Name = "PicM2";
            PicM2.Size = new Size(128, 128);
            PicM2.SizeMode = PictureBoxSizeMode.StretchImage;
            PicM2.TabIndex = 57;
            PicM2.TabStop = false;
            // 
            // Timer2
            // 
            _Timer2.Interval = 60;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(6.0f, 12.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(453, 284);
            Controls.Add(PicM2);
            Controls.Add(PicM1);
            Controls.Add(PicM0);
            Controls.Add(Label5);
            Controls.Add(Label4);
            Controls.Add(Label3);
            Controls.Add(PicBot);
            Controls.Add(_Button8);
            Controls.Add(ListBox2);
            Controls.Add(ListBox1);
            Controls.Add(Label1);
            Controls.Add(TextBox9);
            Controls.Add(_Button4);
            Controls.Add(GroupBox1);
            Controls.Add(_Button2);
            Controls.Add(_Button5);
            Controls.Add(Label2);
            Controls.Add(_TrackBar1);
            Controls.Add(GroupBox2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "马里奥制造2关卡机器人 SMM2 Level Viewer v1.0";
            GroupBox1.ResumeLayout(false);
            GroupBox1.PerformLayout();
            GroupBox2.ResumeLayout(false);
            GroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_TrackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBot).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicM0).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicM1).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicM2).EndInit();
            Load += new EventHandler(Form1_Load);
            DragDrop += new DragEventHandler(Form1_DragDrop);
            DragEnter += new DragEventHandler(Form1_DragEnter);
            ResumeLayout(false);
            PerformLayout();
        }

        private Button _Button1;

        internal Button Button1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Button1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Button1 != null)
                {
                    _Button1.Click -= Button1_Click;
                }

                _Button1 = value;
                if (_Button1 != null)
                {
                    _Button1.Click += Button1_Click;
                }
            }
        }

        internal TextBox TextBox1;
        internal TextBox TextBox2;
        internal TextBox TextBox3;
        internal TextBox TextBox4;
        private Button _Button2;

        internal Button Button2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Button2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Button2 != null)
                {
                    _Button2.Click -= Button2_Click;
                }

                _Button2 = value;
                if (_Button2 != null)
                {
                    _Button2.Click += Button2_Click;
                }
            }
        }

        internal GroupBox GroupBox1;
        private Button _Button4;

        internal Button Button4
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Button4;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Button4 != null)
                {
                    _Button4.Click -= Button4_Click;
                }

                _Button4 = value;
                if (_Button4 != null)
                {
                    _Button4.Click += Button4_Click;
                }
            }
        }

        internal TextBox TextBox9;
        private Button _Button5;

        internal Button Button5
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Button5;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Button5 != null)
                {
                    _Button5.Click -= Button5_Click;
                }

                _Button5 = value;
                if (_Button5 != null)
                {
                    _Button5.Click += Button5_Click;
                }
            }
        }

        internal GroupBox GroupBox2;
        private Button _Button8;

        internal Button Button8
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Button8;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Button8 != null)
                {
                    _Button8.Click -= Button8_Click;
                }

                _Button8 = value;
                if (_Button8 != null)
                {
                    _Button8.Click += Button8_Click;
                }
            }
        }

        private TrackBar _TrackBar1;

        internal TrackBar TrackBar1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _TrackBar1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_TrackBar1 != null)
                {
                    _TrackBar1.Scroll -= TrackBar1_Scroll;
                }

                _TrackBar1 = value;
                if (_TrackBar1 != null)
                {
                    _TrackBar1.Scroll += TrackBar1_Scroll;
                }
            }
        }

        internal Label Label1;
        internal ListBox ListBox1;
        internal ListBox ListBox2;
        internal Label Label2;
        internal PictureBox PicBot;
        internal Label Label3;
        internal Label Label4;
        internal Label Label5;
        internal PictureBox PicM0;
        internal PictureBox PicM1;
        internal PictureBox PicM2;
        private Timer _Timer2;

        internal Timer Timer2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Timer2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Timer2 != null)
                {
                    _Timer2.Tick -= Timer2_Tick;
                }

                _Timer2 = value;
                if (_Timer2 != null)
                {
                    _Timer2.Tick += Timer2_Tick;
                }
            }
        }
    }
}