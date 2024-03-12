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
using Npgsql;

namespace techoservis
{
    public partial class Report_form : Form
    {
        Database database = new Database();
        private Random random = new Random();
        string number, date_app, equipment, fault_type, client, executor, missing_parts;
        public Report_form(string number, string date_app, string equipment, string fault_type, string client, string executor, string missing_parts, Npgsql.NpgsqlConnection npgsqlConnection)
        {
            InitializeComponent();

            this.number = number;
            this.date_app = date_app;
            this.equipment = equipment;
            this.fault_type = fault_type;
            this.client = client;
            this.executor = executor;
            this.missing_parts = missing_parts;
            conn = npgsqlConnection;
        }
        private NpgsqlConnection conn;

        private void Report_form_Load(object sender, EventArgs e)
        {

        }
    }
}
