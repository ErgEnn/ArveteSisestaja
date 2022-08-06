namespace ArveteSisestajaCore {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.invoiceDataGrid = new System.Windows.Forms.DataGridView();
            this.toAnc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vendor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.beginDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.loadInvoicesBtn = new System.Windows.Forms.Button();
            this.uploadInvoices = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mainProgressBar = new System.Windows.Forms.ProgressBar();
            this.generatePriaReport = new System.Windows.Forms.Button();
            this.showPricesBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // invoiceDataGrid
            // 
            this.invoiceDataGrid.AllowUserToAddRows = false;
            this.invoiceDataGrid.AllowUserToDeleteRows = false;
            this.invoiceDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.invoiceDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.toAnc,
            this.vendor,
            this.invoiceNumber,
            this.invoiceDate,
            this.invoiceStatus});
            this.invoiceDataGrid.Location = new System.Drawing.Point(12, 69);
            this.invoiceDataGrid.Name = "invoiceDataGrid";
            this.invoiceDataGrid.RowHeadersVisible = false;
            this.invoiceDataGrid.Size = new System.Drawing.Size(669, 434);
            this.invoiceDataGrid.TabIndex = 3;
            this.invoiceDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.invoiceDataGrid_CellValueChanged);
            // 
            // toAnc
            // 
            this.toAnc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.toAnc.FillWeight = 15F;
            this.toAnc.HeaderText = "";
            this.toAnc.Name = "toAnc";
            this.toAnc.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // vendor
            // 
            this.vendor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.vendor.HeaderText = "Firma";
            this.vendor.Name = "vendor";
            this.vendor.ReadOnly = true;
            // 
            // invoiceNumber
            // 
            this.invoiceNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.invoiceNumber.HeaderText = "Arve NR";
            this.invoiceNumber.Name = "invoiceNumber";
            this.invoiceNumber.ReadOnly = true;
            // 
            // invoiceDate
            // 
            this.invoiceDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.invoiceDate.HeaderText = "Kuupäev";
            this.invoiceDate.Name = "invoiceDate";
            this.invoiceDate.ReadOnly = true;
            // 
            // invoiceStatus
            // 
            this.invoiceStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.invoiceStatus.HeaderText = "Staatus";
            this.invoiceStatus.Name = "invoiceStatus";
            this.invoiceStatus.ReadOnly = true;
            // 
            // beginDateTimePicker
            // 
            this.beginDateTimePicker.CustomFormat = "dd.MM.yyyy";
            this.beginDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.beginDateTimePicker.Location = new System.Drawing.Point(52, 17);
            this.beginDateTimePicker.Name = "beginDateTimePicker";
            this.beginDateTimePicker.Size = new System.Drawing.Size(91, 20);
            this.beginDateTimePicker.TabIndex = 4;
            this.beginDateTimePicker.ValueChanged += new System.EventHandler(this.DateTimePicker_ValueChanged);
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.CustomFormat = "dd.MM.yyyy";
            this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDateTimePicker.Location = new System.Drawing.Point(52, 43);
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size(91, 20);
            this.endDateTimePicker.TabIndex = 5;
            this.endDateTimePicker.ValueChanged += new System.EventHandler(this.DateTimePicker_ValueChanged);
            // 
            // loadInvoicesBtn
            // 
            this.loadInvoicesBtn.Location = new System.Drawing.Point(149, 17);
            this.loadInvoicesBtn.Name = "loadInvoicesBtn";
            this.loadInvoicesBtn.Size = new System.Drawing.Size(75, 46);
            this.loadInvoicesBtn.TabIndex = 6;
            this.loadInvoicesBtn.Text = "Lae arved";
            this.loadInvoicesBtn.UseVisualStyleBackColor = true;
            this.loadInvoicesBtn.Click += new System.EventHandler(this.loadInvoicesBtn_Click);
            // 
            // uploadInvoices
            // 
            this.uploadInvoices.Enabled = false;
            this.uploadInvoices.Location = new System.Drawing.Point(12, 509);
            this.uploadInvoices.Name = "uploadInvoices";
            this.uploadInvoices.Size = new System.Drawing.Size(669, 23);
            this.uploadInvoices.TabIndex = 7;
            this.uploadInvoices.Text = "Lae arved ANCsse";
            this.uploadInvoices.UseVisualStyleBackColor = true;
            this.uploadInvoices.Click += new System.EventHandler(this.uploadInvoices_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Algus";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Lõpp";
            // 
            // mainProgressBar
            // 
            this.mainProgressBar.Location = new System.Drawing.Point(231, 17);
            this.mainProgressBar.Name = "mainProgressBar";
            this.mainProgressBar.Size = new System.Drawing.Size(450, 46);
            this.mainProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.mainProgressBar.TabIndex = 10;
            // 
            // generatePriaReport
            // 
            this.generatePriaReport.Location = new System.Drawing.Point(12, 538);
            this.generatePriaReport.Name = "generatePriaReport";
            this.generatePriaReport.Size = new System.Drawing.Size(669, 23);
            this.generatePriaReport.TabIndex = 11;
            this.generatePriaReport.Text = "Genereeri PRIA andmete fail";
            this.generatePriaReport.UseVisualStyleBackColor = true;
            this.generatePriaReport.Click += new System.EventHandler(this.generatePriaReport_Click);
            // 
            // showPricesBtn
            // 
            this.showPricesBtn.Location = new System.Drawing.Point(12, 567);
            this.showPricesBtn.Name = "showPricesBtn";
            this.showPricesBtn.Size = new System.Drawing.Size(669, 23);
            this.showPricesBtn.TabIndex = 12;
            this.showPricesBtn.Text = "Näita keskmisi hindu";
            this.showPricesBtn.UseVisualStyleBackColor = true;
            this.showPricesBtn.Click += new System.EventHandler(this.showPricesBtn_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 598);
            this.Controls.Add(this.showPricesBtn);
            this.Controls.Add(this.generatePriaReport);
            this.Controls.Add(this.mainProgressBar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.uploadInvoices);
            this.Controls.Add(this.loadInvoicesBtn);
            this.Controls.Add(this.endDateTimePicker);
            this.Controls.Add(this.beginDateTimePicker);
            this.Controls.Add(this.invoiceDataGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Arvete sisestaja";
            this.Load += new System.EventHandler(this.mainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.invoiceDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.DataGridView invoiceDataGrid;
		private System.Windows.Forms.DataGridViewCheckBoxColumn toAnc;
		private System.Windows.Forms.DataGridViewTextBoxColumn vendor;
		private System.Windows.Forms.DataGridViewTextBoxColumn invoiceNumber;
		private System.Windows.Forms.DataGridViewTextBoxColumn invoiceDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn invoiceStatus;
		private System.Windows.Forms.DateTimePicker beginDateTimePicker;
		private System.Windows.Forms.DateTimePicker endDateTimePicker;
		private System.Windows.Forms.Button loadInvoicesBtn;
		private System.Windows.Forms.Button uploadInvoices;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ProgressBar mainProgressBar;
		private System.Windows.Forms.Button generatePriaReport;
        private System.Windows.Forms.Button showPricesBtn;
    }
}

