using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ArveteSisestaja {
	public partial class DefinitionsForm : Form {
		private Product _product;

		public DefinitionsForm(Product product) {
			InitializeComponent();
			this._product = product;
			this.itemNameLabel.Text = product.Name;
			this.amountLbl.Text = product.Amount;
			this.multiplierTextBox.Text = "1";
			this.definitionsListBox.Items.Clear();
			this.definitionsListBox.Items.AddRange(DefinitionsHandler.AncIngredients.Keys.ToArray());
			this.searchBox.Focus();
			this.ActiveControl = searchBox;
		}

		private void submitButton_Click(object sender, EventArgs e) {
			object selectedItem = this.definitionsListBox.SelectedItem;
			if (selectedItem == null) {
				MessageBox.Show("Nimetus on valimata!");
				return;
			}
			this._product.Definition = DefinitionsHandler.AddDefinition(_product, selectedItem.ToString(), int.Parse(this.multiplierTextBox.Text)*(_product.Definition.AncIngredient.IsAltered?1000:1));
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void searchBox_TextChanged(object sender, EventArgs e) {
			this.definitionsListBox.Items.Clear();
			Dictionary<string, Ingredient> anc = DefinitionsHandler.AncIngredients;
			if (searchBox.Text == "") {
				this.definitionsListBox.Items.AddRange(anc.Keys.ToArray());
			} else {
				this.definitionsListBox.Items.AddRange(anc.Keys.Where(s => s.IndexOf(searchBox.Text,StringComparison.OrdinalIgnoreCase)>=0).ToArray());
			}
		}

		private void searchBox_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter&&definitionsListBox.Items.Count>0) {
				definitionsListBox.Focus();
				definitionsListBox.SetSelected(0,true);
			}
		}

		private void definitionsListBox_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				multiplierTextBox.Focus();
			}
			if (e.KeyCode == Keys.Back) {
				searchBox.Focus();
			}
		}

		private void multiplierTextBox_KeyDown(object sender, KeyEventArgs e) {
			if(e.KeyCode == Keys.Enter) {
				submitButton.Focus();
			}
		}

		private void DefinitionsForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.DialogResult!=DialogResult.OK && this.DialogResult!=DialogResult.Ignore) {
				this.DialogResult = DialogResult.Abort;
			}
		}

		private void skipBtn_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Ignore;
			Close();
		}

		private void definitionsListBox_SelectedValueChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(definitionsListBox.Text))
			{
				this.unitLbl.Text = "g";
			}
			this.unitLbl.Text = DefinitionsHandler.AncIngredients[definitionsListBox.Text].IsAltered
				? DefinitionsHandler.AncIngredients[definitionsListBox.Text].Unit
				: "g";
		}
	}
}
