using System.Diagnostics;

namespace ScreenLaunch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("StartScreen");
            if (pname.Length >= 1)
            {
                this.Hide();
                Thread.Sleep(200);
                timer1.Stop();
                this.Close();
            }
            else
            {
                timer1.Stop();
                Process.Start(@"C:\Users\AND1558\source\repos\StartScreen\StartScreen\bin\Debug\net6.0-windows\StartScreen.exe");
                this.Close();
            }
        }
    }
}