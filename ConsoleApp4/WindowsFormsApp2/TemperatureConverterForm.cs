using System;
using System.Drawing;
using System.Windows.Forms;

namespace TemperatureConverter
{
    public partial class TemperatureConverterForm : Form
    {
        private ComboBox comboFrom;
        private ComboBox comboTo;
        private TextBox textInput;
        private TextBox textResult;
        private Button btnConvert;
        private Button btnClear;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;

        public TemperatureConverterForm()
        {
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.SuspendLayout();

            this.Text = "Конвертер температур";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScaleMode = AutoScaleMode.Font;

            label1 = new Label { Text = "Из:", Location = new Point(20, 20), Size = new Size(30, 20) };
            comboFrom = new ComboBox { Location = new Point(60, 20), Size = new Size(100, 20) };

            label2 = new Label { Text = "В:", Location = new Point(180, 20), Size = new Size(30, 20) };
            comboTo = new ComboBox { Location = new Point(220, 20), Size = new Size(100, 20) };

            label3 = new Label { Text = "Температура:", Location = new Point(20, 60), Size = new Size(80, 20) };
            textInput = new TextBox { Location = new Point(110, 60), Size = new Size(100, 20) };

            label4 = new Label { Text = "Результат:", Location = new Point(20, 100), Size = new Size(70, 20) };
            textResult = new TextBox { Location = new Point(110, 100), Size = new Size(100, 20), ReadOnly = true };

            btnConvert = new Button { Text = "Конвертировать", Location = new Point(20, 140), Size = new Size(120, 30) };
            btnClear = new Button { Text = "Очистить", Location = new Point(160, 140), Size = new Size(80, 30) };

            string[] units = { "Цельсий", "Фаренгейт", "Кельвин" };
            comboFrom.Items.AddRange(units);
            comboTo.Items.AddRange(units);
            comboFrom.SelectedIndex = 0;
            comboTo.SelectedIndex = 1;

            btnConvert.Click += BtnConvert_Click;
            btnClear.Click += BtnClear_Click;

            this.Controls.AddRange(new Control[] {
                label1, comboFrom, label2, comboTo,
                label3, textInput, label4, textResult,
                btnConvert, btnClear
            });

            this.ResumeLayout(false);
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textInput.Text))
            {
                MessageBox.Show("Введите значение температуры!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(textInput.Text, out double temperature))
            {
                MessageBox.Show("Введите корректное числовое значение!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                double result = ConvertTemperature(temperature, comboFrom.SelectedItem.ToString(),
                                                 comboTo.SelectedItem.ToString());
                textResult.Text = result.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка конвертации: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            textInput.Clear();
            textResult.Clear();
            textInput.Focus();
        }

        private double ConvertTemperature(double value, string fromUnit, string toUnit)
        {
            double celsius = fromUnit switch
            {
                "Цельсий" => value,
                "Фаренгейт" => (value - 32) * 5 / 9,
                "Кельвин" => value - 273.15,
                _ => throw new ArgumentException("Неизвестная единица измерения")
            };

            return toUnit switch
            {
                "Цельсий" => celsius,
                "Фаренгейт" => celsius * 9 / 5 + 32,
                "Кельвин" => celsius + 273.15,
                _ => throw new ArgumentException("Неизвестная единица измерения")
            };
        }
    }
}