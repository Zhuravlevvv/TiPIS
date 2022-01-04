using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace LabTiPIS
{
    public partial class FormSaleOnApplication : Form
    {
        private SQLiteConnection connect;
        private SQLiteCommand cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath,
        "C:\\SQLite\\TiPISLabs");
        int? ID = null;
        public int IdApplication { set { IdApplication = value; } }


        String selectAllCommand = "Select* from SaleOfApplication";
        public FormSaleOnApplication(int ID)
        {
            this.ID = ID;
            InitializeComponent();
        }

        private void FormSaleOnApplication_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            String selectCommand = "Select * from SaleOfApplication";           
            toolStripTextBoxNameOperation.Text = "Продажа по заявке";
            String selectRequest = "Select Code from Application";
            selectCombo(ConnectionString, selectRequest, toolStripComboBoxNumberOperation, "Code", "Code");
            toolStripComboBoxNumberOperation.SelectedIndex = -1;
            selectTable(ConnectionString, selectCommand);
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
        public void selectTable(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            connect.Close();

            dataGridView.DataSource = ds;
            dataGridView.DataMember = ds.Tables[0].ToString();
            dataGridView.AutoResizeColumns();
            dataGridView.RowHeadersVisible = false;

            connect.Close();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (toolStripComboBoxNumberOperation.SelectedIndex == -1)
                return;

            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";

            string selectClient = "Select CodeClient from Application where Code = " +
                Convert.ToInt32(toolStripComboBoxNumberOperation.ComboBox.SelectedValue);
            string selectCount = "Select Count from Application where Code = " +
               Convert.ToInt32(toolStripComboBoxNumberOperation.ComboBox.SelectedValue);
            string selectGoodsDeclaredForTheAmount = "Select GoodsDeclaredForTheAmount from Application where Code = " +
               Convert.ToInt32(toolStripComboBoxNumberOperation.ComboBox.SelectedValue);
            string selectCodeProduct = "Select CodeMaterial from Application where Code = " +
               Convert.ToInt32(toolStripComboBoxNumberOperation.ComboBox.SelectedValue);
            string selectDate = "Select Date from Application where Code =" +
                Convert.ToInt32(toolStripComboBoxNumberOperation.ComboBox.SelectedValue);

            textBoxClient.Text = Convert.ToString(selectValue(ConnectionString, selectClient));
            textBoxCountSale.Text = Convert.ToString(selectValue(ConnectionString, selectCount));
            textBoxSum.Text = Convert.ToString(selectValue(ConnectionString, selectGoodsDeclaredForTheAmount));
            toolStripComboBoxMaterial.ComboBox.Text = Convert.ToString(selectValue(ConnectionString, selectCodeProduct));
            dateTimePicker.Text = Convert.ToString(selectValue(ConnectionString, selectDate));
        }

        public object selectValue(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteCommand command = new SQLiteCommand(selectCommand,
           connect);
            SQLiteDataReader reader = command.ExecuteReader();
            object value = "";
            while (reader.Read())
            {
                value = reader[0];
            }
            connect.Close();
            return value;

        }

        public void buttonAdd_Click(object sender, EventArgs e)
        {
            string nameoperation = toolStripTextBoxNameOperation.Text = "Продажа по заявке";
            string ConnectionString = @"Data Source=" + sPath +
";New=False;Version=3";
            String selectCommand = "select MAX(IDOperation) from SaleOfApplication";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            if (toolStripComboBoxNumberOperation.SelectedIndex == -1)
            {
                MessageBox.Show("Выберете номер заявки!");
                return;
            }
            if (textBoxCountSale.Text == "")
            {
                MessageBox.Show("Не заполенено количество! Убедитесь, что все поля заполнены и попробуйте снова.");
                return;
            }
            if (textBoxSum.Text == "")
            {
                MessageBox.Show("Не заполнена сумма! Убедитесь, что все поля заполнены и попробуйте снова.");
                return;
            }
            int sum = Convert.ToInt32(textBoxSum.Text);
            if (sum < 0)
            {
                MessageBox.Show("Сумма не может быть отрицательной!!!");
                return;
            }
            if (textBoxClient.Text == "" || toolStripComboBoxMaterial.ComboBox.Text == "")
            {
                MessageBox.Show("Не заполнено поле клиент или продукт! Проверьте заполнение этих полей!"); 
            }
            string selectDate = "select Date from Application where Code = '" +
               Convert.ToInt32(toolStripComboBoxNumberOperation.ComboBox.SelectedValue) + "'";
            DateTime reqDate = Convert.ToDateTime(selectValue(ConnectionString, selectDate));
            if (dateTimePicker.Value < reqDate)
            {
                MessageBox.Show("Дата операции должна быть меньше даты заявки - " + reqDate);
                return;
            }
            else
            {
                string txtSQLQuery = "insert into SaleOfApplication (IDOperation, NameOperation, Date, CountSale, Sum," +
                "CodeApplication, CodeClient, CodeMaterial) values (" +
          (Convert.ToInt32(maxValue) + 1) + ",'" + nameoperation + "','" + dateTimePicker.Value.ToString("yyyy-MM-dd") + "','" + textBoxCountSale.Text + "','" + textBoxSum.Text + "','" + toolStripComboBoxNumberOperation.Text + "','" + textBoxClient.Text + "'," +
          "'" + toolStripComboBoxMaterial.ComboBox.Text + "')";
                selectCommand = "select * from SaleOfApplication";
                ExecuteQuery(txtSQLQuery);      
                //добавление в журнал проводок
                object maxValuePostingJournalID = selectValue(ConnectionString, "select MAX(CodePostingJournal) from PostingJournal");
                if (Convert.ToString(maxValuePostingJournalID) == "")
                    maxValuePostingJournalID = 0;

                // 62 91
               string txtSQLQueryPostingJournal = "insert into PostingJournal (CodePostingJournal, Date, DT, SubcontoDT1, KT, SubcontoKT1, Count, Sum, CodeOperation) values (" +
               (Convert.ToInt32(maxValuePostingJournalID)+1) + ", '" + dateTimePicker.Value.ToString("yyyy-MM-dd") + "', '62.01', '" + textBoxClient.Text +
               "', '91', '" + toolStripTextBoxNameOperation.Text + "', '" + textBoxCountSale.Text + "', '" + textBoxSum.Text + "', '" + toolStripComboBoxNumberOperation.ComboBox.Text + "')";//62  91 субконто - покупатель
               ExecuteQuery(txtSQLQueryPostingJournal);
                object MaterialType = selectValue(ConnectionString, "select Type from Material where Name = '"+  toolStripComboBoxMaterial.ComboBox.Text+"'");
                object KT = selectValue(ConnectionString, "select NumberAccount from ChartOfAccounts where NameAccount =  '" + MaterialType + "'");
                //91 10
                txtSQLQueryPostingJournal = "insert into PostingJournal (CodePostingJournal, Date, DT, SubcontoDT1, KT, SubcontoKT1, Count, Sum, CodeOperation) values (" +
               (Convert.ToInt32(maxValuePostingJournalID)+2) + ", '" + dateTimePicker.Value.ToString("yyyy-MM-dd") + "', '91', '" + toolStripTextBoxNameOperation.Text + "', '" + KT + "', '" + toolStripComboBoxMaterial.ComboBox.Text +
               "','" + textBoxCountSale.Text + "', '" + textBoxSum.Text + "', '" + toolStripComboBoxNumberOperation.ComboBox.Text + "')";// 91  10  
                ExecuteQuery(txtSQLQueryPostingJournal);
                refreshForm(ConnectionString, selectCommand);
            }
        }
        public void refreshForm(string ConnectionString, String selectCommand)
        {
            selectTable(ConnectionString, selectCommand);
            dataGridView.Update();
            dataGridView.Refresh();
            toolStripTextBoxNameOperation.Text = "Покупка по заявке";
            toolStripComboBoxNumberOperation.SelectedIndex = -1;
            dateTimePicker.Text = "";
            textBoxCountSale.Text = "";
            textBoxSum.Text = "";
            textBoxClient.Text = "";
            toolStripComboBoxMaterial.ComboBox.Text = "";
            toolStripComboBoxNumberOperation.Text = "";
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

        private void textBoxSum_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool dotCheck = false;
            char l = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && l != 8) 
            {
                if (dotCheck == false)
                {
                    e.Handled = true;
                }
            }
        }
        private void textBoxCountSale_KeyPress(object sender, KeyPressEventArgs e)
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
        private void textBoxClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l <= 47 || l >= 58) && l != '\b' && l != '.')
            {
                e.Handled = true;
            }
        }
        private void textBoxProduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l <= 47 || l >= 58) && l != '\b' && l != '.')
            {
                e.Handled = true;
            }
        }
        private void toolStripTextBoxNameOperation_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l <= 47 || l >= 58) && l != '\b' && l != '.')
            {
                e.Handled = true;
            }
        }
        private void toolStripComboBoxNumberOperation_KeyPress(object sender, KeyPressEventArgs e)
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
        private void buttonDel_Click(object sender, EventArgs e)
        {
              int CurrentRow = dataGridView.SelectedCells[0].RowIndex;
            string valueId = dataGridView[0, CurrentRow].Value.ToString();
            string ConnectionString = @"Data Source=" + sPath +
    ";New=False;Version=3";
            String selectCommand = "delete from SaleOfApplication where IDOperation =" + valueId;
            changeValue(ConnectionString, selectCommand);
            //удаление проводок
            selectCommand = "delete from PostingJournal where CodeOperation=" + valueId;
            changeValue(ConnectionString, selectCommand);
     
            selectCommand = "select * from SaleOfApplication";
            changeValue(ConnectionString, selectCommand);
            
            toolStripTextBoxNameOperation.Text = "Продажа по заявке";
            toolStripComboBoxNumberOperation.SelectedIndex = -1;
            dateTimePicker.Text = "";
            textBoxCountSale.Text = "";
            textBoxSum.Text = "";
            textBoxClient.Text = "";
            toolStripComboBoxMaterial.ComboBox.Text = "";
            toolStripComboBoxNumberOperation.Text = "";
            refreshForm(ConnectionString, selectCommand);
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int CurrentRow = dataGridView.SelectedCells[0].RowIndex;
            string NameOperationId = dataGridView[1, CurrentRow].Value.ToString();
            string DateId = dataGridView[2, CurrentRow].Value.ToString();
            string CountSaleId = dataGridView[3, CurrentRow].Value.ToString();
            string SumId = dataGridView[4, CurrentRow].Value.ToString();
            string CodeNumbAppId = dataGridView[5, CurrentRow].Value.ToString();
            string CodeClientId = dataGridView[6, CurrentRow].Value.ToString();
            string CodeProductId = dataGridView[7, CurrentRow].Value.ToString();
            toolStripTextBoxNameOperation.Text = NameOperationId;
            dateTimePicker.Text = DateId;
            textBoxCountSale.Text = CountSaleId;
            textBoxSum.Text = SumId;
            textBoxClient.Text = CodeClientId;
            toolStripComboBoxNumberOperation.Text = CodeNumbAppId;
            toolStripComboBoxMaterial.ComboBox.Text = CodeProductId;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            String selectCommand = "Select * from SaleOfApplication";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;

            int CurrentRow = dataGridView.SelectedCells[0].RowIndex;
            string valueId = dataGridView[0, CurrentRow].Value.ToString();
           
            string changeName = toolStripTextBoxNameOperation.Text;
            string selectDate = "select Date from Application where Code = '" +
              Convert.ToInt32(toolStripComboBoxNumberOperation.ComboBox.SelectedValue) + "'";
            DateTime reqDate = Convert.ToDateTime(selectValue(ConnectionString, selectDate));
            if (dateTimePicker.Value < reqDate)
            {
                MessageBox.Show("Дата операции должна быть меньше даты заявки - " + reqDate);
                return;
            }
            if (textBoxCountSale.Text == "")
            {
                MessageBox.Show("Не заполенено количество! Убедитесь, что все поля заполнены и попробуйте снова.");
                return;
            }
            if (textBoxSum.Text == "")
            {
                MessageBox.Show("Не заполнена сумма! Убедитесь, что все поля заполнены и попробуйте снова.");
                return;
            }
            int sum = Convert.ToInt32(textBoxSum.Text);
            if (sum < 0)
            {
                MessageBox.Show("Сумма не может быть отрицательной!!!");
                return;
            }
            if (textBoxClient.Text == "" || toolStripComboBoxMaterial.ComboBox.Text == "")
            {
                MessageBox.Show("Не заполнено поле клиент или продукт или номер заявки! Проверьте заполнение этих полей!");
            }

            else
            {
                
                String selectName = "update SaleOfApplication set NameOperation='" + changeName + "'where IDOperation = " + valueId;
                changeValue(ConnectionString, selectName);

                string changeDate = dateTimePicker.Value.ToString("yyyy-MM-dd");
                String selectDateOp = "update SaleOfApplication set Date='" + changeDate + "'where IDOperation = " + valueId;
                changeValue(ConnectionString, selectDateOp);

                string changeCount = textBoxCountSale.Text;
                String selectCountBuy = "update SaleOfApplication set CountSale='" + changeCount + "'where IDOperation = " + valueId;
                changeValue(ConnectionString, selectCountBuy);

                string changeSum = textBoxSum.Text;
                String selectSum = "update SaleOfApplication set Sum='" + changeSum + "'where IDOperation = " + valueId;
                changeValue(ConnectionString, selectSum);

                string changeIdClient = textBoxClient.Text;
                String selectIdClient = "update SaleOfApplication set CodeClient='" + changeIdClient + "'where IDOperation = " + valueId;
                changeValue(ConnectionString, selectIdClient);

                string changeIdNumbApp = toolStripComboBoxNumberOperation.Text;
                String selectIdNumbApp = "update SaleOfApplication set CodeApplication='" + changeIdNumbApp + "'where IDOperation = " + valueId;
                changeValue(ConnectionString, selectIdNumbApp);

                string changeIdProduct = toolStripComboBoxMaterial.ComboBox.Text;
                String selectIdProduct = "update SaleOfApplication set CodeMaterial='" + changeIdProduct + "'where IDOperation = " + valueId;
                changeValue(ConnectionString, selectIdProduct);

              
                //изменение проводок
                object minValuePostingJournalID = selectValue(ConnectionString, "select MIN(CodePostingJournal) from PostingJournal where CodeOperation = " + maxValue);
                // 62 91
                string txtSQLQueryPostingJournal = "update PostingJournal set Date = '" + dateTimePicker.Value.ToString("yyyy-MM-dd") + "',SubcontoDT1 = '" + textBoxClient.Text + "', SubcontoKT1= '" + toolStripTextBoxNameOperation.Text + "',Count = '" + textBoxCountSale.Text + "', Sum = '" +
                    textBoxSum.Text + "', CodeOperation='"+ toolStripComboBoxNumberOperation.ComboBox.Text + "' where CodePostingJournal = " + (Convert.ToInt32(minValuePostingJournalID));
                ExecuteQuery(txtSQLQueryPostingJournal);
                //определение типа материала
                object MaterialType = selectValue(ConnectionString, "select Type from Material where Name = '" + toolStripComboBoxMaterial.ComboBox.Text + "'");
                object KT = selectValue(ConnectionString, "select NumberAccount from ChartOfAccounts where NameAccount =  '" + MaterialType + "'");
                // 91 10.тип материала
                txtSQLQueryPostingJournal = "update PostingJournal set Date = '" + dateTimePicker.Value.ToString("yyyy-MM-dd") + "',SubcontoDT1 = '" + toolStripTextBoxNameOperation.Text + "',KT = '" + KT + "', SubcontoKT1 = '" + toolStripComboBoxMaterial.ComboBox.Text + "', " +
                    "Count = '" + textBoxCountSale.Text + "', Sum = '" + textBoxSum.Text + "', CodeOperation='"+ toolStripComboBoxNumberOperation.ComboBox.Text + "' where CodePostingJournal = " + (Convert.ToInt32(minValuePostingJournalID) + 1);
                ExecuteQuery(txtSQLQueryPostingJournal);


                refreshForm(ConnectionString, selectCommand);
            }           
        }

        private void buttonCheckJournal_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedCells[0].RowIndex >= 0)
            { 
                //выбрана строка CurrentRow
                int CurrentRow = dataGridView.SelectedCells[0].RowIndex;
                //получить значение Name выбранной строки
                int valueId = Convert.ToInt32(dataGridView[0, CurrentRow].Value.ToString());
                FormPostingJournal newFormApp = new FormPostingJournal();
                newFormApp.IdJO = valueId;
                newFormApp.Show();
                string ConnectionString = @"Data Source=" + sPath +
              ";New=False;Version=3";
                selectTable(ConnectionString, selectAllCommand);
            }
        }
    }
}
