using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Процесс_Маркова
{
    public partial class Form1 : Form
    {
        public void Calculate()
        {
            int n = Convert.ToInt32(textBox1.Text);

            /*Двумерный массив для матрицы интенсивностей*/
            double[,] matrIntens = new double[n, n];

            /*Заполнение DataGridView столбцами*/
            DataGridViewColumn column; 

            for(int i = 0; i < n; i++)
            {
                column = new DataGridViewTextBoxColumn();

                /*Название столбца*/
                column.HeaderCell.Value = "S" + i.ToString();

                dataGridView1.Columns.Add(column);

                dataGridView1.Columns[i].Width = 30;
            }

            /*Заполнение DataGridView строками*/
            for (int i = 0; i < n; i++)
            {
                dataGridView1.Rows.Add();

                dataGridView1.Rows[i].HeaderCell.Value = "S" + i.ToString();
            }

            Random rnd = new Random();

            for (int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if(i == j)
                    {
                        matrIntens[i, j] = 0;
                    }
                    else
                    {
                        matrIntens[i, j] = rnd.Next(0, 4);
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    dataGridView1[i, j].Value = Convert.ToString(matrIntens[i, j]);
                }
            }

            double[,] resMatr = new double[n + 1, n + 1];

            for (int i = 0; i < n + 1; i++)
            {
                column = new DataGridViewTextBoxColumn();

                dataGridView2.Columns.Add(column);

                dataGridView2.Columns[i].Width = 30;
            }

            for (int i = 0; i < n + 1; i++)
            {
                dataGridView2.Rows.Add();
            }

            /*Левая часть уравнений Колмогорова: сумма произведений
              интенсивностей потоков, входящих в i-e состояние, на
              вероятности тех состояний, из которых эти потоки исходят*/
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    resMatr[i, i] = resMatr[i, i] - matrIntens[j, i];
                }
            }

            /*Правая часть уравнений Колмогорова: предельная вероятность i-го 
              состояния, умноженная на суммарную интенсивность всех потоков, 
              ведущих из данного состояния*/
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        resMatr[i, j] = matrIntens[j, i];
                    }
                }
            }

            /*Сумма предельных вероятностей состояний равна единице*/
            for (int i = 0; i < n + 1; i++)
            {
                resMatr[i, n] = 1;
            }

            for (int j = 0; j < n + 1; j++)
            {
                for (int i = 0; i < n + 1; i++)
                {
                    resMatr[j, i] = resMatr[j, i];
                }
            }

            for (int j = 0; j < n + 1; j++)
            {
                for (int i = 0; i < n + 1; i++)
                {
                    dataGridView2[j, i].Value = Convert.ToString(resMatr[j, i]);
                }
            }

            double[,] tempMatr = new double[n + 1, n];
           
            for (int i = 0; i < n + 1; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    tempMatr[i, j] = resMatr[j, i];
                }
            }

            double[,] reducedMatr = new double[n, n];

            int index = 0;

            for (int j = 0; j < n; j++)
            {
               tempMatr[0, j] = tempMatr[0, j] - tempMatr[1, j];
            }

            for (int i = 0; i < n + 1; i++)
            {
                if (i == 1)
                {
                    continue;
                }
                else
                {
                    for (int j = 0; j < n; j++)
                    {
                        reducedMatr[index, j] = tempMatr[i, j];
                    }
                    index++;
                }
            }

            double[] tempMas = new double[n + 1];

            for (int i = 0; i < n + 1; i++)
            {
                tempMas[i] = resMatr[n, i];
            }

            double[] reducedMas = new double[n];

            index = 0;

            reducedMas[0] = reducedMas[0] - reducedMas[1];

            for (int i = 0; i < n + 1; i++)
            {
                if (i == 1)
                {
                    continue;
                }
                else
                {
                    reducedMas[index] = tempMas[i];

                    index++;
                }
            }

            /*Метод Гаусса*/
            for (int k = 0; k < n - 1; k++)
            {
                 for (int i = k + 1; i < n; i++)
                 {
                    for (int j = k + 1; j < n; j++)
                    {
                        reducedMatr[i, j] = reducedMatr[i, j] - reducedMatr[k, j] * (reducedMatr[i, k] / reducedMatr[k, k]);
                    }
                    reducedMas[i] = reducedMas[i] - reducedMas[k] * reducedMatr[i, k] / reducedMatr[k, k];
                 }
            }

            double c = 0;

            double[] resMas = new double[n];

            for (int k = n - 1; k >= 0; k--)
            {
                c = 0;

                for (int j = k + 1; j < n; j++)
                {
                    c = c + reducedMatr[k, j] * resMas[j];
                }
                resMas[k] = (reducedMas[k] - c) / reducedMatr[k, k];
            }            

            for (int i = 0; i < n; i++)
            {
                column = new DataGridViewTextBoxColumn();

                column.HeaderCell.Value = "S" + i.ToString();

                dataGridView3.Columns.Add(column);

                dataGridView3.Columns[i].Width = 200;
            }

            for (int i = 0; i < 1; i++)
            {
                dataGridView3.Rows.Add();

                dataGridView3.Rows[i].HeaderCell.Value = "p";
            }

            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < 1; i++)
                {
                    dataGridView3[j, i].Value = Convert.ToString(resMas[j]);
                }
            }

            double[] timeMas = new double[n];

            double sum1 = 0, sum2 = 0;

            for (int i = 0; i < n; i++)
            {
                sum1 = 0;
                sum2 = 0;

                for (int j = 0; j < n; j++)
                {
                    sum1 = sum1 + matrIntens[i, j];
                    sum2 = sum2 + matrIntens[j, i];

                    timeMas[i] = Math.Abs((sum1 - sum2) / resMas[i]);
                }
            }

            for (int i = 0; i < n; i++)
            {
                column = new DataGridViewTextBoxColumn();

                column.HeaderCell.Value = "S" + i.ToString();

                dataGridView4.Columns.Add(column);

                dataGridView4.Columns[i].Width = 200;
            }

            for (int i = 0; i < 1; i++)
            {
                dataGridView4.Rows.Add();

                dataGridView4.Rows[i].HeaderCell.Value = "T";
            }

            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < 1; i++)
                {
                    dataGridView4[j, i].Value = Convert.ToString(timeMas[j]);
                }
            }
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Очистка DataGridView*/
            int k = dataGridView1.ColumnCount;

            if (k != 0)
            {
                for (int i = 0; i < k; i++)
                    dataGridView1.Columns.Clear();
                    dataGridView2.Columns.Clear();
                    dataGridView3.Columns.Clear();
                    dataGridView4.Columns.Clear();
            }

            Calculate();
        }

    }
}
