namespace ArveteSisestajaCore {
	public partial class DefinitionsForm : Form {
		private Product _product;

		public DefinitionsForm(Product product) {
			InitializeComponent();
			_product = product;
			itemNameLabel.Text = product.Name;
			amountLbl.Text = product.Amount;
			multiplierTextBox.Text = "1000";
			definitionsListBox.Items.Clear();
			definitionsListBox.Items.AddRange(DefinitionsHandler.AncIngredients.Keys.ToArray());
			searchBox.Focus();
			ActiveControl = searchBox;
		}

		private void submitButton_Click(object sender, EventArgs e) {
			object selectedItem = definitionsListBox.SelectedItem;
			if (selectedItem == null) {
				MessageBox.Show("Nimetus on valimata!");
				return;
			}

            var mulitplierStr = multiplierTextBox.Text;
			decimal multiplier;
            if (mulitplierStr.Contains('x'))
            {
                var (multiplierPart1, multiplierPart2, _) = mulitplierStr.Split('x');
                multiplier = Util.ToDecimal(multiplierPart1) * Util.ToDecimal(multiplierPart2);
            }
            else
            {
                multiplier = Util.ToDecimal(mulitplierStr);
            }
            _product.Definition = DefinitionsHandler.AddDefinition(_product, selectedItem.ToString(), multiplier*(DefinitionsHandler.AncIngredients[selectedItem.ToString()].IsAltered?1000:1));
			DialogResult = DialogResult.OK;
			Close();
		}

        private void searchBox_TextChanged(object sender, EventArgs e) {
			definitionsListBox.Items.Clear();
			Dictionary<string, Ingredient> anc = DefinitionsHandler.AncIngredients;
			if (searchBox.Text == "") {
				definitionsListBox.Items.AddRange(anc.Keys.ToArray());
			} else {
				definitionsListBox.Items.AddRange(anc.Keys.Where(s => s.IndexOf(searchBox.Text,StringComparison.OrdinalIgnoreCase)>=0).ToArray());
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
			if (DialogResult!=DialogResult.OK && DialogResult!=DialogResult.Ignore) {
				DialogResult = DialogResult.Abort;
			}
		}

		private void skipBtn_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Ignore;
			Close();
		}

		private void definitionsListBox_SelectedValueChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(definitionsListBox.Text))
			{
				multiplierTextBox.Text = "1000";
				unitLbl.Text = "g";
				return;
			}
			unitLbl.Text = DefinitionsHandler.AncIngredients[definitionsListBox.Text].IsAltered
				? DefinitionsHandler.AncIngredients[definitionsListBox.Text].Unit
				: "g";
			multiplierTextBox.Text = DefinitionsHandler.AncIngredients[definitionsListBox.Text].IsAltered?"1":"1000";
		}
	}
}
