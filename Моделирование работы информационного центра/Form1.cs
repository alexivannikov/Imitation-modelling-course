using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Моделирование_информационного_центра
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double timeInt = double.Parse(textBox1.Text.Replace('.', ','));

                Model model = new Model(timeInt);

                model.Restart();

                while (model.NumHandledRequests < 300)
                {
                    model.MoveOn();
                }

                double p = (double)model.NumDeclinedRequests / model.NumHandledRequests;

                label2.Text = "Количество поступивших заявок: " + model.NumHandledRequests.ToString();
                label3.Text = "Количество заявок, получивших отказ: " + model.NumDeclinedRequests.ToString();
                label4.Text = "Время работы системы: " + String.Format("{0:0.####}", model.CurrentTime);
                label5.Text = "Вероятность отказа: " + String.Format("{0:0.####}", p);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
