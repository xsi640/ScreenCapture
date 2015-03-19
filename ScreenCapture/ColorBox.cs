using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ScreenCapture
{
    [Designer(typeof(ColorBoxDesginer))]
    public partial class ColorBox : Control
    {
        #region 变量
        private Color _SelectedColor;
        private Point _CurrentPoint;
        private Rectangle _RectSelected;
        private Bitmap _ColorImage = Properties.Resources.color;
        private Color _LastColor;
        #endregion

        #region 事件
        public delegate void ColorChangedHandler(object sender, ColorChangedEventArgs e);

        public event ColorChangedHandler ColorChanged;
        #endregion

        #region 构造函数
        public ColorBox()
        {
            InitializeComponent();
            this._SelectedColor = Color.Red;
            this._RectSelected = new Rectangle(-100, -100, 14, 14);

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        #endregion

        #region 属性
        public Color SelectedColor
        {
            get { return this._SelectedColor; }
        }
        #endregion

        protected virtual void OnColorChanged(ColorChangedEventArgs e)
        {
            if (this.ColorChanged != null)
                this.ColorChanged(this, e);
        }

        protected override void OnClick(EventArgs e)
        {
            Color clr = this._ColorImage.GetPixel(this._CurrentPoint.X, this._CurrentPoint.Y);
            if (clr.ToArgb() != Color.FromArgb(255, 254, 254, 254).ToArgb()
                && clr.ToArgb() != Color.FromArgb(255, 133, 141, 151).ToArgb()
                && clr.ToArgb() != Color.FromArgb(255, 110, 126, 149).ToArgb())
            {
                if (this._SelectedColor != clr)
                    this._SelectedColor = clr;
                this.Invalidate();
                this.OnColorChanged(new ColorChangedEventArgs(clr));
            }
            base.OnClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this._CurrentPoint = e.Location;
            try
            {
                Color clr = this._ColorImage.GetPixel(this._CurrentPoint.X, this._CurrentPoint.Y);
                if (clr != this._LastColor)
                {
                    if (clr.ToArgb() != Color.FromArgb(255, 254, 254, 254).ToArgb()
                        && clr.ToArgb() != Color.FromArgb(255, 133, 141, 151).ToArgb()
                        && clr.ToArgb() != Color.FromArgb(255, 110, 126, 149).ToArgb()
                        && e.X > 39)
                    {
                        this._RectSelected.Y = e.Y > 17 ? 17 : 2;
                        this._RectSelected.X = ((e.X - 39) / 15) * 15 + 38;
                        this.Invalidate();
                    }
                    else
                    {
                        this._RectSelected.X = this._RectSelected.Y = -100;
                        this.Invalidate();
                    }
                }
                this._LastColor = clr;
            }
            finally
            {
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this._RectSelected.X = this._RectSelected.Y = -100;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(Properties.Resources.color, new Rectangle(0, 0, 165, 35));
            g.DrawRectangle(Pens.SteelBlue, 0, 0, 164, 34);
            SolidBrush sb = new SolidBrush(this._SelectedColor);
            g.FillRectangle(sb, 9, 5, 24, 24);
            g.DrawRectangle(Pens.DarkCyan, this._RectSelected);
            base.OnPaint(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, 165, 35, specified);
        }
    }
}
