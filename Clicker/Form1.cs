using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clicker
{
    public partial class Form1 : Form
    {
        [DllImport("user32")]
        public static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        private const int MOD_ALT = 0x1;
        private const int MOD_CONTROL = 0x2;
        private const int MOD_SHIFT = 0x4;
        private const int MOD_WIN = 0x8;
        private const int WM_HOTKEY = 0x312;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        Timer t = new Timer();
        int posX, posY, rate;

        public Form1()
        {
            InitializeComponent();

            RegisterHotKey(this.Handle, 42, MOD_ALT, (int)Keys.Z);
            RegisterHotKey(this.Handle, 43, MOD_ALT, (int)Keys.X);

            t.Interval = 20;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY)
            {
                switch (m.WParam.ToInt32())
                {
                    case 42:
                        if (t.Enabled == true)
                        {
                            t.Enabled = false;
                        }
                        else
                        {
                            t.Enabled = true;
                        }
                        break;

                    case 43:
                        button1.PerformClick();
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            posX = Convert.ToInt32(textBox1.Text);
            posY = Convert.ToInt32(textBox2.Text);
            rate = Convert.ToInt32(numericUpDown1.Value);

            Cursor.Position = new Point(posX, posY);

            for (int i = 0; i < rate; i++)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                
                if (radioButton2.Checked)
                    System.Threading.Thread.Sleep(1400);
                
                if (radioButton3.Checked)
                    System.Threading.Thread.Sleep(50);
            }
        }

        void t_Tick(object sender, EventArgs e)
        {
            textBox1.Text = MousePosition.X.ToString();
            textBox2.Text = MousePosition.Y.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 42);
            UnregisterHotKey(this.Handle, 43);
        }

        private void Form1_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            MessageBox.Show("В программе задействованы всплывающие подсказки — наведите курсор на интересующий элемент.\n\n\nГорячие клавиши:\n\nALT+Z — зафиксировать координаты\nALT+X — запустить");
        }
    }
}
