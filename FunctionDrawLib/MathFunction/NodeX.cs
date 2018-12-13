using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionDrawLib
{
    class NodeX : INode
    {
        public bool ContainsX => true;
        public double GetX(double X) => X;
    }
}
