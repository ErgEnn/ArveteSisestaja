namespace ArveteSisestajaCore {
	partial class DefinitionsForm {
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
			this.definitionsListBox = new System.Windows.Forms.ListBox();
			this.submitButton = new System.Windows.Forms.Button();
			this.itemNameLabel = new System.Windows.Forms.Label();
			this.multiplierTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.searchBox = new System.Windows.Forms.TextBox();
			this.unitLbl = new System.Windows.Forms.Label();
			this.skipBtn = new System.Windows.Forms.Button();
			this.amountLbl = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// definitionsListBox
			// 
			this.definitionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.definitionsListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.definitionsListBox.FormattingEnabled = true;
			this.definitionsListBox.ItemHeight = 25;
			this.definitionsListBox.Items.AddRange(new object[] {
            "placeHolder1",
            "placeHolder2",
            "placeHolder3",
            "placeHolder4",
            "placeHolder5",
            "placeHolder6",
            "placeHolder7",
            "placeHolder8",
            "placeHolder9"});
			this.definitionsListBox.Location = new System.Drawing.Point(12, 158);
			this.definitionsListBox.Name = "definitionsListBox";
			this.definitionsListBox.Size = new System.Drawing.Size(579, 679);
			this.definitionsListBox.TabIndex = 0;
			this.definitionsListBox.SelectedValueChanged += new System.EventHandler(this.definitionsListBox_SelectedValueChanged);
			this.definitionsListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.definitionsListBox_KeyDown);
			// 
			// submitButton
			// 
			this.submitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.submitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.submitButton.Location = new System.Drawing.Point(244, 853);
			this.submitButton.Name = "submitButton";
			this.submitButton.Size = new System.Drawing.Size(347, 34);
			this.submitButton.TabIndex = 1;
			this.submitButton.Text = "KINNITA";
			this.submitButton.UseVisualStyleBackColor = true;
			this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
			// 
			// itemNameLabel
			// 
			this.itemNameLabel.AutoSize = true;
			this.itemNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.itemNameLabel.Location = new System.Drawing.Point(8, 49);
			this.itemNameLabel.Name = "itemNameLabel";
			this.itemNameLabel.Size = new System.Drawing.Size(553, 24);
			this.itemNameLabel.TabIndex = 2;
			this.itemNameLabel.Text = "placeholderplaceholderplaceolderplaceholderplaceholder";
			// 
			// multiplierTextBox
			// 
			this.multiplierTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.multiplierTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.multiplierTextBox.Location = new System.Drawing.Point(100, 855);
			this.multiplierTextBox.Name = "multiplierTextBox";
			this.multiplierTextBox.Size = new System.Drawing.Size(100, 29);
			this.multiplierTextBox.TabIndex = 3;
			this.multiplierTextBox.Text = "1000";
			this.multiplierTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.multiplierTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.multiplierTextBox_KeyDown);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(13, 858);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 24);
			this.label1.TabIndex = 4;
			this.label1.Text = "Kordaja";
			// 
			// searchBox
			// 
			this.searchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.searchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.searchBox.Location = new System.Drawing.Point(12, 121);
			this.searchBox.Name = "searchBox";
			this.searchBox.Size = new System.Drawing.Size(579, 31);
			this.searchBox.TabIndex = 5;
			this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
			this.searchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchBox_KeyDown);
			// 
			// unitLbl
			// 
			this.unitLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.unitLbl.AutoSize = true;
			this.unitLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.unitLbl.Location = new System.Drawing.Point(206, 858);
			this.unitLbl.Name = "unitLbl";
			this.unitLbl.Size = new System.Drawing.Size(22, 24);
			this.unitLbl.TabIndex = 6;
			this.unitLbl.Text = "g";
			// 
			// skipBtn
			// 
			this.skipBtn.Location = new System.Drawing.Point(516, 13);
			this.skipBtn.Name = "skipBtn";
			this.skipBtn.Size = new System.Drawing.Size(75, 23);
			this.skipBtn.TabIndex = 7;
			this.skipBtn.Text = "Jäta vahele";
			this.skipBtn.UseVisualStyleBackColor = true;
			this.skipBtn.Click += new System.EventHandler(this.skipBtn_Click);
			// 
			// amountLbl
			// 
			this.amountLbl.AutoSize = true;
			this.amountLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.amountLbl.Location = new System.Drawing.Point(8, 84);
			this.amountLbl.Name = "amountLbl";
			this.amountLbl.Size = new System.Drawing.Size(553, 24);
			this.amountLbl.TabIndex = 8;
			this.amountLbl.Text = "placeholderplaceholderplaceolderplaceholderplaceholder";
			// 
			// DefinitionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(603, 899);
			this.Controls.Add(this.amountLbl);
			this.Controls.Add(this.skipBtn);
			this.Controls.Add(this.unitLbl);
			this.Controls.Add(this.searchBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.multiplierTextBox);
			this.Controls.Add(this.itemNameLabel);
			this.Controls.Add(this.submitButton);
			this.Controls.Add(this.definitionsListBox);
			this.Name = "DefinitionsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DefinitionsForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DefinitionsForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox definitionsListBox;
		private System.Windows.Forms.Button submitButton;
		private System.Windows.Forms.Label itemNameLabel;
		private System.Windows.Forms.TextBox multiplierTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox searchBox;
		private System.Windows.Forms.Label unitLbl;
		private System.Windows.Forms.Button skipBtn;
		private System.Windows.Forms.Label amountLbl;
	}
}