using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Printer.Cs.PageEntityInfo
{
    public class LineEntity
    {
        private float _Width = 1;

        public float Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        private decimal _s_x = 0;

        public decimal S_x
        {
            get { return _s_x; }
            set { _s_x = value; }
        }
        private decimal _s_y = 0;

        public decimal S_y
        {
            get { return _s_y; }
            set { _s_y = value; }
        }
        private decimal _e_x = 0;

        public decimal E_x
        {
            get { return _e_x; }
            set { _e_x = value; }
        }
        private decimal _e_y = 0;

        public decimal E_y
        {
            get { return _e_y; }
            set { _e_y = value; }
        }

    }
}
