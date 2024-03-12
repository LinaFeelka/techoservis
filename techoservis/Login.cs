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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace techoservis
{
    public partial class Login : Form
    {
        Database Database = new Database();
        private bool closed = false;
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            if (closed)
            {
                return;
            }
            else if (CheckLogin(username, password))
            {
                ShowUserRoleForm(username);
            }

            /* ApplicationForm application = new ApplicationForm(); //менеджер
             application.Show();
             this.Close(); 

             DoApplication doApplication = new DoApplication(); //исполнитель
             doApplication.Show();
             this.Close();*/
        }
        string connectingString = "Server = localhost; Port = 5432; Database = technoservis; User Id = postgres; Password = assaq123;";

        private bool CheckLogin(string username, string password)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                string query = "SELECT COUNT(*) FROM users WHERE login = @username AND password = @password";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", password);

                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        private void ShowUserRoleForm(string username)
        {
            string role = GetUserRole(username);

            if (role == "Менеджер")
            {
                ApplicationForm application = new ApplicationForm(username, role); //менеджер
                application.Show();
                this.Close();
            }
            else if (role == "Исполнитель")
            {
                DoApplication doApplication = new DoApplication(username, role); //исполнитель
             doApplication.Show();
             this.Close();
            }
            else
            {
                MessageBox.Show("ошибка: неизвестная роль пользователя", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Hide();
        }

        private string GetUserRole(string username)
        {
            string role = "";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                string query = "SELECT role FROM users WHERE login = @username";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            role = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("не удалось получить роль пользователя", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ошибка при получении роли пользователя: {ex.Message}", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return role;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }
    }
}
