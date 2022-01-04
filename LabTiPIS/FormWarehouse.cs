using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace LabTiPIS
{
    public partial class FormWarehouse : Form
    {
        private SQLiteConnection connect;
        private SQLiteCommand cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath,
        "C:\\SQLite\\TiPISLabs");
        public FormWarehouse()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormWarehouse_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
";New=False;Version=3";
            String selectCommand = "Select * from Warehouse";
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
            dataGridViewWarehouse.DataSource = ds;
            dataGridViewWarehouse.DataMember = ds.Tables[0].ToString();
            connect.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
";New=False;Version=3";
            String selectCommand = "select MAX(Code) from Warehouse";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            string txtSQLQuery = "insert into Warehouse (Code, Name) values (" +
           (Convert.ToInt32(maxValue) + 1) + ",'" + textBoxName.Text + "')";
            ExecuteQuery(txtSQLQuery);
            selectCommand = "select * from Warehouse";
            refreshForm(ConnectionString, selectCommand);
            textBoxName.Text = "";
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
            dataGridViewWarehouse.Update();
            dataGridViewWarehouse.Refresh();
            textBoxName.Text = "";
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            int CurrentRow = dataGridViewWarehouse.SelectedCells[0].RowIndex;
            string valueId = dataGridViewWarehouse[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from Warehouse where Code =" + valueId;
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            selectCommand = "select * from Warehouse";
            refreshForm(ConnectionString, selectCommand);
            textBoxName.Text = "";
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

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int CurrentRow = dataGridViewWarehouse.SelectedCells[0].RowIndex;
            string valueId = dataGridViewWarehouse[0, CurrentRow].Value.ToString();
            string changeName = textBoxName.Text;
            String selectCommand = "update Warehouse set Name='" + changeName + "'where Code = " + valueId;
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            selectCommand = "select * from Warehouse";
            refreshForm(ConnectionString, selectCommand);
            textBoxName.Text = "";
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != '\b' && l != '.' && l != ' ')
            {
                e.Handled = true;
            }
        }

        private void dataGridViewWarehouse_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int CurrentRow = dataGridViewWarehouse.SelectedCells[0].RowIndex;
            string NameId = dataGridViewWarehouse[1, CurrentRow].Value.ToString();
            textBoxName.Text = NameId;
        }
    }
}
