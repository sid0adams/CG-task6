using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FunctionDrawLib;
namespace CG_task6
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        FunctionDraw functionDraw;
        private void MainForm_Load(object sender, EventArgs e)
        {
            functionDraw = new FunctionDraw(Output.Size, new RectangleF(-Output.Width / 80f, Output.Height / 80f, Output.Width / 40f, Output.Height / 40f));
            Output.MouseWheel += Output_MouseWheel;
            Upd();
        }

        private void Output_MouseWheel(object sender, MouseEventArgs e)
        {
            functionDraw.Scale(1 - 0.001f * e.Delta, e.X, e.Y);
            Upd();
        }

        private void Upd() => Output.Image = functionDraw.Draw();

        private void Input_TextChanged(object sender, EventArgs e)
        {
            try
            {
                functionDraw.Function = new Function(Input.Text);
            }
            catch (Exception)
            {
                functionDraw.Function = null;
            }
            Upd();
        }
        Point last = new Point();
        private void Output_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                functionDraw.Move(last.X - e.X, e.Y - last.Y);
            }
            last = e.Location;
            Upd();
        }
    }
}
