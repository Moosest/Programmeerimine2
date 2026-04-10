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
            // textBoxName
            // 
            textBoxName.Location = new Point(89, 12);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(162, 27);
            textBoxName.TabIndex = 1;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(89, 45);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(162, 27);
            textBoxEmail.TabIndex = 2;
            // 
            // textBoxPhone
            // 
            textBoxPhone.Location = new Point(381, 12);
            textBoxPhone.Name = "textBoxPhone";
            textBoxPhone.Size = new Size(162, 27);
            textBoxPhone.TabIndex = 3;
            // 
            // textBoxAddress
            // 
            textBoxAddress.Location = new Point(381, 45);
            textBoxAddress.Name = "textBoxAddress";
            textBoxAddress.Size = new Size(162, 27);
            textBoxAddress.TabIndex = 4;
            // 
            // textBoxDiscount
            // 
            textBoxDiscount.Location = new Point(89, 78);
            textBoxDiscount.Name = "textBoxDiscount";
            textBoxDiscount.Size = new Size(162, 27);
            textBoxDiscount.TabIndex = 5;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(12, 15);
            labelName.Name = "labelName";
            labelName.Size = new Size(49, 20);
            labelName.TabIndex = 6;
            labelName.Text = "Name";
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(12, 48);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(46, 20);
            labelEmail.TabIndex = 7;
            labelEmail.Text = "Email";
            // 
            // labelPhone
            // 
            labelPhone.AutoSize = true;
            labelPhone.Location = new Point(317, 15);
            labelPhone.Name = "labelPhone";
            labelPhone.Size = new Size(50, 20);
            labelPhone.TabIndex = 8;
            labelPhone.Text = "Phone";
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Location = new Point(302, 48);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(62, 20);
            labelAddress.TabIndex = 9;
            labelAddress.Text = "Address";
            // 
            // labelDiscount
            // 
            labelDiscount.AutoSize = true;
            labelDiscount.Location = new Point(12, 81);
            labelDiscount.Name = "labelDiscount";
            labelDiscount.Size = new Size(65, 20);
            labelDiscount.TabIndex = 10;
            labelDiscount.Text = "Discount";
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(596, 12);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(192, 41);
            buttonAdd.TabIndex = 11;
            buttonAdd.Text = "Lisa uus";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(596, 64);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(192, 41);
            buttonDelete.TabIndex = 12;
            buttonDelete.Text = "Kustuta valitud";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
        private Button buttonAdd;
        private Button buttonDelete;
    }
}
