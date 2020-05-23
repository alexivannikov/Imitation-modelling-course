using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ГПСЧ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            textBox2.Text = "5";
            textBox3.Text = "3";
            textBox4.Text = "8";
            textBox5.Text = "10";
            textBox6.Text = "1";
        }

        public int[] LCG()
        {
            int a = Convert.ToInt32(textBox2.Text);
            int b = Convert.ToInt32(textBox3.Text);
            int m = Convert.ToInt32(textBox4.Text);
            int n = Convert.ToInt32(textBox5.Text);
            int seed = Convert.ToInt32(textBox6.Text);

            int[] x = new int[n];

            x[0] = seed;

            for (int i = 1; i < n; i++)
            {
                x[i] = (a * x[i - 1] + b) % m;
            }

            for (int i = 0; i < n; i++)
            {
                listBox1.Items.Add(x[i].ToString());
            }

            double C = 0;

            int num1 = 0, num2 = 0, denom = 0;

            num2 = x[0];
            denom = n * x[0] * x[0];

            for (int i = 0; i < n - 1; i++)
            {
                num1 = num1 + n * x[i] * x[i + 1];
                num2 = num2 + x[i + 1];
                denom = denom + n * x[i + 1] * x[i + 1];
            }

            C = (double)(num1 - num2 * num2 + n * x[n - 1] * x[0]) / (double)(denom - num2 * num2);

            textBox13.Text = (C.ToString());
            textBox1.Text = (((1 - Math.Abs(C))*100).ToString());

            return x;
        }

        public void tableInput()
        {
            string path = "Random digits table.txt";

            string[] sNums = File.ReadAllText(path).Split(new char[] { ' ', '\n' });

            int[] x = new int[sNums.Length - 1];

            for (int i = 0; i < x.Length; i++)
            {
                x[i] = Convert.ToInt32(sNums[i]);
            }

           int n = Convert.ToInt32(textBox9.Text);

            for (int i = 0; i < n; i++)
            {
                listBox3.Items.Add(x[i].ToString());
            }

            double C = 0;

            int num1 = 0, num2 = 0, denom = 0;

            num2 = x[0];
            denom = n * x[0] * x[0];

            for (int i = 0; i < n - 1; i++)
            {
                num1 = num1 + n * x[i] * x[i + 1];
                num2 = num2 + x[i + 1];
                denom = denom + n * x[i + 1] * x[i + 1];
            }

            C = (double)(num1 - num2 * num2 + n * x[n - 1] * x[0]) / (double)(denom - num2 * num2);

            textBox11.Text = (C.ToString());
            textBox10.Text = (((1 - Math.Abs(C)) * 100).ToString());
        }

        public int[] manualInput()
        {
            string[] sNums = textBox8.Text.Split(' ');

            int[] x = new int[sNums.Length];

            for (int i = 0; i < x.Length; i++)
            {
                x[i] = Convert.ToInt32(sNums[i]);
            }

            for (int i = 0; i < x.Length; i++)
            {
                listBox2.Items.Add(x[i].ToString());
            }

            double C = 0;

            int num1 = 0, num2 = 0, denom = 0;

            num2 = x[0];
            denom = x.Length * x[0] * x[0];

            for (int i = 0; i < x.Length - 1; i++)
            {
                num1 = num1 + x.Length * x[i] * x[i + 1];
                num2 = num2 + x[i + 1];
                denom = denom + x.Length * x[i + 1] * x[i + 1];
            }

            C = (double)(num1 - num2 * num2 + x.Length * x[x.Length - 1] * x[0]) / (double)(denom - num2 * num2);

            textBox12.Text = (C.ToString());
            textBox7.Text = (((1 - Math.Abs(C)) * 100).ToString());

            return x;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            LCG();
            tableInput();
            manualInput();
        }
    }
}
