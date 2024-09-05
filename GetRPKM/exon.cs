using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetRPKM
{
    internal class exon
    {
        private string name;
        private int x;
        private int y;

        public exon(string Name, int X, int Y) 
        {
            name = Name;
            x = X;
            y = Y;
        }

        public string Name { get { return name; } }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

    }
}
