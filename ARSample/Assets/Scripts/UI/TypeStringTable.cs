using System;
using System.Collections.Generic;
using TMPro;

namespace Assets.Scripts.UI
{
    public static class TypeStringTable
    {
        private static Dictionary<Type, string> _table = new Dictionary<Type, string>
        {
            { typeof(TMP_Text), "Text (TMP) - " },
        };

        public static bool IsValid(Type type)
        {
            return _table.ContainsKey(type);
        }

        public static string GetString(Type type)
        {
            return _table[type];
        }
    }
}
