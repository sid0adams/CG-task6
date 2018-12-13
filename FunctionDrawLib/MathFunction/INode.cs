using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionDrawLib
{
    interface INode
    {
        bool ContainsX { get; }
        double GetX(double X);
    }
}
