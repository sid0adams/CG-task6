using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionDrawLib
{
    class NodeValue : INode
    {
        public bool ContainsX => false;
        public double Value { get; }
        public double GetX(double X) => Value;

        public NodeValue(double value) => Value = value;
    }
}
