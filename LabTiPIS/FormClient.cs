using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace LabTiPIS
{
    public partial class FormClient : Form
    {
        private SQLiteConnection connect;
        private SQLiteCommand cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath,
        "C:\\SQLite\\TiPISLabs");
        public FormClient()
        {
            InitializeComponent();
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
";New=False;Version=3";
            String selectCommand = "Select * from Client";
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
            dataGridViewClient.DataSource = ds;
            dataGridViewClient.DataMember = ds.Tables[0].ToString();
            connect.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
";New=False;Version=3";
            String selectCommand = "select MAX(Code) from Client";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            string result = textBoxPhone.Text.ToString();
            string txtSQLQuery = "insert into Client (Code, FIO, Phone) values (" +
           (Convert.ToInt32(maxValue) + 1) + ",'" + textBoxFIO.Text + "'," + result + ")";
            ExecuteQuery(txtSQLQuery);
            selectCommand = "select * from Client";
            refreshForm(ConnectionString, selectCommand);
            textBoxFIO.Text = "";
            textBoxPhone.Text = "";
        }
        public object selectValue(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteCommand command = new SQLiteCommand(selectCommand, connect);
            SQLiteDataReader reader = command.ExecuteReader();
            object value = "";
            while (reader.Read())
            {
                value = reader[0];
            }
            connect.Close();
            return value;
        }

        private void ExecuteQuery(string txtQuery)
        {
            connect = new SQLiteConnection("Data Source=" + sPath +
           ";Version=3;New=False;Compress=True;");
            connect.Open();
            cmd = connect.CreateCommand();
            cmd.CommandText = txtQuery;
            cmd.ExecuteNonQuery();
            cmd.Cancel();
        }
        public void refreshForm(string ConnectionString, String selectCommand)
        {
            selectTable(ConnectionString, selectCommand);
            dataGridViewClient.Update();
            dataGridViewClient.Refresh();
            textBoxFIO.Text = "";
            textBoxPhone.Text = "";
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            int CurrentRow = dataGridViewClient.SelectedCells[0].RowIndex;
            string valueId = dataGridViewClient[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from Client where Code =" + valueId;
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            selectCommand = "select * from Client";
            refreshForm(ConnectionString, selectCommand);
            textBoxFIO.Text = "";
            textBoxPhone.Text = "";
        }
        public void changeValue(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteTransaction trans;
            SQLiteCommand cmd = new SQLiteCommand();
            trans = connect.BeginTransaction();
            cmd.Connection = connect;
            cmd.CommandText = selectCommand;
            cmd.ExecuteNonQuery();
            trans.Commit();
            connect.Close();
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            int CurrentRow = dataGridViewClient.SelectedCells[0].RowIndex;
            string valueId = dataGridViewClient[0, CurrentRow].Value.ToString();
            string changeName = textBoxFIO.Text;
            string changeName1 = textBoxPhone.Text;
            String selectCommand = "update Client set FIO='" + changeName + "', Phone='" + changeName1 + "'where Code = " + valueId;
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            selectCommand = "select * from Client";
            refreshForm(ConnectionString, selectCommand);
            textBoxFIO.Text = "";
            textBoxPhone.Text = "";
        }

        private void textBoxFIO_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != '\b' && l != '.' && l != ' ')
            {
                e.Handled = true;
            }
        }

        private void textBoxPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l <= 47 || l >= 58) && l != '\b' && l != '.')
            {
                e.Handled = true;
            }
        }

        private void dataGridViewClient_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int CurrentRow = dataGridViewClient.SelectedCells[0].RowIndex;
            string FIOId = dataGridViewClient[1, CurrentRow].Value.ToString();
            string PhoneId = dataGridViewClient[2, CurrentRow].Value.ToString();
            textBoxFIO.Text = FIOId;
            textBoxPhone.Text = PhoneId;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
