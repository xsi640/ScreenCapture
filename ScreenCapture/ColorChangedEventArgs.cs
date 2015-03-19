using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ScreenCapture
{
    public class ColorChangedEventArgs : EventArgs
    {
        private Color _Color;

        public Color Color
        {
            get { return this._Color; }
        }

        public ColorChangedEventArgs(Color clr)
        {
            this._Color = clr;
        }
    }
}
