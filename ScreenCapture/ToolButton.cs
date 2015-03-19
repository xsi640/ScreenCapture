using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace ScreenCapture
{
    [Designer(typeof(ToolButtonDesigner))]
    public partial class ToolButton : Control
    {
        private Image _Image;
        private bool _IsSelectedButton;
        private bool _IsSingleSelectedButton;
        private bool _IsSelected;
        private bool _IsMouseEnter;

        public ToolButton()
        {
            InitializeComponent();
        }

        public Image Image
        {
            get { return this._Image; }
            set
            {
                this._Image = value;
                this.Invalidate();
            }
        }

        public bool IsSelectedButton
        {
            get { return this.IsSelectedButton; }
            set
            {
                this._IsSelectedButton = value;
                if (!this._IsSelectedButton)
                    this._IsSingleSelectedButton = false;
            }
        }

        public bool IsSingleSelectedButton
        {
            get { return this._IsSingleSelectedButton; }
            set
            {
                this._IsSingleSelectedButton = value;
                if (this._IsSingleSelectedButton)
                    this._IsSelectedButton = true;
            }
        }

        public bool IsSelected
        {
            get { return this._IsSelected; }
            set
            {
                if (value == this._IsSelected)
                    return;
                this._IsSelected = value;
                this.Invalidate();
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                Size size = TextRenderer.MeasureText(this.Text, this.Font);
                this.Width = size.Width + 21;
            }
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            this._IsMouseEnter = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this._IsMouseEnter = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (this._IsSelectedButton)
            {
                if (this._IsSelected)
                {
                    if (!this._IsSingleSelectedButton)
                    {
                        this._IsSelected = false;
                        this.Invalidate();
                    }
                }
                else
                {
                    this._IsSelected = true;
                    this.Invalidate();
                    for (int i = 0, len = this.Parent.Controls.Count; i < len; i++)
                    {
                        if (this.Parent.Controls[i] is ToolButton && this.Parent.Controls[i] != this)
                        {
                            if (((ToolButton)(this.Parent.Controls[i]))._IsSelected)
                                ((ToolButton)(this.Parent.Controls[i])).IsSelected = false;
                        }
                    }
                }
            }
            this.Focus();
            base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            this.OnClick(e);
            base.OnDoubleClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (this._IsMouseEnter)
            {
                g.FillRectangle(Brushes.LightBlue, this.ClientRectangle);
                g.DrawRectangle(Pens.DarkCyan, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }
            if (this._Image == null)
                g.DrawImage(Properties.Resources.none, new Rectangle(2, 2, 17, 17));
            else
                g.DrawImage(this._Image, new Rectangle(2, 2, 17, 17));
            g.DrawString(this.Text, this.Font, Brushes.Black, 21, (this.Height - this.Font.Height) / 2 + 1);
            if (this._IsSelected)
                g.DrawRectangle(Pens.DarkCyan, new Rectangle(0, 0, this.Width - 1, this.Height - 1));

            base.OnPaint(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, TextRenderer.MeasureText(this.Text, this.Font).Width + 21, 21, specified);
        }
    }
}
