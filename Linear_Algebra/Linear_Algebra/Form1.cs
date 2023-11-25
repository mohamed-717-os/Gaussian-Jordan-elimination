using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Linear_Algebra
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox2.Focus();
            }

            if (textBox1.Text != "")
            {
                textBox2.Text = $"{int.Parse(textBox1.Text) + 1}";
            }
        }
        int rows;
        int columns;
        double[,] arr;
        string[] MYtxts;

        private void button1_Click(object sender, EventArgs e)
        {
            int x_axis = 350;
            int y_axis = 100;

            try
            {
                this.rows = Convert.ToInt32(textBox1.Text);
                this.columns = Convert.ToInt32(textBox2.Text);
            }
            catch
            {
                MessageBox.Show($"You must enter an integer number");
            }
            

            this.arr = new double[this.rows, this.columns];

            if (MYtxts != null)
            {
                foreach (var item in MYtxts)
                {
                    var Old_item = this.Controls.Find($"{item}", true).FirstOrDefault();
                    this.Controls.Remove(Old_item);
                }
            }
            MYtxts = new string[rows * columns +2];
            int MYtxts_idx = 0;

            ///////////// The Raised lable /////////////
            
            System.Windows.Forms.Label title = new System.Windows.Forms.Label();
            this.Controls.Add(title);
            title.Text = "Enter The Matrix Data";
            title.Top = y_axis-50;
            title.Left = x_axis-10;
            title.Width = 200;
            title.Font = new Font("Mongolian Baiti", 15);

            ///////////// The Raised text boxes /////////////

            for (int r = 0; r < this.rows; r++)
            {
                for (int c = 0; c < this.columns; c++)
                {
                    TextBox txt = new TextBox();
                    this.Controls.Add(txt);
                    txt.Name = $"txt{r}{c}";

                    MYtxts[MYtxts_idx] = txt.Name;
                    MYtxts_idx++;

                    txt.Top = y_axis;
                    txt.Left = x_axis;
                    txt.Height = 40;
                    txt.Width = 40;
                    x_axis += 50;

                }
                x_axis = 350;
                y_axis += 50;
            }

            ///////////// The Raised button /////////////
            ///
            Button btn = new Button();
            this.Controls.Add(btn);
            btn.Top = y_axis;
            btn.Left = x_axis;
            btn.Name = "Guassian";

            MYtxts[MYtxts_idx] = btn.Name;
            MYtxts_idx++;
            btn.Text = "Guassian";
            btn.Font = new Font("Calibri", 15);
            btn.Height = 40;
            btn.Width = 150;
            btn.BackColor = Color.Azure;
            btn.Font = new Font("Mongolian Baiti", 15);

            btn.Click += new EventHandler(Guassian);

            //////////////////////////////////////////

            Button btn2 = new Button();
            this.Controls.Add(btn2);
            btn2.Top = y_axis + 50;
            btn2.Left = x_axis;
            btn2.Name = "Guassian-Jordn";

            MYtxts[MYtxts_idx] = btn2.Name;

            btn2.Text = "Guassian-Jordn";
            btn2.Font = new Font("Calibri", 15);
            btn2.Height = 40;
            btn2.Width = 150;
            btn2.BackColor = Color.Azure;
            btn2.Font = new Font("Mongolian Baiti", 15);

            btn2.Click += new EventHandler(Guassian_Jordn);
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
            
        }

        private void Guassian(object sender, EventArgs e)
        {
            for (int ro = 0; ro < this.rows; ro++)
            {
                for (int co = 0; co < this.columns; co++)
                {
                    try
                    {
                        TextBox txb = this.Controls.Find($"txt{ro}{co}", true).FirstOrDefault() as TextBox;
                        this.arr[ro, co] = Convert.ToDouble(txb.Text);
                    }
                    catch
                    {
                        MessageBox.Show("You must write all boxes with an integer ");
                        return;
                    }
                    
                }
            }

            string solution = Solve(this.arr, "g");

            Form2 f2 = new Form2();
            f2.answer = solution.Replace("\n", "\r\n\t");

            f2.ShowDialog();
        }

        private void Guassian_Jordn(object sender, EventArgs e)
        {
            for (int ro = 0; ro < this.rows; ro++)
            {
                for (int co = 0; co < this.columns; co++)
                {
                    try
                    {
                        TextBox txb = this.Controls.Find($"txt{ro}{co}", true).FirstOrDefault() as TextBox;
                        this.arr[ro, co] = Convert.ToDouble(txb.Text);
                    }
                    catch
                    {
                        MessageBox.Show("You must write all boxes with an integer ");
                        return;
                    }

                }
            }

            string solution = Solve(this.arr, "g-j");

            Form2 f2 = new Form2();
            f2.answer = solution.Replace("\n", "\r\n\t");

            f2.ShowDialog();
        }


        static String Solve(double[,] matrix, string method)
        {
            string result = $"\nThe Original Matrix\n{GoodMatrix(matrix)}\n";
            string line = new string('_', 50) + "\n\n";
            result += line;
            int step = 1;
            int row_zeros = 1;


            ////////////////////// Guassian ////////////////////////

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                int LeadingColumn = 0;
                int zeros = 0;
                int FirstNoneZero = row;
                if (matrix[row, FirstNoneZero] == 0)
                {
                    matrix = Order(matrix, row, FirstNoneZero, ref result, step);
                    result += line;
                    step++;
                }

                for (int column = 0; column < matrix.GetLength(1); column++)
                {
                    if (matrix[row, column] == 1)
                    {
                        break;
                    }
                    else if (matrix[row, column] == 0)
                    {
                        zeros++;
                        continue;
                    }
                    else
                    {
                        double factor = matrix[row, column];
                        LeadingColumn = column;

                        for (int divcol = 0; divcol < matrix.GetLength(1); divcol++)
                        {
                            matrix[row, divcol] = matrix[row, divcol] / factor;
                        }
                        int f = factor.ToString().Length;
                        result += $"{new string(' ', (14 + f / 2))}{1}\nStep {step}: {new string('-', 2 + f)} R[{row + 1}] --> R[{row + 1}]\n{new string(' ', 14)}{factor}\n{GoodMatrix(matrix)}\n";
                        result += line;
                        step++;
                        break;
                    }
                }

                if (zeros == matrix.GetLength(1))
                {
                    row_zeros = -1;
                }
                else if (zeros == matrix.GetLength(1) - 1 && matrix[row, matrix.GetLength(1) - 1] != 0)
                {
                    row_zeros = 0;
                }

                for (int nex_rows = row + 1; nex_rows < matrix.GetLength(0); nex_rows++)
                {
                    double MakeZero = matrix[nex_rows, LeadingColumn];
                    if (MakeZero != 0)
                    {
                        for (int num = 0; num < matrix.GetLength(1); num++)
                        {
                            matrix[nex_rows, num] = matrix[nex_rows, num] - MakeZero * matrix[row, num];
                        }
                        result += $"Step {step}: ({-MakeZero}) R[{row + 1}] + R[{nex_rows + 1}] --> R[{nex_rows + 1}]\n{GoodMatrix(matrix)}\n";
                        result += line;
                        step++;
                    }
                }
            }

            //////////////////// Guassian_Jordn ////////////////////////

            if (row_zeros == 1)
            {
                for (int last_row = matrix.GetLength(0) - 1; last_row >= 0; last_row--)
                {
                    double leading_factor = 0;
                    for (int last_col = 0; last_col < matrix.GetLength(1); last_col++)
                    {
                        if (matrix[last_row, last_col] == 1)
                        {
                            for (int rest_rows = last_row - 1; rest_rows >= 0; rest_rows--)
                            {
                                leading_factor = matrix[rest_rows, last_col];
                                for (int n = 0; n < matrix.GetLength(1); n++)
                                {
                                    matrix[rest_rows, n] = matrix[rest_rows, n] - matrix[last_row, n] * leading_factor;
                                }
                                if (method == "g-j")
                                {
                                    result += $"Step {step}: ({-leading_factor}) R[{last_row + 1}] + R[{rest_rows + 1}] --> R[{rest_rows + 1}]\n{GoodMatrix(matrix)}\n";
                                    result += line;
                                    step++;
                                }
                                
                            }
                            break;
                        }
                    }

                }
                if (matrix.GetLength(1) == matrix.GetLength(0)+1)
                {
                    result += $"There is 1 solution: \n";
                    for (int x = 0; x < matrix.GetLength(0); x++)
                    {
                        result += $"X{x + 1} = {matrix[x, matrix.GetLength(1) - 1]}\n";
                    }
                }
                

            }
            else if (row_zeros == 0 && matrix.GetLength(1) == matrix.GetLength(0) + 1)
            {
                result += $"There is no solutions\n";
            }
            else if(matrix.GetLength(1) == matrix.GetLength(0) + 1)
            {
                result += $"There is infinit solutions\n";
            }

            result += line;
            return result;
        }

        static string GoodMatrix(double[,] matrix)
        {
            string MyMatrix = "\n";


            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                MyMatrix += "|";
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    int len = $"{matrix[row, col]}".Length;
                    try
                    {
                        MyMatrix += new string(' ', (9 - len) / 2) + $"{matrix[row, col]}" + new string(' ', 9 - len - (9 - len) / 2) + "|";
                    }
                    catch
                    {
                        MyMatrix +=  $" {matrix[row, col]} " + "|";

                    }
                }
                MyMatrix += "\n\n";
            }

            return MyMatrix;
        }

        static double[,] Order(double[,] matrix, int my_row, int my_indx, ref string result, int step)
        {
            double[,] NewArray = (double[,])matrix.Clone();

            for (int r = my_row + 1; r < matrix.GetLength(0); r++)
            {
                if (matrix[r, my_indx] != 0)
                {
                    for (int z_col = 0; z_col < matrix.GetLength(1); z_col++)
                    {
                        NewArray[my_row, z_col] = matrix[r, z_col];
                        for (int z_row = my_row; z_row < r; z_row++)
                        {
                            NewArray[z_row + 1, z_col] = matrix[z_row, z_col];
                        }
                    }
                    result += $"Step {step}: Move R[{r + 1}]  --> R[{my_row + 1}]\n{GoodMatrix(NewArray)}\n";
                    break;
                }

            }
            return NewArray;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
