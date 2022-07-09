namespace ArveteSisestajaCore {
    class ProductsInfo {
        private readonly List<Product> _products;
	    private readonly int _sumCalculated;
	    private readonly int _sumOnInvoice;

        public ProductsInfo(List<Product> tooted, int summaArvel, int summaArvutades) {
            this._products = tooted;
            this._sumOnInvoice = summaArvel;
            this._sumCalculated = summaArvutades;
        }

        public List<Product> GetProducts() {
            return _products;
        }

        public int GetSumCalculated() {
            return _sumCalculated;
        }

        public int GetSumInvoice() {
            return _sumOnInvoice;
        }
    }
}
