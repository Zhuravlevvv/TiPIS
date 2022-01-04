namespace LabTiPIS
{
    partial class FormApplication
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.labelNumber = new System.Windows.Forms.Label();
            this.labelZav = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.labelsq1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxNumber = new System.Windows.Forms.TextBox();
            this.textBoxZav = new System.Windows.Forms.TextBox();
            this.textBoxCount = new System.Windows.Forms.TextBox();
            this.comboBoxClient = new System.Windows.Forms.ComboBox();
            this.comboBoxEmployee = new System.Windows.Forms.ComboBox();
            this.comboBoxProduct = new System.Windows.Forms.ComboBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonDel = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.Labelzz = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxSaleOnApplication = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeColumns = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(751, 204);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.Location = new System.Drawing.Point(11, 214);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(83, 13);
            this.labelNumber.TabIndex = 2;
            this.labelNumber.Text = "Номер заявки:";
            // 
            // labelZav
            // 
            this.labelZav.AutoSize = true;
            this.labelZav.Location = new System.Drawing.Point(204, 214);
            this.labelZav.Name = "labelZav";
            this.labelZav.Size = new System.Drawing.Size(147, 13);
            this.labelZav.TabIndex = 3;
            this.labelZav.Text = "Заялено товаров на сумму:";
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(11, 240);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(69, 13);
            this.labelCount.TabIndex = 4;
            this.labelCount.Text = "Количество:";
            // 
            // labelsq1
            // 
            this.labelsq1.AutoSize = true;
            this.labelsq1.Location = new System.Drawing.Point(305, 268);
            this.labelsq1.Name = "labelsq1";
            this.labelsq1.Size = new System.Drawing.Size(46, 13);
            this.labelsq1.TabIndex = 5;
            this.labelsq1.Text = "Клиент:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(288, 294);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Сотрудник:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(291, 321);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Материал:";
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Location = new System.Drawing.Point(100, 211);
            this.textBoxNumber.MaxLength = 1000;
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.Size = new System.Drawing.Size(87, 20);
            this.textBoxNumber.TabIndex = 8;
            this.textBoxNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumber_KeyPress);
            // 
            // textBoxZav
            // 
            this.textBoxZav.Location = new System.Drawing.Point(357, 214);
            this.textBoxZav.MaxLength = 1000;
            this.textBoxZav.Name = "textBoxZav";
            this.textBoxZav.Size = new System.Drawing.Size(121, 20);
            this.textBoxZav.TabIndex = 9;
            this.textBoxZav.TextChanged += new System.EventHandler(this.textBoxZav_TextChanged);
            this.textBoxZav.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxZav_KeyPress);
            // 
            // textBoxCount
            // 
            this.textBoxCount.Location = new System.Drawing.Point(86, 237);
            this.textBoxCount.MaxLength = 1000;
            this.textBoxCount.Name = "textBoxCount";
            this.textBoxCount.Size = new System.Drawing.Size(109, 20);
            this.textBoxCount.TabIndex = 10;
            this.textBoxCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCount_KeyPress);
            // 
            // comboBoxClient
            // 
            this.comboBoxClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.comboBoxClient.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxClient.FormattingEnabled = true;
            this.comboBoxClient.Location = new System.Drawing.Point(357, 265);
            this.comboBoxClient.Name = "comboBoxClient";
            this.comboBoxClient.Size = new System.Drawing.Size(121, 21);
            this.comboBoxClient.TabIndex = 12;
            // 
            // comboBoxEmployee
            // 
            this.comboBoxEmployee.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.comboBoxEmployee.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxEmployee.FormattingEnabled = true;
            this.comboBoxEmployee.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBoxEmployee.Location = new System.Drawing.Point(357, 291);
            this.comboBoxEmployee.Name = "comboBoxEmployee";
            this.comboBoxEmployee.Size = new System.Drawing.Size(121, 21);
            this.comboBoxEmployee.TabIndex = 13;
            // 
            // comboBoxProduct
            // 
            this.comboBoxProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.comboBoxProduct.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxProduct.FormattingEnabled = true;
            this.comboBoxProduct.Location = new System.Drawing.Point(357, 318);
            this.comboBoxProduct.Name = "comboBoxProduct";
            this.comboBoxProduct.Size = new System.Drawing.Size(121, 21);
            this.comboBoxProduct.TabIndex = 14;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(496, 211);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(121, 50);
            this.buttonAdd.TabIndex = 15;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(623, 211);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(119, 50);
            this.buttonEdit.TabIndex = 16;
            this.buttonEdit.Text = "Изменить";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDel
            // 
            this.buttonDel.Location = new System.Drawing.Point(496, 273);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(119, 50);
            this.buttonDel.TabIndex = 17;
            this.buttonDel.Text = "Удалить";
            this.buttonDel.UseVisualStyleBackColor = true;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(623, 273);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(119, 50);
            this.buttonCancel.TabIndex = 18;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // Labelzz
            // 
            this.Labelzz.BackColor = System.Drawing.Color.Transparent;
            this.Labelzz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Labelzz.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Labelzz.Font = new System.Drawing.Font("Druk Wide", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Labelzz.ForeColor = System.Drawing.Color.Lime;
            this.Labelzz.Location = new System.Drawing.Point(33, 294);
            this.Labelzz.Name = "Labelzz";
            this.Labelzz.Size = new System.Drawing.Size(154, 40);
            this.Labelzz.TabIndex = 19;
            this.Labelzz.Text = "ЗАЯВКА";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(204, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Дата заявки:";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(285, 237);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(193, 20);
            this.dateTimePicker.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 265);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Отгружено по заявке:";
            // 
            // comboBoxSaleOnApplication
            // 
            this.comboBoxSaleOnApplication.FormattingEnabled = true;
            this.comboBoxSaleOnApplication.Location = new System.Drawing.Point(128, 265);
            this.comboBoxSaleOnApplication.Name = "comboBoxSaleOnApplication";
            this.comboBoxSaleOnApplication.Size = new System.Drawing.Size(151, 21);
            this.comboBoxSaleOnApplication.TabIndex = 23;
            // 
            // FormApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 356);
            this.Controls.Add(this.comboBoxSaleOnApplication);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Labelzz);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDel);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.comboBoxProduct);
            this.Controls.Add(this.comboBoxEmployee);
            this.Controls.Add(this.comboBoxClient);
            this.Controls.Add(this.textBoxCount);
            this.Controls.Add(this.textBoxZav);
            this.Controls.Add(this.textBoxNumber);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelsq1);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.labelZav);
            this.Controls.Add(this.labelNumber);
            this.Controls.Add(this.dataGridView);
            this.Name = "FormApplication";
            this.Text = "Заявка";
            this.Load += new System.EventHandler(this.FormApplication_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.Label labelZav;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label labelsq1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxNumber;
        private System.Windows.Forms.TextBox textBoxZav;
        private System.Windows.Forms.TextBox textBoxCount;
        private System.Windows.Forms.ComboBox comboBoxClient;
        private System.Windows.Forms.ComboBox comboBoxEmployee;
        private System.Windows.Forms.ComboBox comboBoxProduct;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonDel;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label Labelzz;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSaleOnApplication;
    }
}