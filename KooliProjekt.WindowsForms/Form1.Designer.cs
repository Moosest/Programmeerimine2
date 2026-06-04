namespace KooliProjekt.WindowsForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            labelId = new Label();
            textBoxId = new TextBox();
            textBoxName = new TextBox();
            textBoxEmail = new TextBox();
            textBoxPhone = new TextBox();
            textBoxAddress = new TextBox();
            textBoxDiscount = new TextBox();
            labelName = new Label();
            labelEmail = new Label();
            labelPhone = new Label();
            labelAddress = new Label();
            labelDiscount = new Label();
            buttonSave = new Button();
            buttonAdd = new Button();
            buttonDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 128);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(776, 310);
            dataGridView1.TabIndex = 0;
            // 
            // labelId
            // 
            labelId.AutoSize = true;
            labelId.Location = new Point(12, 15);
            labelId.Name = "labelId";
            labelId.Size = new Size(21, 20);
            labelId.TabIndex = 1;
            labelId.Text = "Id";
            // 
            // textBoxId
            // 
            textBoxId.Location = new Point(89, 12);
            textBoxId.Name = "textBoxId";
            textBoxId.ReadOnly = true;
            textBoxId.Size = new Size(72, 27);
            textBoxId.TabIndex = 2;
            textBoxId.Text = "0";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(245, 12);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(162, 27);
            textBoxName.TabIndex = 3;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(245, 45);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(162, 27);
            textBoxEmail.TabIndex = 4;
            // 
            // textBoxPhone
            // 
            textBoxPhone.Location = new Point(526, 12);
            textBoxPhone.Name = "textBoxPhone";
            textBoxPhone.Size = new Size(162, 27);
            textBoxPhone.TabIndex = 5;
            // 
            // textBoxAddress
            // 
            textBoxAddress.Location = new Point(526, 45);
            textBoxAddress.Name = "textBoxAddress";
            textBoxAddress.Size = new Size(162, 27);
            textBoxAddress.TabIndex = 6;
            // 
            // textBoxDiscount
            // 
            textBoxDiscount.Location = new Point(245, 78);
            textBoxDiscount.Name = "textBoxDiscount";
            textBoxDiscount.Size = new Size(162, 27);
            textBoxDiscount.TabIndex = 7;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(179, 15);
            labelName.Name = "labelName";
            labelName.Size = new Size(49, 20);
            labelName.TabIndex = 8;
            labelName.Text = "Name";
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(179, 48);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(46, 20);
            labelEmail.TabIndex = 9;
            labelEmail.Text = "Email";
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(462, 15);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(50, 20);
            labelPhone.TabIndex = 10;
            labelPhone.Text = "Phone";
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Location = new Point(447, 48);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(62, 20);
            labelAddress.TabIndex = 11;
            labelAddress.Text = "Address";
            // 
            // labelDiscount
            // 
            labelDiscount.AutoSize = true;
            labelDiscount.Location = new Point(160, 81);
            labelDiscount.Name = "labelDiscount";
            labelDiscount.Size = new Size(65, 20);
            labelDiscount.TabIndex = 12;
            labelDiscount.Text = "Discount";
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(596, 12);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(192, 25);
            buttonSave.TabIndex = 13;
            buttonSave.Text = "Salvesta";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(596, 43);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(192, 25);
            buttonAdd.TabIndex = 14;
            buttonAdd.Text = "Lisa uus";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(596, 80);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(192, 25);
            buttonDelete.TabIndex = 15;
            buttonDelete.Text = "Kustuta valitud";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBoxId);
            Controls.Add(labelId);
            Controls.Add(buttonSave);
            Controls.Add(buttonDelete);
            Controls.Add(buttonAdd);
            Controls.Add(labelDiscount);
            Controls.Add(labelAddress);
            Controls.Add(labelPhone);
            Controls.Add(labelEmail);
            Controls.Add(labelName);
            Controls.Add(textBoxDiscount);
            Controls.Add(textBoxAddress);
            Controls.Add(textBoxPhone);
            Controls.Add(textBoxEmail);
            Controls.Add(textBoxName);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Clients";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
    private Label labelId;
    private TextBox textBoxId;
        private TextBox textBoxName;
        private TextBox textBoxEmail;
        private TextBox textBoxPhone;
        private TextBox textBoxAddress;
        private TextBox textBoxDiscount;
        private Label labelName;
        private Label labelEmail;
        private Label labelPhone;
        private Label labelAddress;
        private Label labelDiscount;
        private Button buttonSave;
        private Button buttonAdd;
        private Button buttonDelete;
    }
}
