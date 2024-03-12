using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace techoservis
{
    public partial class AddApplication : Form
    {
        Database database = new Database();
        public AddApplication()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExistApplication existApplication = new ExistApplication();
            existApplication.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.OpenConnection();
            // Получаем значения из richTextBox1
            var fio = textBox1.Text;
            var email = textBox5.Text;
            var equip = textBox2.Text;
            var type = comboBox1.Text;
            var descr = textBox4.Text;
            var num = generateOrderNum();
            DateTime date = DateTime.Now;

            // Сохраняем данные в базу данных
            string query = $"insert into application (number, date_app, equipment, fault_type, description, client, comment, missing_parts, email, status, executor) " +
                $"values ('{num}', '{date}', '{equip}', '{type}', '{descr}', '{fio}', '-', '-', '{email}', 'В ожидании', 'НАЗНАЧИТЬ')";

            NpgsqlCommand comm = new NpgsqlCommand(query, database.GetConnection());
            comm.ExecuteNonQuery();

            database.CloseConnection();

            MessageBox.Show("Заявка успешно оформлена и сохранена в базе данных.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            clearTB();
            
        }
        private string generateOrderNum()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssff");
        }
        private void clearTB()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            pictureBox1.Image = qrcode.Draw("https://docs.google.com/forms/d/1Am2gjwtnH-2fB8A3r7J_nfwGYK6vUd-3Q36BAyYnWs4/edit", 70);
        }
    }
}
