using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionDrawLib
{
    public class Function
    {
        INode Root { get; set;}
        public double GetY(double X) => Root.GetX(X);
        public Function(string str)
        {
            StringBuilder sb = new StringBuilder();
            int braces = 0;
            foreach (var item in str)
            {
                if (item == '(')
                    braces++;
                else if (item == ')')
                    braces--;
                else if (!(item >= '0' && item <= '9' || item == ',' || item == '.' || item == ' ' || item == 'x' || item == 'X' || item == '*' || item == '/' || item == '+' || item == '-' || item == '^') || braces < 0)
                    throw new Exception("not valid");
                if (item != ' ')
                    sb.Append(item);
            }
            if(braces!=0)
                throw new Exception("not valid");
            Root = GetNode(sb.ToString());
            
        }
        private INode GetNode(string str)
        {
            if(str.Length == 0)
                throw new Exception("not valid");
            int braces = 0;
            for (int i = str.Length -1; i >= 0; i--)
            {
                if (str[i] == '(')
                    braces--;
                else if (str[i] == ')')
                    braces++;
                else if(str[i] =='+'&& braces == 0)
                {
                    if (i == 0)
                        return GetNode(str.Substring(i + 1));
                    return new NodeOperation(Operation.Plus, GetNode(str.Substring(0, i)), GetNode(str.Substring(i + 1)));
                }
                else if (str[i] == '-' && braces == 0)
                {
                    return new NodeOperation(Operation.Minus, GetNode(i == 0?"0":str.Substring(0, i)), GetNode(str.Substring(i + 1)));
                }
            }

            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (str[i] == '(')
                    braces--;
                else if (str[i] == ')')
                    braces++;
                else if (str[i] == '*' && braces == 0)
                {
                    return new NodeOperation(Operation.Multiply, GetNode(str.Substring(0, i)), GetNode(str.Substring(i + 1)));
                }
                else if (str[i] == '/' && braces == 0)
                {
                    return new NodeOperation(Operation.Divide, GetNode(str.Substring(0, i)), GetNode(str.Substring(i + 1)));
                }
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '(')
                    braces++;
                else if (str[i] == ')')
                    braces--;
                else if (str[i] == '^' && braces == 0)
                {
                    return new NodeOperation(Operation.Degree, GetNode(str.Substring(0, i)), GetNode(str.Substring(i + 1)));
                }
            }

            if (str[0] == '(')
                return GetNode(str.Substring(1, str.Length - 2));
            if (str.ToUpper() == "X")
                return new NodeX();
            if (double.TryParse(str, out double res))
                return new NodeValue(res);
            throw new Exception("not valid");
        }
    }
}
