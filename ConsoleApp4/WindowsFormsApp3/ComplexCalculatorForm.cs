using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComplexCalculator
{
    public partial class ComplexCalculatorForm : Form
    {
        private TextBox txtReal1, txtImag1, txtReal2, txtImag2;
        private TextBox txtResult;
        private ComboBox comboOperation;
        private Button btnCalculate;
        private Button btnClear;
        private Label[] labels;

        public ComplexCalculatorForm()
        {
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.SuspendLayout();

            this.Text = "Калькулятор комплексных чисел";
            this.Size = new Size(500, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScaleMode = AutoScaleMode.Font;

            labels = new Label[7];
            labels[0] = new Label { Text = "Первое комплексное число:", Location = new Point(20, 20), Size = new Size(180, 20) };
            labels[1] = new Label { Text = "Действительная часть:", Location = new Point(40, 50), Size = new Size(140, 20) };
            labels[2] = new Label { Text = "Мнимая часть:", Location = new Point(40, 80), Size = new Size(100, 20) };

            labels[3] = new Label { Text = "Второе комплексное число:", Location = new Point(20, 120), Size = new Size(180, 20) };
            labels[4] = new Label { Text = "Действительная часть:", Location = new Point(40, 150), Size = new Size(140, 20) };
            labels[5] = new Label { Text = "Мнимая часть:", Location = new Point(40, 180), Size = new Size(100, 20) };

            labels[6] = new Label { Text = "Операция:", Location = new Point(20, 220), Size = new Size(70, 20) };

            txtReal1 = new TextBox { Location = new Point(180, 50), Size = new Size(80, 20) };
            txtImag1 = new TextBox { Location = new Point(180, 80), Size = new Size(80, 20) };

            txtReal2 = new TextBox { Location = new Point(180, 150), Size = new Size(80, 20) };
            txtImag2 = new TextBox { Location = new Point(180, 180), Size = new Size(80, 20) };

            comboOperation = new ComboBox { Location = new Point(100, 220), Size = new Size(100, 20) };
            comboOperation.Items.AddRange(new string[] { "Сложение", "Вычитание", "Умножение", "Деление" });
            comboOperation.SelectedIndex = 0;

            btnCalculate = new Button { Text = "Вычислить", Location = new Point(220, 220), Size = new Size(80, 25) };
            btnClear = new Button { Text = "Очистить", Location = new Point(310, 220), Size = new Size(80, 25) };

            Label lblResult = new Label { Text = "Результат:", Location = new Point(20, 260), Size = new Size(70, 20) };
            txtResult = new TextBox { Location = new Point(100, 260), Size = new Size(250, 20), ReadOnly = true };

            btnCalculate.Click += BtnCalculate_Click;
            btnClear.Click += BtnClear_Click;

            this.Controls.AddRange(labels);
            this.Controls.AddRange(new Control[] {
                txtReal1, txtImag1, txtReal2, txtImag2,
                comboOperation, btnCalculate, btnClear,
                lblResult, txtResult
            });

            this.ResumeLayout(false);
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            try
            {
                ComplexNumber num1 = new ComplexNumber(
                    double.Parse(txtReal1.Text),
                    double.Parse(txtImag1.Text)
                );

                ComplexNumber num2 = new ComplexNumber(
                    double.Parse(txtReal2.Text),
                    double.Parse(txtImag2.Text)
                );

                ComplexNumber result = comboOperation.SelectedItem.ToString() switch
                {
                    "Сложение" => num1 + num2,
                    "Вычитание" => num1 - num2,
                    "Умножение" => num1 * num2,
                    "Деление" => num1 / num2,
                    _ => throw new InvalidOperationException("Неизвестная операция")
                };

                txtResult.Text = result.ToString();
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Деление на ноль невозможно!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вычисления: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtReal1.Clear();
            txtImag1.Clear();
            txtReal2.Clear();
            txtImag2.Clear();
            txtResult.Clear();
            comboOperation.SelectedIndex = 0;
            txtReal1.Focus();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtReal1.Text) || string.IsNullOrWhiteSpace(txtImag1.Text) ||
                string.IsNullOrWhiteSpace(txtReal2.Text) || string.IsNullOrWhiteSpace(txtImag2.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!double.TryParse(txtReal1.Text, out _) || !double.TryParse(txtImag1.Text, out _) ||
                !double.TryParse(txtReal2.Text, out _) || !double.TryParse(txtImag2.Text, out _))
            {
                MessageBox.Show("Введите корректные числовые значения!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }

    public class ComplexNumber
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(a.Real + b.Real, a.Imaginary + b.Imaginary);
        }

        public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(a.Real - b.Real, a.Imaginary - b.Imaginary);
        }

        public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(
                a.Real * b.Real - a.Imaginary * b.Imaginary,
                a.Real * b.Imaginary + a.Imaginary * b.Real
            );
        }

        public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
        {
            double denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
            if (denominator == 0)
                throw new DivideByZeroException();

            return new ComplexNumber(
                (a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator,
                (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator
            );
        }

        public override string ToString()
        {
            if (Imaginary >= 0)
                return $"{Real} + {Imaginary}i";
            else
                return $"{Real} - {Math.Abs(Imaginary)}i";
        }
    }
}