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
            DrawXoY();
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
                LastYv = Function.GetY(LastXv);
                View2Screen(LastXv, LastYv, out LastXs, out LastYs);
                for (int X = 0; X < ScreenSize.Width; X++)
                {
                    Screen2View(X, 0, out NextXv, out NextYv);
                    NextYv = Function.GetY(NextXv);
                    View2Screen(NextXv, NextYv, out NextXs, out NextYs);
                    if (!(LastYv > View.Y && NextYv > View.Y || LastYv < (View.Y - View.Height) && NextYv < (View.Y - View.Height)))
                    {
                        for (int Y = Math.Max(0, Math.Min(LastYs, NextYs)); Y <= Math.Min(ScreenSize.Height - 1, Math.Max(LastYs, NextYs)); Y++)
                        {
                            DrawPoint(X, Y, Color.Red);
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

        private void DrawXoY()
        {
            int TextS = 10;
            Graphics g = Graphics.FromImage(bitmap);
            double t = View.Height * 100 / ScreenSize.Height;
            t = Math.Pow(10, Math.Round(Math.Log10(t)-0.2));
            View2Screen(0, 0, out int Xs, out int Ys);
            g.DrawString("0", new Font("Microsoft Sans Serif", TextS), Brushes.Black, Xs+ 5, Ys+5);
            if (0 < View.X + View.Width && 0 > View.X)
            {
                g.DrawLine(Pens.Black, Xs, 0, Xs, ScreenSize.Height);
                g.DrawString("y", new Font("Microsoft Sans Serif", TextS), Brushes.Black, Xs - 30, 5);
                double Y = t;
                while (Y < View.Y)
                {
                    View2Screen(0, Y, out int Xt, out int Yt);
                    g.DrawLine(Pens.Black, Xt - 10, Yt, Xt + 10, Yt);
                    g.DrawString(Y.ToString(), new Font("Microsoft Sans Serif", TextS), Brushes.Black, Xt + 5, Yt);
                    Y += t;
                }
                Y = -t;
                while (Y > View.Y - View.Height)
                {
                    View2Screen(0, Y, out int Xt, out int Yt);
                    g.DrawLine(Pens.Black, Xt - 10, Yt, Xt + 10, Yt);
                    g.DrawString(Y.ToString(), new Font("Microsoft Sans Serif", TextS), Brushes.Black, Xt + 5, Yt);
                    Y -= t;
                }
            }
            if (0 < View.Y && 0 > View.Y - View.Height)
            {
                g.DrawLine(Pens.Black, 0, Ys, ScreenSize.Width,Ys);
                g.DrawString("x", new Font("Microsoft Sans Serif", TextS), Brushes.Black, ScreenSize.Width - 15 , Ys - 30);
                double X = t;
                while (X < View.X + View.Width)
                {
                    View2Screen(X, 0, out int Xt, out int Yt);
                    g.DrawLine(Pens.Black, Xt, Yt - 10, Xt, Yt + 10);
                    g.DrawString(X.ToString(), new Font("Microsoft Sans Serif", TextS), Brushes.Black, Xt , Yt + 5);
                    X += t;
                }
                X = -t;
                while (X > View.X)
                {
                    View2Screen(X, 0, out int Xt, out int Yt);
                    g.DrawLine(Pens.Black, Xt, Yt - 10, Xt, Yt + 10);
                    g.DrawString(X.ToString(), new Font("Microsoft Sans Serif", TextS), Brushes.Black, Xt, Yt + 5);
                    X -= t;
                }
            }
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
