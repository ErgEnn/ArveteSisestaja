namespace ArveteSisestajaCore
{
    public static class Util
    {
        public static decimal ToDecimal(string s)
        {
            var (intPart, decPart, unknown) = s.Split(',', '.');
            if (unknown.Any())
                throw new Exception($"Problematic input: '{s}' ");
            if (decPart is null)
                return Convert.ToDecimal(intPart);
            var dec = Convert.ToDecimal(decPart);
            dec /= Convert.ToDecimal(Math.Pow(10, decPart.Length));
            return Convert.ToDecimal(intPart) + dec;
        }
    }
}
