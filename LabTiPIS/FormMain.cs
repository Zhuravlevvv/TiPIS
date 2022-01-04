using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabTiPIS
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void планСчетовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChartOfAccounts newFormCl = new FormChartOfAccounts();
            newFormCl.Show();
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClient newFormCl = new FormClient();
            newFormCl.Show();
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEmployee newFormCl = new FormEmployee();
            newFormCl.Show();
        }

        private void материалыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMaterial newFormCl = new FormMaterial();
            newFormCl.Show();
        }

        private void складыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormWarehouse newFormCl = new FormWarehouse();
            newFormCl.Show();
        }

        private void продажаПоЗаявкеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSaleOnApplication newFormCl = new FormSaleOnApplication(0);
            newFormCl.Show();
        }

        private void журналПроводокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPostingJournal newFormCl = new FormPostingJournal();
            newFormCl.Show();
        }

        private void отчётыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReport newFormCl = new FormReport(null);
            newFormCl.Show();
        }

        private void заявкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormApplication newFormCl = new FormApplication();
            newFormCl.Show();
        }
    }
}
