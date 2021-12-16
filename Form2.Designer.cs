using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace SMM2VIEWER
{
    [DesignerGenerated()]
    public partial class Form2 : Form
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
            _P = new PictureBox();
            _P.MouseDown += new MouseEventHandler(P_MouseDown);
            _P.MouseUp += new MouseEventHandler(P_MouseUp);
            _P.MouseMove += new MouseEventHandler(P_MouseMove);
            _ToolTip1 = new ToolTip(components);
            _ToolTip1.Popup += new PopupEventHandler(ToolTip1_Popup);
            _ToolTip1.Draw += new DrawToolTipEventHandler(ToolTip1_Draw);
            ((System.ComponentModel.ISupportInitialize)_P).BeginInit();
            SuspendLayout();
            // 
            // P
            // 
            _P.Location = new Point(0, 0);
            _P.Name = "_P";
            _P.Size = new Size(100, 100);
            _P.TabIndex = 0;
            _P.TabStop = false;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(6.0f, 12.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 441);
            Controls.Add(_P);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "表世界";
            ((System.ComponentModel.ISupportInitialize)_P).EndInit();
            ResumeLayout(false);
        }

        private PictureBox _P;

        internal PictureBox P
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _P;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_P != null)
                {
                    _P.MouseDown -= P_MouseDown;
                    _P.MouseUp -= P_MouseUp;
                    _P.MouseMove -= P_MouseMove;
                }

                _P = value;
                if (_P != null)
                {
                    _P.MouseDown += P_MouseDown;
                    _P.MouseUp += P_MouseUp;
                    _P.MouseMove += P_MouseMove;
                }
            }
        }

        private ToolTip _ToolTip1;

        internal ToolTip ToolTip1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolTip1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ToolTip1 != null)
                {
                    _ToolTip1.Popup -= ToolTip1_Popup;
                    _ToolTip1.Draw -= ToolTip1_Draw;
                }

                _ToolTip1 = value;
                if (_ToolTip1 != null)
                {
                    _ToolTip1.Popup += ToolTip1_Popup;
                    _ToolTip1.Draw += ToolTip1_Draw;
                }
            }
        }
    }
}