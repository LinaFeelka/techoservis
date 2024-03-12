using Npgsql;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace techoservis
{
    public partial class ApplicationForm : Form
    {
        readonly CheckUser checkUser;
        Database database = new Database();

        //string[] statuses = { "В ожидании", "В работе", "Выполнено", "Не выполнено" };
        string[] workers = { "Му Цин", "Инь Юй" };
        public ApplicationForm(string username, string role)
        {
            InitializeComponent();

            labelRole.Text = username + " : " + role;

            label4.Visible = false;
            textBox1.Visible = false;

            /*foreach (string status in statuses)
            {
                comboBox1.Items.Add(status);
            }*/

            foreach (string worker in workers)
            {
                comboBox2.Items.Add(worker);
            }
        }

        private void ApplicationForm_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefeshDataGrid(dataGridView1);
        }

        private void RefeshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string query = $"select * from application";
            NpgsqlCommand comm = new NpgsqlCommand(query, database.GetConnection());

            database.OpenConnection();

            NpgsqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();

            database.CloseConnection();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "ID");                           //0
            dataGridView1.Columns.Add("number", "Номер заявки");             //1
            dataGridView1.Columns.Add("date_app", "Дата заявки");            //2
            dataGridView1.Columns.Add("equipment", "Оборудование");          //3
            dataGridView1.Columns.Add("fault_type", "Тип поломки");          //4
            dataGridView1.Columns.Add("description", "Описание поломки");    //5
            dataGridView1.Columns.Add("client", "ФИО");                      //6
            dataGridView1.Columns.Add("comment", "Комментарий");             //7
            dataGridView1.Columns.Add("missing_parts", "Недостающие компл.");//8
            dataGridView1.Columns.Add("email", "E-mail");                    //9
            dataGridView1.Columns.Add("status", "Статус");                   //10
            dataGridView1.Columns.Add("executor", "Исполнитель");            //11
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetInt64(1), record.GetDateTime(2), record.GetString(3), record.GetString(4),
                record.GetString(5), record.GetString(6), record.GetString(7), record.GetString(8), record.GetString(9),
                record.GetString(10), record.GetString(11));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var status = comboBox1.SelectedItem.ToString();
            var executor = comboBox2.SelectedItem.ToString();
            var desc = textBox4.Text;
            var id = textBox1.Text;
           
            database.OpenConnection();
            //string query = $"update application set description = '{desc}', status = '{status}', executor = '{executor}'   where id_app = '{id}'";
            string query = $"update application set description = '{desc}', executor = '{executor}'   where id_app = '{id}'";

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, database.GetConnection());
            npgsqlCommand.ExecuteNonQuery();

            MessageBox.Show("Данные успешно добавлены!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            database.CloseConnection();

            RefeshDataGrid(dataGridView1);
        }
        int selectedRow = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox1.Text = row.Cells[0].Value.ToString();
                textBox4.Text = row.Cells[5].Value.ToString();
                //comboBox1.Text = row.Cells[10].Value.ToString();
                comboBox2.Text = row.Cells[11].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
