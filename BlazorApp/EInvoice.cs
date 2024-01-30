namespace BlazorApp
{
    public class EInvoice
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class E_Invoice
        {

            private E_InvoiceHeader headerField;

            private E_InvoiceInvoice invoiceField;

            private E_InvoiceFooter footerField;

            /// <remarks/>
            public E_InvoiceHeader Header
            {
                get
                {
                    return this.headerField;
                }
                set
                {
                    this.headerField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoice Invoice
            {
                get
                {
                    return this.invoiceField;
                }
                set
                {
                    this.invoiceField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceFooter Footer
            {
                get
                {
                    return this.footerField;
                }
                set
                {
                    this.footerField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceHeader
        {

            private System.DateTime dateField;

            private string fileIdField;

            private string appIdField;

            private decimal versionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public System.DateTime Date
            {
                get
                {
                    return this.dateField;
                }
                set
                {
                    this.dateField = value;
                }
            }

            /// <remarks/>
            public string FileId
            {
                get
                {
                    return this.fileIdField;
                }
                set
                {
                    this.fileIdField = value;
                }
            }

            /// <remarks/>
            public string AppId
            {
                get
                {
                    return this.appIdField;
                }
                set
                {
                    this.appIdField = value;
                }
            }

            /// <remarks/>
            public decimal Version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoice
        {

            private E_InvoiceInvoiceInvoiceParties invoicePartiesField;

            private E_InvoiceInvoiceInvoiceInformation invoiceInformationField;

            private E_InvoiceInvoiceInvoiceSumGroup invoiceSumGroupField;

            private E_InvoiceInvoiceInvoiceItem invoiceItemField;

            private E_InvoiceInvoicePaymentInfo paymentInfoField;

            private string channelAddressField;

            private string channelIdField;

            private uint invoiceGlobUniqIdField;

            private uint invoiceIdField;

            private uint regNumberField;

            private uint sellerRegnumberField;

            private uint serviceIdField;

            /// <remarks/>
            public E_InvoiceInvoiceInvoiceParties InvoiceParties
            {
                get
                {
                    return this.invoicePartiesField;
                }
                set
                {
                    this.invoicePartiesField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoiceInformation InvoiceInformation
            {
                get
                {
                    return this.invoiceInformationField;
                }
                set
                {
                    this.invoiceInformationField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoiceSumGroup InvoiceSumGroup
            {
                get
                {
                    return this.invoiceSumGroupField;
                }
                set
                {
                    this.invoiceSumGroupField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoiceItem InvoiceItem
            {
                get
                {
                    return this.invoiceItemField;
                }
                set
                {
                    this.invoiceItemField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoicePaymentInfo PaymentInfo
            {
                get
                {
                    return this.paymentInfoField;
                }
                set
                {
                    this.paymentInfoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string channelAddress
            {
                get
                {
                    return this.channelAddressField;
                }
                set
                {
                    this.channelAddressField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string channelId
            {
                get
                {
                    return this.channelIdField;
                }
                set
                {
                    this.channelIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint invoiceGlobUniqId
            {
                get
                {
                    return this.invoiceGlobUniqIdField;
                }
                set
                {
                    this.invoiceGlobUniqIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint invoiceId
            {
                get
                {
                    return this.invoiceIdField;
                }
                set
                {
                    this.invoiceIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint regNumber
            {
                get
                {
                    return this.regNumberField;
                }
                set
                {
                    this.regNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint sellerRegnumber
            {
                get
                {
                    return this.sellerRegnumberField;
                }
                set
                {
                    this.sellerRegnumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint serviceId
            {
                get
                {
                    return this.serviceIdField;
                }
                set
                {
                    this.serviceIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceParties
        {

            private E_InvoiceInvoiceInvoicePartiesSellerParty sellerPartyField;

            private E_InvoiceInvoiceInvoicePartiesBuyerParty buyerPartyField;

            private E_InvoiceInvoiceInvoicePartiesRecipientParty recipientPartyField;

            private E_InvoiceInvoiceInvoicePartiesDeliveryParty deliveryPartyField;

            private E_InvoiceInvoiceInvoicePartiesPayerParty payerPartyField;

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesSellerParty SellerParty
            {
                get
                {
                    return this.sellerPartyField;
                }
                set
                {
                    this.sellerPartyField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesBuyerParty BuyerParty
            {
                get
                {
                    return this.buyerPartyField;
                }
                set
                {
                    this.buyerPartyField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesRecipientParty RecipientParty
            {
                get
                {
                    return this.recipientPartyField;
                }
                set
                {
                    this.recipientPartyField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesDeliveryParty DeliveryParty
            {
                get
                {
                    return this.deliveryPartyField;
                }
                set
                {
                    this.deliveryPartyField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesPayerParty PayerParty
            {
                get
                {
                    return this.payerPartyField;
                }
                set
                {
                    this.payerPartyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesSellerParty
        {

            private string nameField;

            private uint regNumberField;

            private string vATRegNumberField;

            private E_InvoiceInvoiceInvoicePartiesSellerPartyContactData contactDataField;

            private E_InvoiceInvoiceInvoicePartiesSellerPartyAccountInfo accountInfoField;

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public uint RegNumber
            {
                get
                {
                    return this.regNumberField;
                }
                set
                {
                    this.regNumberField = value;
                }
            }

            /// <remarks/>
            public string VATRegNumber
            {
                get
                {
                    return this.vATRegNumberField;
                }
                set
                {
                    this.vATRegNumberField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesSellerPartyContactData ContactData
            {
                get
                {
                    return this.contactDataField;
                }
                set
                {
                    this.contactDataField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesSellerPartyAccountInfo AccountInfo
            {
                get
                {
                    return this.accountInfoField;
                }
                set
                {
                    this.accountInfoField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesSellerPartyContactData
        {

            private string phoneNumberField;

            private string faxNumberField;

            private string emailAddressField;

            private E_InvoiceInvoiceInvoicePartiesSellerPartyContactDataLegalAddress legalAddressField;

            /// <remarks/>
            public string PhoneNumber
            {
                get
                {
                    return this.phoneNumberField;
                }
                set
                {
                    this.phoneNumberField = value;
                }
            }

            /// <remarks/>
            public string FaxNumber
            {
                get
                {
                    return this.faxNumberField;
                }
                set
                {
                    this.faxNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("E-mailAddress")]
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesSellerPartyContactDataLegalAddress LegalAddress
            {
                get
                {
                    return this.legalAddressField;
                }
                set
                {
                    this.legalAddressField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesSellerPartyContactDataLegalAddress
        {

            private string postalAddress1Field;

            private string postalAddress2Field;

            private string cityField;

            private uint postalCodeField;

            private string countryField;

            /// <remarks/>
            public string PostalAddress1
            {
                get
                {
                    return this.postalAddress1Field;
                }
                set
                {
                    this.postalAddress1Field = value;
                }
            }

            /// <remarks/>
            public string PostalAddress2
            {
                get
                {
                    return this.postalAddress2Field;
                }
                set
                {
                    this.postalAddress2Field = value;
                }
            }

            /// <remarks/>
            public string City
            {
                get
                {
                    return this.cityField;
                }
                set
                {
                    this.cityField = value;
                }
            }

            /// <remarks/>
            public uint PostalCode
            {
                get
                {
                    return this.postalCodeField;
                }
                set
                {
                    this.postalCodeField = value;
                }
            }

            /// <remarks/>
            public string Country
            {
                get
                {
                    return this.countryField;
                }
                set
                {
                    this.countryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesSellerPartyAccountInfo
        {

            private string accountNumberField;

            private string iBANField;

            private string bICField;

            /// <remarks/>
            public string AccountNumber
            {
                get
                {
                    return this.accountNumberField;
                }
                set
                {
                    this.accountNumberField = value;
                }
            }

            /// <remarks/>
            public string IBAN
            {
                get
                {
                    return this.iBANField;
                }
                set
                {
                    this.iBANField = value;
                }
            }

            /// <remarks/>
            public string BIC
            {
                get
                {
                    return this.bICField;
                }
                set
                {
                    this.bICField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesBuyerParty
        {

            private string nameField;

            private uint regNumberField;

            private string vATRegNumberField;

            private E_InvoiceInvoiceInvoicePartiesBuyerPartyContactData contactDataField;

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public uint RegNumber
            {
                get
                {
                    return this.regNumberField;
                }
                set
                {
                    this.regNumberField = value;
                }
            }

            /// <remarks/>
            public string VATRegNumber
            {
                get
                {
                    return this.vATRegNumberField;
                }
                set
                {
                    this.vATRegNumberField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesBuyerPartyContactData ContactData
            {
                get
                {
                    return this.contactDataField;
                }
                set
                {
                    this.contactDataField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesBuyerPartyContactData
        {

            private string phoneNumberField;

            private string emailAddressField;

            private E_InvoiceInvoiceInvoicePartiesBuyerPartyContactDataLegalAddress legalAddressField;

            /// <remarks/>
            public string PhoneNumber
            {
                get
                {
                    return this.phoneNumberField;
                }
                set
                {
                    this.phoneNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("E-mailAddress")]
            public string EmailAddress
            {
                get
                {
                    return this.emailAddressField;
                }
                set
                {
                    this.emailAddressField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesBuyerPartyContactDataLegalAddress LegalAddress
            {
                get
                {
                    return this.legalAddressField;
                }
                set
                {
                    this.legalAddressField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesBuyerPartyContactDataLegalAddress
        {

            private string postalAddress1Field;

            private string postalAddress2Field;

            private string cityField;

            private uint postalCodeField;

            private string countryField;

            /// <remarks/>
            public string PostalAddress1
            {
                get
                {
                    return this.postalAddress1Field;
                }
                set
                {
                    this.postalAddress1Field = value;
                }
            }

            /// <remarks/>
            public string PostalAddress2
            {
                get
                {
                    return this.postalAddress2Field;
                }
                set
                {
                    this.postalAddress2Field = value;
                }
            }

            /// <remarks/>
            public string City
            {
                get
                {
                    return this.cityField;
                }
                set
                {
                    this.cityField = value;
                }
            }

            /// <remarks/>
            public uint PostalCode
            {
                get
                {
                    return this.postalCodeField;
                }
                set
                {
                    this.postalCodeField = value;
                }
            }

            /// <remarks/>
            public string Country
            {
                get
                {
                    return this.countryField;
                }
                set
                {
                    this.countryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesRecipientParty
        {

            private uint uniqueCodeField;

            private string nameField;

            private uint regNumberField;

            /// <remarks/>
            public uint UniqueCode
            {
                get
                {
                    return this.uniqueCodeField;
                }
                set
                {
                    this.uniqueCodeField = value;
                }
            }

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public uint RegNumber
            {
                get
                {
                    return this.regNumberField;
                }
                set
                {
                    this.regNumberField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesDeliveryParty
        {

            private ushort uniqueCodeField;

            private string nameField;

            private E_InvoiceInvoiceInvoicePartiesDeliveryPartyContactData contactDataField;

            /// <remarks/>
            public ushort UniqueCode
            {
                get
                {
                    return this.uniqueCodeField;
                }
                set
                {
                    this.uniqueCodeField = value;
                }
            }

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesDeliveryPartyContactData ContactData
            {
                get
                {
                    return this.contactDataField;
                }
                set
                {
                    this.contactDataField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesDeliveryPartyContactData
        {

            private E_InvoiceInvoiceInvoicePartiesDeliveryPartyContactDataLegalAddress legalAddressField;

            /// <remarks/>
            public E_InvoiceInvoiceInvoicePartiesDeliveryPartyContactDataLegalAddress LegalAddress
            {
                get
                {
                    return this.legalAddressField;
                }
                set
                {
                    this.legalAddressField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesDeliveryPartyContactDataLegalAddress
        {

            private string postalAddress1Field;

            private string cityField;

            private string countryField;

            /// <remarks/>
            public string PostalAddress1
            {
                get
                {
                    return this.postalAddress1Field;
                }
                set
                {
                    this.postalAddress1Field = value;
                }
            }

            /// <remarks/>
            public string City
            {
                get
                {
                    return this.cityField;
                }
                set
                {
                    this.cityField = value;
                }
            }

            /// <remarks/>
            public string Country
            {
                get
                {
                    return this.countryField;
                }
                set
                {
                    this.countryField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoicePartiesPayerParty
        {

            private uint uniqueCodeField;

            private string nameField;

            private uint regNumberField;

            /// <remarks/>
            public uint UniqueCode
            {
                get
                {
                    return this.uniqueCodeField;
                }
                set
                {
                    this.uniqueCodeField = value;
                }
            }

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public uint RegNumber
            {
                get
                {
                    return this.regNumberField;
                }
                set
                {
                    this.regNumberField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceInformation
        {

            private E_InvoiceInvoiceInvoiceInformationType typeField;

            private string documentNameField;

            private byte invoiceNumberField;

            private uint paymentReferenceNumberField;

            private System.DateTime invoiceDateField;

            private System.DateTime dueDateField;

            private decimal fineRatePerDayField;

            private E_InvoiceInvoiceInvoiceInformationInvoiceDeliverer invoiceDelivererField;

            private E_InvoiceInvoiceInvoiceInformationExtension[] extensionField;

            /// <remarks/>
            public E_InvoiceInvoiceInvoiceInformationType Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            public string DocumentName
            {
                get
                {
                    return this.documentNameField;
                }
                set
                {
                    this.documentNameField = value;
                }
            }

            /// <remarks/>
            public byte InvoiceNumber
            {
                get
                {
                    return this.invoiceNumberField;
                }
                set
                {
                    this.invoiceNumberField = value;
                }
            }

            /// <remarks/>
            public uint PaymentReferenceNumber
            {
                get
                {
                    return this.paymentReferenceNumberField;
                }
                set
                {
                    this.paymentReferenceNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public System.DateTime InvoiceDate
            {
                get
                {
                    return this.invoiceDateField;
                }
                set
                {
                    this.invoiceDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public System.DateTime DueDate
            {
                get
                {
                    return this.dueDateField;
                }
                set
                {
                    this.dueDateField = value;
                }
            }

            /// <remarks/>
            public decimal FineRatePerDay
            {
                get
                {
                    return this.fineRatePerDayField;
                }
                set
                {
                    this.fineRatePerDayField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoiceInformationInvoiceDeliverer InvoiceDeliverer
            {
                get
                {
                    return this.invoiceDelivererField;
                }
                set
                {
                    this.invoiceDelivererField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Extension")]
            public E_InvoiceInvoiceInvoiceInformationExtension[] Extension
            {
                get
                {
                    return this.extensionField;
                }
                set
                {
                    this.extensionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceInformationType
        {

            private string typeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceInformationInvoiceDeliverer
        {

            private string contactNameField;

            private uint phoneNumberField;

            /// <remarks/>
            public string ContactName
            {
                get
                {
                    return this.contactNameField;
                }
                set
                {
                    this.contactNameField = value;
                }
            }

            /// <remarks/>
            public uint PhoneNumber
            {
                get
                {
                    return this.phoneNumberField;
                }
                set
                {
                    this.phoneNumberField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceInformationExtension
        {

            private string informationContentField;

            private string extensionIdField;

            /// <remarks/>
            public string InformationContent
            {
                get
                {
                    return this.informationContentField;
                }
                set
                {
                    this.informationContentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string extensionId
            {
                get
                {
                    return this.extensionIdField;
                }
                set
                {
                    this.extensionIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceSumGroup
        {

            private decimal invoiceSumField;

            private E_InvoiceInvoiceInvoiceSumGroupVAT vATField;

            private decimal totalSumField;

            private decimal totalToPayField;

            private string currencyField;

            /// <remarks/>
            public decimal InvoiceSum
            {
                get
                {
                    return this.invoiceSumField;
                }
                set
                {
                    this.invoiceSumField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoiceSumGroupVAT VAT
            {
                get
                {
                    return this.vATField;
                }
                set
                {
                    this.vATField = value;
                }
            }

            /// <remarks/>
            public decimal TotalSum
            {
                get
                {
                    return this.totalSumField;
                }
                set
                {
                    this.totalSumField = value;
                }
            }

            /// <remarks/>
            public decimal TotalToPay
            {
                get
                {
                    return this.totalToPayField;
                }
                set
                {
                    this.totalToPayField = value;
                }
            }

            /// <remarks/>
            public string Currency
            {
                get
                {
                    return this.currencyField;
                }
                set
                {
                    this.currencyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceSumGroupVAT
        {

            private decimal sumBeforeVATField;

            private byte vATRateField;

            private decimal vATSumField;

            private decimal sumAfterVATField;

            private string vatIdField;

            /// <remarks/>
            public decimal SumBeforeVAT
            {
                get
                {
                    return this.sumBeforeVATField;
                }
                set
                {
                    this.sumBeforeVATField = value;
                }
            }

            /// <remarks/>
            public byte VATRate
            {
                get
                {
                    return this.vATRateField;
                }
                set
                {
                    this.vATRateField = value;
                }
            }

            /// <remarks/>
            public decimal VATSum
            {
                get
                {
                    return this.vATSumField;
                }
                set
                {
                    this.vATSumField = value;
                }
            }

            /// <remarks/>
            public decimal SumAfterVAT
            {
                get
                {
                    return this.sumAfterVATField;
                }
                set
                {
                    this.sumAfterVATField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string vatId
            {
                get
                {
                    return this.vatIdField;
                }
                set
                {
                    this.vatIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceItem
        {

            private E_InvoiceInvoiceInvoiceItemItemEntry[] invoiceItemGroupField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("ItemEntry", IsNullable = false)]
            public E_InvoiceInvoiceInvoiceItemItemEntry[] InvoiceItemGroup
            {
                get
                {
                    return this.invoiceItemGroupField;
                }
                set
                {
                    this.invoiceItemGroupField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceItemItemEntry
        {

            private byte rowNoField;

            private string serialNumberField;

            private string sellerProductIdField;

            private string descriptionField;

            private E_InvoiceInvoiceInvoiceItemItemEntryItemDetailInfo itemDetailInfoField;

            private decimal itemSumField;

            private E_InvoiceInvoiceInvoiceItemItemEntryVAT vATField;

            private decimal itemTotalField;

            /// <remarks/>
            public byte RowNo
            {
                get
                {
                    return this.rowNoField;
                }
                set
                {
                    this.rowNoField = value;
                }
            }

            /// <remarks/>
            public string SerialNumber
            {
                get
                {
                    return this.serialNumberField;
                }
                set
                {
                    this.serialNumberField = value;
                }
            }

            /// <remarks/>
            public string SellerProductId
            {
                get
                {
                    return this.sellerProductIdField;
                }
                set
                {
                    this.sellerProductIdField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoiceItemItemEntryItemDetailInfo ItemDetailInfo
            {
                get
                {
                    return this.itemDetailInfoField;
                }
                set
                {
                    this.itemDetailInfoField = value;
                }
            }

            /// <remarks/>
            public decimal ItemSum
            {
                get
                {
                    return this.itemSumField;
                }
                set
                {
                    this.itemSumField = value;
                }
            }

            /// <remarks/>
            public E_InvoiceInvoiceInvoiceItemItemEntryVAT VAT
            {
                get
                {
                    return this.vATField;
                }
                set
                {
                    this.vATField = value;
                }
            }

            /// <remarks/>
            public decimal ItemTotal
            {
                get
                {
                    return this.itemTotalField;
                }
                set
                {
                    this.itemTotalField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceItemItemEntryItemDetailInfo
        {

            private string itemUnitField;

            private decimal itemAmountField;

            private decimal itemPriceField;

            /// <remarks/>
            public string ItemUnit
            {
                get
                {
                    return this.itemUnitField;
                }
                set
                {
                    this.itemUnitField = value;
                }
            }

            /// <remarks/>
            public decimal ItemAmount
            {
                get
                {
                    return this.itemAmountField;
                }
                set
                {
                    this.itemAmountField = value;
                }
            }

            /// <remarks/>
            public decimal ItemPrice
            {
                get
                {
                    return this.itemPriceField;
                }
                set
                {
                    this.itemPriceField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoiceInvoiceItemItemEntryVAT
        {

            private decimal sumBeforeVATField;

            private byte vATRateField;

            private decimal vATSumField;

            private string vatIdField;

            /// <remarks/>
            public decimal SumBeforeVAT
            {
                get
                {
                    return this.sumBeforeVATField;
                }
                set
                {
                    this.sumBeforeVATField = value;
                }
            }

            /// <remarks/>
            public byte VATRate
            {
                get
                {
                    return this.vATRateField;
                }
                set
                {
                    this.vATRateField = value;
                }
            }

            /// <remarks/>
            public decimal VATSum
            {
                get
                {
                    return this.vATSumField;
                }
                set
                {
                    this.vATSumField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string vatId
            {
                get
                {
                    return this.vatIdField;
                }
                set
                {
                    this.vatIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceInvoicePaymentInfo
        {

            private string currencyField;

            private uint paymentRefIdField;

            private string paymentDescriptionField;

            private string payableField;

            private System.DateTime payDueDateField;

            private decimal paymentTotalSumField;

            private string payerNameField;

            private uint paymentIdField;

            private string payToAccountField;

            private string payToNameField;

            /// <remarks/>
            public string Currency
            {
                get
                {
                    return this.currencyField;
                }
                set
                {
                    this.currencyField = value;
                }
            }

            /// <remarks/>
            public uint PaymentRefId
            {
                get
                {
                    return this.paymentRefIdField;
                }
                set
                {
                    this.paymentRefIdField = value;
                }
            }

            /// <remarks/>
            public string PaymentDescription
            {
                get
                {
                    return this.paymentDescriptionField;
                }
                set
                {
                    this.paymentDescriptionField = value;
                }
            }

            /// <remarks/>
            public string Payable
            {
                get
                {
                    return this.payableField;
                }
                set
                {
                    this.payableField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
            public System.DateTime PayDueDate
            {
                get
                {
                    return this.payDueDateField;
                }
                set
                {
                    this.payDueDateField = value;
                }
            }

            /// <remarks/>
            public decimal PaymentTotalSum
            {
                get
                {
                    return this.paymentTotalSumField;
                }
                set
                {
                    this.paymentTotalSumField = value;
                }
            }

            /// <remarks/>
            public string PayerName
            {
                get
                {
                    return this.payerNameField;
                }
                set
                {
                    this.payerNameField = value;
                }
            }

            /// <remarks/>
            public uint PaymentId
            {
                get
                {
                    return this.paymentIdField;
                }
                set
                {
                    this.paymentIdField = value;
                }
            }

            /// <remarks/>
            public string PayToAccount
            {
                get
                {
                    return this.payToAccountField;
                }
                set
                {
                    this.payToAccountField = value;
                }
            }

            /// <remarks/>
            public string PayToName
            {
                get
                {
                    return this.payToNameField;
                }
                set
                {
                    this.payToNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class E_InvoiceFooter
        {

            private byte totalNumberInvoicesField;

            private decimal totalAmountField;

            /// <remarks/>
            public byte TotalNumberInvoices
            {
                get
                {
                    return this.totalNumberInvoicesField;
                }
                set
                {
                    this.totalNumberInvoicesField = value;
                }
            }

            /// <remarks/>
            public decimal TotalAmount
            {
                get
                {
                    return this.totalAmountField;
                }
                set
                {
                    this.totalAmountField = value;
                }
            }
        }


    }
}
