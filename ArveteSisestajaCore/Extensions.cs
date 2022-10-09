﻿using System.Collections;

namespace ArveteSisestajaCore
{
    public static class Extensions
    {
        public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            rest = list.Skip(1).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            rest = list.Skip(2).ToList();
        }

        public static IEnumerable<DataGridViewRow> OnlySelected(this DataGridViewRowCollection rows)
        {
            foreach (DataGridViewRow row in rows)
            {
                if((bool)row.Cells[0].Value)
                    yield return row;
            }
        }
    }
}
