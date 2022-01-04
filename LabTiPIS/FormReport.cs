using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using Ionic.Zip;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Logic.abstractLogic;

namespace LabTiPIS
{
    public partial class FormReport : Form
    {
        private string itogo = "";
        private LogicBackup _backUpAbstractLogic; //добавить
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private System.Data.DataTable DT = new System.Data.DataTable();
        private string sPath = "C:\\SQLite\\TiPISLabs";
        public FormReport(LogicBackup buckUp)
        {
            InitializeComponent();
            _backUpAbstractLogic = buckUp;
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            comboBoxReport.Items.Add("Ведомость заявок");
            comboBoxReport.Items.Add("Ведомость проданных материалов");
            comboBoxReport.SelectedIndex = -1;
        }

        private void updateTable()
        {
            if (comboBoxReport.SelectedIndex != -1)
            {
                string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
                string dateFrom = dateTimePickerFrom.Value.ToString("yyyy-MM-dd");
                string dateTo = dateTimePickerTo.Value.ToString("yyyy-MM-dd");
                labelSum.Text = "Итого: ";
                itogo = "";
                dataGridView1.Columns.Clear();

                if (comboBoxReport.SelectedIndex == 0)
                {
                    if (dateTimePickerFrom.Value.Date > dateTimePickerTo.Value.Date)
                    {
                        MessageBox.Show("Дата начала периода должна быть меньше даты конца периода");
                        return;
                    }
                    dateTimePickerFrom.Enabled = true;
                    string selectCommand = "Select Date as 'Дата заявки', NumberApplication as 'Номер заявки', GoodsDeclaredForTheAmount as 'Заявлено товаров на сумму', SaleOnApplication as 'Отгружено по заявке' from Application " +
                        "where Application.Date >= '" + dateFrom + "'" +
                        " and Application.Date <= '" + dateTo + "'";
                    selectTable(ConnectionString, selectCommand);
                    int sum = 0;

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[1].Value != DBNull.Value)
                        {
                            sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                        }
                    }
                    itogo += sum;
                    labelSum.Text += itogo;

                }
                if (comboBoxReport.SelectedIndex == 1)
                {
                    if (dateTimePickerFrom.Value.Date > dateTimePickerTo.Value.Date)
                    {
                        MessageBox.Show("Дата начала периода должна быть меньше даты конца периода");
                        return;
                    }
                    dataGridView1.Columns.Clear();
                    dateFrom = dateTimePickerFrom.Value.ToString("yyyy-MM-dd");
                    dateTo = dateTimePickerTo.Value.ToString("yyyy-MM-dd");
                    string selectCommand = "Select CodeMaterial as Материал, Date as 'Дата продажи', CodeApplication as 'По заявке', CountSale as Количество, Sum as 'Продажная стоимость', M.CostPrice as Себестоимость " +
                        "from SaleOfApplication join Material M ON M.Name = SaleOfApplication.CodeMaterial" +
                        " where SaleOfApplication.Date >= '" + dateFrom + "'" +
                        " and SaleOfApplication.Date <= '" + dateTo + "'"; 
                    selectTable(ConnectionString, selectCommand);
                    int sum = 0;

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
                    }
                    itogo += sum + " ";

