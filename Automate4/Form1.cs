using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Timers;

namespace Automate4
{
    public partial class Form1 : Form
    {
        private static readonly Point[] Points = {
            new Point(415, 661),
            new Point(1575, 457),
            new Point(415, 661),
            new Point(415, 661),
            new Point(1575, 457),
            new Point(1575, 457),
        };

        public int NumberOfFailures;
        private static System.Timers.Timer _timer;

        public static void SetTimer()
        {
            _timer = new System.Timers.Timer { Interval = 10000, AutoReset = true, Enabled = true };
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetTimer();
            int numberOfFailures = 0;

            foreach (Point point in Points)
            {
                if (!MyResult(point, CalculateCounter(numberOfFailures)))
                    numberOfFailures++;
                else
                    numberOfFailures = 0;
            }
            _timer.Stop();
        }
        private static int CalculateCounter(int numberOfFailures) => (int)Math.Pow(2, numberOfFailures);

        public void DoClick(MouseOperations.MouseEventFlags events, int x, int y, int manyClick)
        {
            MouseOperations.SetCursorPosition(x, y);
            MouseOperations.MouseEvent(events);
            // MouseClick(clickSide, x, y, manyClick, speed);
        }

        public void Sleep(int millis)
        {
            Thread.Sleep(millis);
        }

        public bool MyResult(Point point, int counter)
        {
            Bitmap target = CaptureScreen();
            target.Save("test.bmp");
            if (SearchImage.Find(target, Properties.Resources.winningNumber, out Point _).IsEmpty) return false;

            DoClick(MouseOperations.MouseEventFlags.LeftDown, point.X, point.Y, counter);
            Sleep(15000);
            return true;
        }

        private Bitmap CaptureScreen()
        {
            Screen screen = Screen.FromControl(this);
            return SearchImage.CaptureWindow(SearchImage.GetDesktopWindow(), screen.Bounds.Width, screen.Bounds.Height);
        }
    }
}
