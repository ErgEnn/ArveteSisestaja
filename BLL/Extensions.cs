namespace BLL
{
    internal static class Extensions
    {
        public static Stream ToStream(this string @this)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@this);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public static IEnumerable<MappedInvoice.InvoiceItem> AllItems(
            this IReadOnlyCollection<MappedInvoice> invoices)
        {
            foreach (var invoice in invoices)
            {
                if (invoice.Items is { } items)
                {
                    foreach (var item in items)
                    {
                        yield return item;
                    }
                }
            }
        }

        public static decimal? Mul(this decimal? d1, decimal d2)
        {
            if (d1 is null) return null;
            return d1 * d2;
        }

        public static decimal? Div(this decimal? d1, decimal d2)
        {
            if (d1 is null) return null;
            return d1 / d2;
        }

        public static T Aquire<T>(this IEnumerator<T> enumerator)
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
    }
}