                    sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[3].Value != DBNull.Value)
                        {
                            sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
                        }
                    }
                    itogo += sum + " ";

                    sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[4].Value != DBNull.Value)
                        {
                            sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
                        }
                    }
                    itogo += sum;
                    labelSum.Text += itogo;


                }
            }
        }




        private void comboBoxReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTable();
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
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = ds.Tables[0].ToString();
            dataGridView1.AutoResizeColumns();
            connect.Close();
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

        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            updateTable();
        }

        private void dateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            updateTable();
        }
        public void savePDF(string FileName)
        {
            string FONT_LOCATION = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.TTF"); //определяем В СИСТЕМЕ(чтобы не копировать файл) расположение шрифта arial.ttf
            BaseFont baseFont = BaseFont.CreateFont(FONT_LOCATION, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED); //создаем шрифт
            iTextSharp.text.Font fontParagraph = new iTextSharp.text.Font(baseFont, 17, iTextSharp.text.Font.NORMAL); //регистрируем + можно задать параметры для него(17 - размер, последний параметр - стиль)
            string title = "";
            if (comboBoxReport.SelectedIndex == 0)
            {
                title = "Ведомость заявок за период" + " с " + Convert.ToString(dateTimePickerFrom.Text) + " по " + Convert.ToString(dateTimePickerTo.Text) + "\n\n";
            }
            if (comboBoxReport.SelectedIndex == 1)
            {
                title = "Ведомость проданных товаров за период" + " с " + Convert.ToString(dateTimePickerFrom.Text) + " по " + Convert.ToString(dateTimePickerTo.Text) + "\n\n";
            }

            var phraseTitle = new Phrase(title,
            new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD));
            iTextSharp.text.Paragraph paragraph = new
           iTextSharp.text.Paragraph(phraseTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };

            PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                table.AddCell(new Phrase(dataGridView1.Columns[i].HeaderCell.Value.ToString(), fontParagraph));
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(dataGridView1.Rows[i].Cells[j].Value.ToString(), fontParagraph));
                }
            }
            if (comboBoxReport.SelectedIndex == 0)
            {
                PdfPTable table2 = new PdfPTable(dataGridView1.Columns.Count);
                String s = labelSum.Text;
                List<string> words = new List<string>();
                string[] sum = {"-", ""};
                words.AddRange(sum);
                String[] words1 = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                words.AddRange(words1);
                for (int j = 0; j < words.Count; j++)
                {
                    table2.AddCell(new Phrase(words[j], fontParagraph));
                }
                using (FileStream stream = new FileStream(FileName, FileMode.Create))
                {
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(paragraph);
                    pdfDoc.Add(table);
                    pdfDoc.Add(table2);
                    pdfDoc.Close();
                    stream.Close();
                }
            }
            if (comboBoxReport.SelectedIndex == 1)
            {
                PdfPTable table2 = new PdfPTable(dataGridView1.Columns.Count);
                String s = labelSum.Text;
                List<string> words = new List<string>();
                string[] sum = { "", "" };
                words.AddRange(sum);
                String[] words1 = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                words.AddRange(words1);
                for (int j = 0; j < words.Count; j++)
                {
                    table2.AddCell(new Phrase(words[j], fontParagraph));
                }
                using (FileStream stream = new FileStream(FileName, FileMode.Create))
                {
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(paragraph);
                    pdfDoc.Add(table);
                    pdfDoc.Add(table2);
                    pdfDoc.Close();
                    stream.Close();
                }
            }
        }
        private void buttonPDF_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания",
               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    savePDF(sfd.FileName);
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }

        public void saveDoc(string FileName)
        {
            var winword = new Microsoft.Office.Interop.Word.Application();
            try
            {
                object missing = System.Reflection.Missing.Value;
                //создаем документ
                Microsoft.Office.Interop.Word.Document document =
                winword.Documents.Add(ref missing, ref missing, ref missing, ref
               missing);
                //получаем ссылку на параграф
                var paragraph = document.Paragraphs.Add(missing);
                var range = paragraph.Range;
                string title = "";
                if (comboBoxReport.SelectedIndex == 0)
                {
                    title = "Ведомость заявок за период" + " с " + Convert.ToString(dateTimePickerFrom.Text) + " по " + Convert.ToString(dateTimePickerTo.Text) + "\n\n";
                }
                if (comboBoxReport.SelectedIndex == 1)
                {
                    title = "Ведомость проданных товаров за период" + " с " + Convert.ToString(dateTimePickerFrom.Text) + " по " + Convert.ToString(dateTimePickerTo.Text) + "\n\n";
                }
                //задаем текст
                range.Text = title;
                //задаем настройки шрифта
                var font = range.Font;
                font.Size = 16;
                font.Name = "Times New Roman";
                font.Bold = 1;
                //задаем настройки абзаца
                var paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 0;
                //добавляем абзац в документ
                range.InsertParagraphAfter();
                //создаем таблицу
                var paragraphTable = document.Paragraphs.Add(Type.Missing);
                var rangeTable = paragraphTable.Range;

                int count = 0;
                for (int i = 0; i < dataGridView1.Columns.Count; ++i)
                {
                    if (dataGridView1.Columns[i].Visible == true)
                    {
                        count++;
                    }
                }
                var table = document.Tables.Add(rangeTable, dataGridView1.Rows.Count + 1, count, ref
        missing, ref missing);
                font = table.Range.Font;
                font.Size = 14;
                font.Name = "Times New Roman";
                var paragraphTableFormat = table.Range.ParagraphFormat;
                paragraphTableFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphTableFormat.SpaceAfter = 0;
                paragraphTableFormat.SpaceBefore = 0;
                count = 0;
                for (int i = 0; i < dataGridView1.Columns.Count; ++i)
                {
                    if (dataGridView1.Columns[i].Visible == true)
                    {
                        table.Cell(1, count + 1).Range.Text = dataGridView1.Columns[i].HeaderCell.Value.ToString();
                        count++;
                    }
                }
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    count = 0;
                    for (int j = 0; j < dataGridView1.Columns.Count; ++j)
                    {
                        if (dataGridView1.Columns[j].Visible == true)
                        {
                            table.Cell(i + 2, count + 1).Range.Text = dataGridView1.Rows[i].Cells[j].Value.ToString();
                            count++;
                        }
                    }
                }

                if (comboBoxReport.SelectedIndex == 0)
                {
                    String s = labelSum.Text;
                    List<string> words = new List<string>();
                    string[] sum = {  "", "" };
                    words.AddRange(sum);
                    String[] words1 = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    words.AddRange(words1);
                    count = 0;
                    for (int j = 0; j < words.Count; j++)
                    {
                        table.Cell(dataGridView1.Rows.Count + 1, count).Range.Text = words[j];
                        count++;
                    }
                    //задаем границы таблицы
                    table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleInset;
                    table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
                    //сохраняем
                    object fileFormat = WdSaveFormat.wdFormatXMLDocument;
                    document.SaveAs(FileName, ref fileFormat, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing);
                    document.Close(ref missing, ref missing, ref missing);
                }
                if (comboBoxReport.SelectedIndex == 1)
                {
                    String s = labelSum.Text;
                    List<string> words = new List<string>();
                    string[] sum = {"",  "", "" };
                    words.AddRange(sum);
                    String[] words1 = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    words.AddRange(words1);
                    count = 0;
                    for (int j = 0; j < words.Count; j++)
                    {
                        table.Cell(dataGridView1.Rows.Count + 1, count).Range.Text = words[j];
                        count++;
                    }
                    //задаем границы таблицы
                    table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleInset;
                    table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
                    //сохраняем
                    object fileFormat = WdSaveFormat.wdFormatXMLDocument;
                    document.SaveAs(FileName, ref fileFormat, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing);
                    document.Close(ref missing, ref missing, ref missing);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                winword.Quit();
            }         
        }
        private void buttonSaveDoc_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания",
               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    saveDoc(sfd.FileName);
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }

        public void saveXls(string FileName)
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                if (File.Exists(FileName))
                {
                    excel.Workbooks.Open(FileName, Type.Missing, Type.Missing,
                   Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                   Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                   Type.Missing,
                    Type.Missing);
                }
                else
                {
                    excel.SheetsInNewWorkbook = 1;
                    excel.Workbooks.Add(Type.Missing);
                    excel.Workbooks[1].SaveAs(FileName, XlFileFormat.xlExcel8,
                    Type.Missing,
                     Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange,
                    Type.Missing,
                     Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                Sheets excelsheets = excel.Workbooks[1].Worksheets;

                var excelworksheet = (Worksheet)excelsheets.get_Item(1);
                excelworksheet.Cells.Clear();
                Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("A1", "H1");
                excelcells.Merge(Type.Missing);
                excelcells.Font.Bold = true;
                string title = "";
                if (comboBoxReport.SelectedIndex == 0)
                {
                    title = "Ведомость заявок за период" + " с " + Convert.ToString(dateTimePickerFrom.Text) + " по " + Convert.ToString(dateTimePickerTo.Text) + "\n\n";
                }
                if (comboBoxReport.SelectedIndex == 1)
                {
                    title = "Ведомость проданных товаров за период" + " с " + Convert.ToString(dateTimePickerFrom.Text) + " по " + Convert.ToString(dateTimePickerTo.Text) + "\n\n";
                }
                excelcells.Value2 = title;
                excelcells.RowHeight = 40;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 14;

                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    excelcells = excelworksheet.get_Range("A3", "A3");
                    excelcells = excelcells.get_Offset(0, j);
                    excelcells.ColumnWidth = 15;
                    excelcells.Value2 = dataGridView1.Columns[j].HeaderCell.Value.ToString();
                    excelcells.Font.Bold = true;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        excelcells = excelworksheet.get_Range("A4", "A4");
                        excelcells = excelcells.get_Offset(i, j);
                        excelcells.ColumnWidth = 15;
                        excelcells.Value2 = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                if(comboBoxReport.SelectedIndex == 0)
                {
                    String s = labelSum.Text;
                    List<string> words = new List<string>();
                    string[] sum = { "" };
                    words.AddRange(sum);
                    String[] words1 = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    words.AddRange(words1);
                    for (int j = 0; j < words.Count; j++)
                    {
                        excelcells = excelworksheet.get_Range("A5", "A5");
                        excelcells = excelcells.get_Offset(dataGridView1.Rows.Count + 1, j);
                        excelcells.ColumnWidth = 25;
                        excelcells.Value2 = words[j].ToString();
                    }
                    excel.Workbooks[1].Save();
                }
                if (comboBoxReport.SelectedIndex == 1)
                {
                    String s = labelSum.Text;
                    List<string> words = new List<string>();
                    string[] sum = { "", "" };
                    words.AddRange(sum);
                    String[] words1 = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    words.AddRange(words1);
                    for (int j = 0; j < words.Count; j++)
                    {
                        excelcells = excelworksheet.get_Range("A5", "A5");
                        excelcells = excelcells.get_Offset(dataGridView1.Rows.Count + 1, j);
                        excelcells.ColumnWidth = 25;
                        excelcells.Value2 = words[j].ToString();
                    }
                    excel.Workbooks[1].Save();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                excel.Quit();
            }
        }
        private void buttonSaveXlsx_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания",
               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    saveXls(sfd.FileName);
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }


        //бэкап
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_backUpAbstractLogic == null)
                {
                    var fbd = new FolderBrowserDialog();
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        _backUpAbstractLogic.CreateArchive(fbd.SelectedPath);
                        MessageBox.Show("Бекап создан", "Сообщение",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                   
                    
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }
    }
}
 