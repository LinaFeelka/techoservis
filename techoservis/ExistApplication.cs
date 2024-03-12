using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace techoservis
{
    public partial class ExistApplication : Form
    {
        Database database = new Database();
        public ExistApplication()
        {
            InitializeComponent();
        }

        private void ExistApplication_Load(object sender, EventArgs e)
        {
            CreateColumn();
            RefreshDataGrid(dataGridView1);
        }

        private void CreateColumn()
        {
            dataGridView1.Columns.Add("client", "ФИО");             //0
            dataGridView1.Columns.Add("number", "Номер заявки");    //1
            dataGridView1.Columns.Add("date_app", "Дата");          //2
            dataGridView1.Columns.Add("equipment", "Оборудование"); //3
            dataGridView1.Columns.Add("fault_type", "Тип поломки"); //4
            dataGridView1.Columns.Add("status", "Статус");          //5
            dataGridView1.Columns.Add("email", "E-mail");           //6
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string query = $"select client, number, date_app, equipment, fault_type, status, email from application";
            NpgsqlCommand comm = new NpgsqlCommand(query, database.GetConnection());

            database.OpenConnection();

            NpgsqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dataGridView1, reader);
            }reader.Close();

            database.CloseConnection();
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetString(0), record.GetInt64(1), record.GetDateTime(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetString(6));
        }


        private void button2_Click(object sender, EventArgs e)
        {
            AddApplication application = new AddApplication();
            application.Show();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string query = $"select client, number, date_app, equipment, fault_type, status, email from application where concat (client, number, date_app, email) like '%" + textBox1.Text +"%'";

            NpgsqlCommand comm = new NpgsqlCommand(query , database.GetConnection());

            database.OpenConnection();

            NpgsqlDataReader read = comm.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }
            read.Close();

            database.CloseConnection();
        }
    }
}
