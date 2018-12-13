using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FunctionDrawLib
{
    public class FunctionDraw
    {
        public Size ScreenSize { get; private set; }
        public RectangleF View { get; private set; }

        Bitmap bitmap;
        public Function Function { get; set; }
        void DrawPoint(int X, int Y, Color color)
        {
            if (X >= 0 && Y >= 0 && X < bitmap.Width && Y < bitmap.Height)
                bitmap.SetPixel(X, Y, color);
        }
        void Screen2View(int Xs, int Ys, out double Xv, out double Yv)
        {
            Xv = Xs * View.Width / ScreenSize.Width + View.X;
            Yv = -Ys * View.Height / ScreenSize.Height + View.Y;
        }
        void View2Screen(double Xv, double Yv, out int Xs, out int Ys)
        {
            Xs = (int)Math.Round(((Xv - View.X) * ScreenSize.Width / View.Width));
            Ys = (int)Math.Round(((View.Y - Yv) / View.Height * ScreenSize.Height));
        }
        public Bitmap Draw()
        {
            bitmap?.Dispose();
            bitmap = new Bitmap(ScreenSize.Width, ScreenSize.Height);
            View2Screen(0, 0, out int Xs, out int Ys);
            if(0 < View.X+View.Width && 0> View.X)
                for (int Y = 0; Y < ScreenSize.Height; Y++)
                {
                    DrawPoint(Xs, Y,Color.Black);
                }
            if(0 < View.Y && 0 > View.Y - View.Height)
                for (int X = 0; X < ScreenSize.Width; X++)
                {
                    DrawPoint(X, Ys,Color.Black);
                }
            if (Function != null)
            {
                double LastXv = 0;
                double LastYv = 0;
                int LastXs = 0;
                int LastYs = 0;

                double NextXv = 0;
                double NextYv = 0;
                int NextXs = 0;
                int NextYs = 0;
                Screen2View(-1, 0, out LastXv, out LastYv);
                LastYv = Function.GetX(LastXv);
                View2Screen(LastXv, LastYv, out LastXs, out LastYs);
                for (int X = 0; X < ScreenSize.Width; X++)
                {
                    Screen2View(X, 0, out NextXv, out NextYv);
                    NextYv = Function.GetX(NextXv);
                    View2Screen(NextXv, NextYv, out NextXs, out NextYs);
                    if (!(LastYv > View.Y&& NextYv > View.Y || LastYv < (View.Y - View.Height) && NextYv < (View.Y - View.Height)))
                    {
                        for (int Y = Math.Max(0,Math.Min(LastYs, NextYs)); Y <= Math.Min(ScreenSize.Height -1, Math.Max(LastYs, NextYs)); Y++)
                        {
                            DrawPoint(X, Y,Color.Red);
                        }
                    }
                    LastXv = NextXv;
                    LastYv = NextYv;
                    LastXs = NextXs;
                    LastYs = NextYs;
                }
            }

            return bitmap;
        }

        public FunctionDraw(Size screenSize, RectangleF view)
        {
            ScreenSize = screenSize;
            View = view;
        }
        public void Move(int X, int Y)
        {
            float Dx = X * View.Width / ScreenSize.Width;
            float Dy = Y * View.Height / ScreenSize.Height;
            View = new RectangleF(View.X + Dx, View.Y + Dy, View.Width, View.Height);
        }
        public void Scale(float C, int x, int y)
        {
            Screen2View(x, y, out double Xv1, out double Yv1);
            View = new RectangleF(View.X, View.Y, C * View.Width, C * View.Height);
            Screen2View(x, y, out double Xv2, out double Yv2);
            View = new RectangleF((float)(View.X + Xv1 - Xv2),(float)( View.Y + Yv1 - Yv2), View.Width, View.Height);
        }
    }
}
