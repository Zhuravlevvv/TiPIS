using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace LabTiPIS
{
    public partial class FormChartOfAccounts : Form
    {
        private SQLiteConnection connect;
        private SQLiteCommand cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath,
        "C:\\SQLite\\TiPISLabs");

        public FormChartOfAccounts()
        {
            InitializeComponent();
        }

        private void ChartOfAccounts_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
            ";New=False;Version=3";
            String selectCommand = "Select * from ChartOfAccounts";
            selectTable(ConnectionString, selectCommand);
        }
        public void selectTable(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
            SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridView.DataSource = ds;
            dataGridView.DataMember = ds.Tables[0].ToString();
            connect.Close();
        }
    }
}
