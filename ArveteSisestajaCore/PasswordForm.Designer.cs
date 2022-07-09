namespace ArveteSisestajaCore {
	partial class PasswordForm {
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.saveButton = new System.Windows.Forms.Button();
			this.ancUsername = new System.Windows.Forms.TextBox();
			this.ancPassword = new System.Windows.Forms.TextBox();
			this.omnivaPassword = new System.Windows.Forms.TextBox();
			this.omnivaUsername = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.ancPassword);
			this.groupBox1.Controls.Add(this.ancUsername);
			this.groupBox1.Location = new System.Drawing.Point(13, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 76);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "ANC";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.omnivaPassword);
			this.groupBox2.Controls.Add(this.omnivaUsername);
			this.groupBox2.Location = new System.Drawing.Point(219, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(200, 77);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Omniva";
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(13, 95);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(406, 23);
			this.saveButton.TabIndex = 2;
			this.saveButton.Text = "Salvesta";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// ancUsername
			// 
			this.ancUsername.Location = new System.Drawing.Point(7, 20);
			this.ancUsername.Name = "ancUsername";
			this.ancUsername.Size = new System.Drawing.Size(187, 20);
			this.ancUsername.TabIndex = 0;
			// 
			// ancPassword
			// 
			this.ancPassword.Location = new System.Drawing.Point(7, 47);
			this.ancPassword.Name = "ancPassword";
			this.ancPassword.PasswordChar = '*';
			this.ancPassword.Size = new System.Drawing.Size(187, 20);
			this.ancPassword.TabIndex = 1;
			this.ancPassword.UseSystemPasswordChar = true;
			// 
			// omnivaPassword
			// 
			this.omnivaPassword.Location = new System.Drawing.Point(6, 48);
			this.omnivaPassword.Name = "omnivaPassword";
			this.omnivaPassword.PasswordChar = '*';
			this.omnivaPassword.Size = new System.Drawing.Size(187, 20);
			this.omnivaPassword.TabIndex = 3;
			this.omnivaPassword.UseSystemPasswordChar = true;
			// 
			// omnivaUsername
			// 
			this.omnivaUsername.Location = new System.Drawing.Point(7, 21);
			this.omnivaUsername.Name = "omnivaUsername";
			this.omnivaUsername.Size = new System.Drawing.Size(187, 20);
			this.omnivaUsername.TabIndex = 2;
			// 
			// PasswordForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(431, 123);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "PasswordForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Kasutajaandmete sisestus";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox ancPassword;
		private System.Windows.Forms.TextBox ancUsername;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox omnivaPassword;
		private System.Windows.Forms.TextBox omnivaUsername;
		private System.Windows.Forms.Button saveButton;
	}
}