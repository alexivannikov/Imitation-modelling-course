using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace СМО_с_очередью
{
    public partial class Form1 : Form
    {
        public void Calculate()
        {
            /*Ввод исходных данных для расчета*/
            double timeStep = Convert.ToDouble(textBox1.Text); //Шаг повремени
            int numReqs = Convert.ToInt32(textBox2.Text); //Число заявок
            double a = Convert.ToDouble(textBox3.Text);
            double b = Convert.ToDouble(textBox4.Text);
            double M = Convert.ToDouble(textBox5.Text);
            double sigma = Convert.ToDouble(textBox6.Text);
            double retProb = Convert.ToDouble(textBox7.Text); //Вероятность возврата заявки

            UniformTimeRandomizer recvGen = new UniformTimeRandomizer(a, b);
            NormalTimeRandomizer handleGen = new NormalTimeRandomizer(M, sigma);
            DecisionRandomizer decRand = new DecisionRandomizer(retProb);

            ModelController modelController = null;

            if (radioButton1.Checked)
            {
                modelController = new TimeModelController(numReqs, recvGen, handleGen, decRand, timeStep);
            }
            else if (radioButton2.Checked)
            {
                textBox1.Visible = false;
                modelController = new EventModelController(numReqs, recvGen, handleGen, decRand);
            }

            modelController.Initialize();

            while (!modelController.Finished())
            {
                modelController.MoveOn();
            }

            textBox8.Text = modelController.MaxQueueLength.ToString();
        }

        private void OnMethodChanged(object sender, EventArgs e)
        {
            label2.Visible = radioButton1.Checked;
            textBox1.Visible = radioButton1.Checked;
        }

        public Form1()
        {
            InitializeComponent();

            radioButton1.CheckedChanged += OnMethodChanged;
            radioButton2.CheckedChanged += OnMethodChanged;

            textBox1.Text = "0,1".ToString();
            textBox5.Text = "0".ToString();
            textBox6.Text = "1".ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox8.Clear();

            Calculate();
        }
    }
}
