using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;
using System.CodeDom;

namespace IpekYolu_denenme1
{

    public partial class Form1 : Form
    {
        String[] portlar = SerialPort.GetPortNames();
        float Default_value = 0.1f, Min = 0.0f, Max = 1.0f;
        float Default_value2 = 0.1f, Min2 = 0.0f, Max2= 1.0f;
        public float sıcaklık;
        bool mouse = false;
        bool mouse2 = false;
        public float Bar(float value)
        {
            return (slider.Width - 24) * (value - Min) / (float)(Max - Min);
        }
        public float Bar2(float value)
        {
            return (slider2.Width - 24) * (value - Min2) / (float)(Max2-Min2);
        }
        public void thumb(float value)
        {
            if (value < Min) value = Min;
            if (value > Max) value = Max;
            Default_value = value;
            slider.Refresh();
        }
        public void thumb2(float value)
        {
            if (value < Min2) value = Min2;
            if (value > Max2) value = Max2;
            Default_value2 = value;
            slider2.Refresh();
        }
        public float slider_With(int x)
        {
            return Min + (Max - Min) * x / (float)(slider.Width);
        }
        public float slider_With2(int x)
        {
            return Min2 + (Max2 - Min2) * x / (float)(slider2.Width);
        }
        private Timer refreshTimer;
        public Form1()
        {
            InitializeComponent();
            slider.Height = 30;
            txtNemDegeri.Text = (Default_value * 100).ToString();
            txtRuzgarDegeri.Text = (Default_value2 * 100).ToString();
            ///////////////////////////////////////////////////////////////
            refreshTimer = new Timer();
            refreshTimer.Interval = 1000;
            refreshTimer.Tick += new EventHandler(timer1_Tick);
            refreshTimer.Start();
            if (serialPort1.IsOpen== false)
            {
                return;
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox1.Text);

            }
            try
            {
                serialPort1.Open();

            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
            try
            {
                String sonuc = serialPort1.ReadExisting();
;
               float.TryParse(sonuc, out sıcaklık);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                timer1.Stop();
                
            }
        }

        private void slide_MouseDown(object sender, MouseEventArgs e)
        {
            mouse = true;
            thumb(slider_With(e.X));
        }
        private void slider2_mouseDown(object sender, MouseEventArgs e)
        {
            mouse2 = true;
            thumb2(slider_With2(e.X));
        }
        private void slide_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouse)
            {
                return;
            }
            thumb(slider_With(e.X));
        }
        private void slider2_mouseMove(object sender, MouseEventArgs e)
        {
            if (!mouse2)
            {
                return;
            }
            thumb2(slider_With2(e.X));
        }
        private void slide_MouseUp(object sender, MouseEventArgs e)
        {
            mouse = false;
            txtNemDegeri.Text = (Default_value*100).ToString();
        }

        private void slider2_mouseUp(object sender, MouseEventArgs e)
        {
            mouse2 = false;
            txtRuzgarDegeri.Text = (Default_value2 * 100).ToString();
        }


        private void slide_Paint(object sender, PaintEventArgs e)
        {
            float bar_Size = 0.45f;
            float x = Bar(Default_value);
            int y = (int)(slider.Height * bar_Size);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(Brushes.DimGray, 0, y, slider.Width, y / 2);
            e.Graphics.FillRectangle(Brushes.Red, 0, y, x, slider.Height - 2 * y);
            using (Pen pen = new Pen(Color.White, 5))
            {
                e.Graphics.DrawEllipse(pen, x + 4, y - 6, slider.Height / 2, slider.Height / 2);
                e.Graphics.FillEllipse(Brushes.Red, x + 4, y - 6, slider.Height / 2, slider.Height / 2);
            }
            using (Pen pen = new Pen(Color.White, 5))
            {
                e.Graphics.DrawEllipse(pen, x + 4, y - 6, slider.Height / 2, slider.Height/2);
            }

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void slider2_Paint(object sender, PaintEventArgs e)
        {
            float bar_Size = 0.45f;
            float x = Bar(Default_value2);
            int y = (int)(slider2.Height * bar_Size);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(Brushes.DimGray, 0, y, slider.Width, y / 2);
            e.Graphics.FillRectangle(Brushes.Red, 0, y, x, slider.Height - 2 * y);
            using (Pen pen = new Pen(Color.White, 5))
            {
                e.Graphics.DrawEllipse(pen, x + 4, y - 6, slider.Height / 2, slider.Height / 2);
                e.Graphics.FillEllipse(Brushes.Red, x + 4, y - 6, slider.Height / 2, slider.Height / 2);
            }
            using (Pen pen = new Pen(Color.White, 5))
            {
                e.Graphics.DrawEllipse(pen, x + 4, y - 6, slider.Height / 2, slider.Height / 2);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen==true)
            {
                serialPort1.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] categories = { "sıcaklık", "rüzgar", "sıcaklık" };
            float[] data = { Default_value*100, Default_value2*100, sıcaklık };
            Chart chart = chart1;
            chart.Series[0].Points.Clear();
            for (int i = 0; i < categories.Length; i++)
            {
                chart.Series[0].Points.AddXY(categories[i], data[i]);
            }
        }
        
        private void txtGoster_TextChanged(object sender, EventArgs e)
        {
            
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
            foreach (String port in portlar)
            {
                comboBox1.Items.Add(port);
                comboBox1.SelectedIndex = 0;
            }
            comboBox2.Items.Add("115200");
            comboBox2.Items.Add("4800");
            comboBox2.Items.Add("9600");
            comboBox2.SelectedIndex = 0;


        }
        

        


    }
}
