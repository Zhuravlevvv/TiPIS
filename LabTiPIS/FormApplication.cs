using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace LabTiPIS
{
    public partial class FormApplication : Form
    {
        private SQLiteConnection connect;
        private SQLiteCommand cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath,
        "C:\\SQLite\\TiPISLabs");
        public FormApplication()
        {
            InitializeComponent();
        }

        private void FormApplication_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
            ";New=False;Version=3";
            String selectSubd = "SELECT Code, FIO FROM Client";
            selectCombo(ConnectionString, selectSubd, comboBoxClient, "FIO", "Code");
            comboBoxClient.SelectedIndex = -1;

            selectSubd = "SELECT Code, FIO FROM Employee";
            selectCombo(ConnectionString, selectSubd, comboBoxEmployee, "FIO", "Code");
            comboBoxEmployee.SelectedIndex = -1;

            selectSubd = "SELECT Code, Name FROM Material";
            selectCombo(ConnectionString, selectSubd, comboBoxProduct, "Name", "Code");
            comboBoxProduct.SelectedIndex = -1;
           
            String selectCommand = "Select * from Application";
            selectTable(ConnectionString, selectCommand);
            comboBoxClient.Text = "";
            comboBoxEmployee.Text = "";
            comboBoxProduct.Text = "";

            comboBoxSaleOnApplication.Items.Add("Да");
            comboBoxSaleOnApplication.Items.Add("Нет");
            comboBoxSaleOnApplication.SelectedIndex = -1;


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
            comboBoxClient.Text = ds.Tables[0].ToString();
            comboBoxEmployee.Text = ds.Tables[0].ToString();
            comboBoxProduct.Text = ds.Tables[0].ToString();          
            connect.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(textBoxNumber.Text);           
                string ConnectionString = @"Data Source=" + sPath +
 ";New=False;Version=3";
            String selectCommand = "select MAX(Code) from Application";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            if (comboBoxClient.SelectedValue == null && comboBoxEmployee.SelectedValue == null &&
              comboBoxProduct.SelectedValue == null)
            {
                MessageBox.Show("Не заполнены боксы");
                return;
            }
            if (number == 0)
            {              
                MessageBox.Show("Номер не может быть = 0");
                return;
            }
            else
            {
                string otgruzheno = comboBoxSaleOnApplication.Text;
                string txtSQLQuery = "insert into Application (Code, Date, NumberApplication, GoodsDeclaredForTheAmount," +
                "Count, CodeClient, CodeEmployee, CodeMaterial, SaleOnApplication) values (" +
          (Convert.ToInt32(maxValue) + 1) + ",'" + dateTimePicker.Value.ToString("yyyy-MM-dd") + "','" + textBoxNumber.Text + "','" + textBoxZav.Text + "','" + textBoxCount.Text + "'," +
          "'" + comboBoxClient.Text + "','" + comboBoxEmployee.Text + "','" + comboBoxProduct.Text + "', '" + otgruzheno + "')";
                ExecuteQuery(txtSQLQuery);
                selectCommand = "select * from Application";
                refreshForm(ConnectionString, selectCommand);
            }
            dateTimePicker.Text = "";
            textBoxNumber.Text = "";
            textBoxZav.Text = "";
            textBoxCount.Text = "";
            comboBoxClient.Text = "";
            comboBoxEmployee.Text = "";
            comboBoxProduct.Text = "";
            comboBoxSaleOnApplication.Text = "";
        }
        private void buttonDel_Click(object sender, EventArgs e)
        {
            int CurrentRow = dataGridView.SelectedCells[0].RowIndex;
            string valueId = dataGridView[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from Application where Code =" + valueId;
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            selectCommand = "select * from Application";
            refreshForm(ConnectionString, selectCommand);
            textBoxNumber.Text = "";
            textBoxZav.Text = "";
            textBoxCount.Text = "";
            comboBoxClient.Text = "";
            comboBoxEmployee.Text = "";
            comboBoxProduct.Text = "";
            comboBoxSaleOnApplication.Text = "";
        }
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(textBoxNumber.Text);
            int CurrentRow = dataGridView.SelectedCells[0].RowIndex;
            string valueId = dataGridView[0, CurrentRow].Value.ToString();
            if (number == 0 || comboBoxClient.SelectedValue == null && comboBoxEmployee.SelectedValue == null && comboBoxProduct.SelectedValue == null)
            {
                MessageBox.Show("Ошибки при заполнении полей!");
                return;
            }
            else
            {
                String selectCommand = "update Application set Date='" + dateTimePicker.Value.ToString("yyyy-MM-dd") +"', NumberApplication='" + textBoxNumber.Text + "'," +
                " GoodsDeclaredForTheAmount='" + textBoxZav.Text + "', Count='" + textBoxCount.Text + "', CodeClient='" + comboBoxClient.Text + "'," +
                "CodeEmployee='" + comboBoxEmployee.Text + "', CodeMaterial='" + comboBoxProduct.Text + "', SaleOnApplication='"+ comboBoxSaleOnApplication.Text +"' where Code = " + valueId;
                string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
                changeValue(ConnectionString, selectCommand);
                selectCommand = "select * from Application";
                refreshForm(ConnectionString, selectCommand);
            }
            dateTimePicker.Text = "";
            textBoxNumber.Text = "";
            textBoxZav.Text = "";
            textBoxCount.Text = "";
            comboBoxClient.Text = "";
            comboBoxEmployee.Text = "";
            comboBoxProduct.Text = "";
            comboBoxSaleOnApplication.Text = "";
        }
        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int CurrentRow = dataGridView.SelectedCells[0].RowIndex;
            string DateId = dataGridView[1, CurrentRow].Value.ToString();
            string NumberId = dataGridView[2, CurrentRow].Value.ToString();
            string GoodsDeclaredForTheAmountId = dataGridView[3, CurrentRow].Value.ToString();
            string CountId = dataGridView[4, CurrentRow].Value.ToString();
            string CodeClientId = dataGridView[5, CurrentRow].Value.ToString();
            string CodeEmployeeId = dataGridView[6, CurrentRow].Value.ToString();
            string CodeProductId = dataGridView[7, CurrentRow].Value.ToString();
            string CodeOtgruzId = dataGridView[8, CurrentRow].Value.ToString();
            dateTimePicker.Text = DateId;
            textBoxNumber.Text = NumberId;
            textBoxZav.Text = GoodsDeclaredForTheAmountId;
            textBoxCount.Text = CountId;
            comboBoxClient.Text = CodeClientId;
            comboBoxEmployee.Text = CodeEmployeeId;
            comboBoxProduct.Text = CodeProductId;
            comboBoxSaleOnApplication.Text = CodeOtgruzId;
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
        public void selectCombo(string ConnectionString, String selectCommand,
ComboBox comboBox, string displayMember, string valueMember)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            comboBox.DataSource = ds.Tables[0];
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
            connect.Close();
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

        private void ExecuteQuery(string txtQuery)
        {
            connect = new SQLiteConnection("Data Source=" + sPath +
           ";Version=3;New=False;Compress=True;");
            connect.Open();
            cmd = connect.CreateCommand();
            cmd.CommandText = txtQuery;
            cmd.ExecuteNonQuery();
            connect.Close();
        }
        public void refreshForm(string ConnectionString, String selectCommand)
        {
            selectTable(ConnectionString, selectCommand);
            dataGridView.Update();
            dataGridView.Refresh();
            textBoxNumber.Text = "";
            textBoxZav.Text = "";
            textBoxCount.Text = "";
            comboBoxClient.Text = "";
            comboBoxEmployee.Text = "";
            comboBoxProduct.Text = "";
            comboBoxSaleOnApplication.Text = "";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        private void textBoxCount_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxZav_TextChanged(object sender, EventArgs e)
        {
            string tmp = textBoxZav.Text.Trim();
            string outS = string.Empty;
            bool zapyataya = true;
            foreach (char ch in tmp)
                if (Char.IsDigit(ch) || (ch == ',' && zapyataya))
                {
                    outS += ch;
                    if (ch == ',')
                        zapyataya = false;
                }
            textBoxZav.Text = outS;
            textBoxZav.SelectionStart = outS.Length;
        }

        private void textBoxZav_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxNumber_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
