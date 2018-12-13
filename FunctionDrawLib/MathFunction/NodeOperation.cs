using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionDrawLib
{
    class NodeOperation : INode
    {
        public Operation Operation { get; private set; }
        public INode Left { get; private set; }
        public INode Right { get; private set; }
        public bool ContainsX => Left.ContainsX || Right.ContainsX;
        public double GetX(double X)
        {
            switch (Operation)
            {
                case Operation.Plus:
                    return Left.GetX(X) + Right.GetX(X);
                case Operation.Minus:
                    return Left.GetX(X) - Right.GetX(X);
                case Operation.Multiply:
                    return Left.GetX(X) * Right.GetX(X);
                case Operation.Divide:
                    return Left.GetX(X) / Right.GetX(X);
                case Operation.Degree:
                    return Math.Pow(Left.GetX(X), Right.GetX(X));
            }
            throw new Exception("Function NodeOperation Error");
        }
        public NodeOperation(Operation operation, INode left, INode right)
        {
            Operation = operation;
            Left = left;
            Right = right;
        }
        public void Optimize()
        {
            if (!Right.ContainsX)
                Right = new NodeValue(Right.GetX(0));
            else if (Right is NodeOperation O)
                O.Optimize();
            if (!Left.ContainsX)
                Left = new NodeValue(Left.GetX(0));
            else if (Left is NodeOperation O)
                O.Optimize();
        }
    }
    public enum Operation
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        Degree
    }
}
