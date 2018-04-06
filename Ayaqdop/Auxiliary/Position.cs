using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayaqdop.Auxiliary
{
    public struct Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int column)
            : this()
        {
            this.Row = row;
            this.Column = column;
        }
    }
}
