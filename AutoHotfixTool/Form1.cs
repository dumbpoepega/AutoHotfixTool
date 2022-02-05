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
using System.Timers;
using System.IO;

namespace AutoHotfixTool
{
    public partial class Form1 : Form
    {
        private string hotfixPath;
        private string leaguePath;
        private string selfPath = AppDomain.CurrentDomain.BaseDirectory;
        public Form1()
        {
            InitializeComponent();
        }
        protected override void WndProc(ref Message m) // Make form movable
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;
    
    private void Form1_Load(object sender, EventArgs e)
        {
            System.Timers.Timer t1 = new System.Timers.Timer();
            t1.Elapsed += T1_Elapsed;
            t1.Enabled = true;
            t1.Interval = 500;
            t1.Start();
            if (!File.Exists(selfPath + "config.txt"))
            {
                File.Create(selfPath + "config.txt");
            }
            else
            {
                textBox1.Text = File.ReadAllLines(selfPath + "config.txt")[0];
            }
            if (!Directory.Exists(selfPath + "Original Files"))
            {
                Directory.CreateDirectory(selfPath + "Original Files");
            }
        }

        private void T1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (leagueOpened())
            {
                leagueLbl.ForeColor = Color.Lime;
                leagueLbl.Invoke((MethodInvoker)(() => leagueLbl.Text = "opened"));
            }
            else
            {
                leagueLbl.ForeColor = Color.Red;
                leagueLbl.Invoke((MethodInvoker)(() => leagueLbl.Text = "closed"));
            }
        }

        private bool leagueOpened()
        {
            Process[] leagueList = Process.GetProcessesByName("LeagueClient");
            if (leagueList.Length > 0)
            {
                return true;
            }
            return false;
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            if (leagueLbl.Text == "closed")
            {
                MessageBox.Show("League of Legends is closed. Please open it before applying hotfix.", "Error");
                return;
            }
            if (textBox1.Text == "" || textBox1.Text == String.Empty)
            {
                MessageBox.Show("Please enter your League of Legends.exe file path. Click on the textbox and select League Of Legends.com (not from Desktop but from C/D drive.)");
                return;
            }
            try
            {
                
                
                File.Copy(textBox1.Text + @"\League of Legends.exe", selfPath + @"\Original Files\League of Legends.exe");
                File.Copy(textBox1.Text + @"\stub.dll", selfPath + @"\Original Files\stub.dll");
            }
            catch
            {

            }

            string[] hotfixFiles = Directory.GetFiles(selfPath + @"\Hotfix\"); 
            string[] fileNames = { "League of Legends.exe", "stub.dll" };
            File.Delete(textBox1.Text + @"\League of Legends.exe");
            File.Delete(textBox1.Text + @"\stub.dll");
            for (int i = 0; i < hotfixFiles.Length; i++)
            {  
                try
                {
                    File.Copy(hotfixFiles[i], textBox1.Text + @"\" + fileNames[i], true); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error has occured. " + ex.Message);
                }
            }
            MessageBox.Show("Hotfix Applied.");
            hotfixLbl.ForeColor = Color.Lime;
            hotfixLbl.Text = "true";



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Click_1(object sender, EventArgs e)
        {
            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = ofd.SelectedPath;
                    using (StreamWriter wr = new StreamWriter(selfPath + "config.txt"))
                    {
                        wr.WriteLine(textBox1.Text);
                        
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy(selfPath + @"\Original Files\League of Legends.exe", textBox1.Text + @"\League of Legends.exe", true);
                File.Copy(selfPath + @"\Original Files\stub.dll", textBox1.Text + @"\stub.dll", true);
                MessageBox.Show("Successfully unloaded the hotfix.");
                hotfixLbl.Text = "false";
                hotfixLbl.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured. " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!leagueOpened())
            {
                MessageBox.Show("League of Legends is already closed.");
                return;
            }
            Process[] pList = Process.GetProcessesByName("LeagueClient");
            foreach (Process p in pList) { p.Kill(); }
        }

        private void label11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You have found an easter egg :D add me on discord for free eune acc to script on and send me a screenshot of this message window :D");
        }
    }
}
