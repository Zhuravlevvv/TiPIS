using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace LabTiPIS
{
    public partial class FormMaterial : Form
    {
        private SQLiteConnection connect;
        private SQLiteCommand cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath,
        "C:\\SQLite\\TiPISLabs");
        public FormMaterial()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
";New=False;Version=3";
            String selectCommand = "Select * from Material";
            selectTable(ConnectionString, selectCommand);
            String selectType = "SELECT NumberAccount, NameAccount FROM ChartOfAccounts WHERE NumberAccount<11 AND NumberAccount>10";
            selectCombo(ConnectionString, selectType, toolStripComboBoxType, "NameAccount", "NumberAccount");
            toolStripComboBoxType.SelectedIndex = -1;
        }
        public void selectTable(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridViewProduct.DataSource = ds;
            dataGridViewProduct.DataMember = ds.Tables[0].ToString();
            connect.Close();
        }
        public void selectCombo(string ConnectionString, String selectCommand,
ToolStripComboBox comboBox, string displayMember, string valueMember)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            comboBox.ComboBox.DataSource = ds.Tables[0];
            comboBox.ComboBox.DisplayMember = displayMember;
            comboBox.ComboBox.ValueMember = valueMember;
            connect.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
            ";New=False;Version=3";
            String selectCommand = "select MAX(Code) from Material";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            if (toolStripComboBoxType.Text == "")
            {
                MessageBox.Show("Выберите тип");
                return;
            }
            string result = textBoxPrice.Text.ToString();
            string txtSQLQuery = "insert into Material (Code, Name, Type, CostPrice) values (" +
           (Convert.ToInt32(maxValue) + 1) + ",'" + textBoxName.Text + "','" + toolStripComboBoxType.ComboBox.Text + "', " + result + ")";
            ExecuteQuery(txtSQLQuery);
            selectCommand = "select * from Material";
            refreshForm(ConnectionString, selectCommand);
            textBoxName.Text = "";
            textBoxPrice.Text = "";
            toolStripComboBoxType.SelectedIndex = -1;
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
            dataGridViewProduct.Update();
            dataGridViewProduct.Refresh();
            textBoxName.Text = "";
            textBoxPrice.Text = "";
            toolStripComboBoxType.SelectedIndex = -1;
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            int CurrentRow = dataGridViewProduct.SelectedCells[0].RowIndex;
            string valueId = dataGridViewProduct[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from Material where Code =" + valueId;
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            selectCommand = "select * from Material";
            refreshForm(ConnectionString, selectCommand);
            textBoxName.Text = "";
            textBoxPrice.Text = "";
            toolStripComboBoxType.SelectedIndex = -1;
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
            int CurrentRow = dataGridViewProduct.SelectedCells[0].RowIndex;
            string valueId = dataGridViewProduct[0, CurrentRow].Value.ToString();
            string changeName = textBoxName.Text;
            string changeName1 = textBoxPrice.Text;
            string changeName2 = toolStripComboBoxType.ComboBox.Text;
            String selectCommand = "update Material set Name='" + changeName + "', Type='" + changeName2 + "', CostPrice='" + changeName1 + "'where Code = " + valueId;
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            selectCommand = "select * from Material";
            refreshForm(ConnectionString, selectCommand);
            textBoxName.Text = "";
            textBoxPrice.Text = "";
            toolStripComboBoxType.SelectedIndex = -1;
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != '\b' && l != '.' && l != ' ')
            {
                e.Handled = true;
            }
        }

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool dotCheck = false;
            char l = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && l != 8) //цифры, клавиша BackSpace и запятая а ASCII
            {
                if (dotCheck == false)
                {
                    e.Handled = true;
                }
            }
        }

        private void dataGridViewProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int CurrentRow = dataGridViewProduct.SelectedCells[0].RowIndex;
            string NameId = dataGridViewProduct[1, CurrentRow].Value.ToString();
            string TypeId = dataGridViewProduct[2, CurrentRow].Value.ToString();
            string PeiceId = dataGridViewProduct[3, CurrentRow].Value.ToString();
            textBoxName.Text = NameId;
            toolStripComboBoxType.ComboBox.Text = TypeId;
            textBoxPrice.Text = PeiceId;
        }

        private void textBoxPrice_TextChanged(object sender, EventArgs e)
        {
            string tmp = textBoxPrice.Text.Trim();
            string outS = string.Empty;
            bool zapyataya = true;
            foreach (char ch in tmp)
                if (Char.IsDigit(ch) || (ch == ',' && zapyataya))
                {
                    outS += ch;
                    if (ch == ',')
                        zapyataya = false;
                }
            textBoxPrice.Text = outS;
            textBoxPrice.SelectionStart = outS.Length;
        }
    }
}
