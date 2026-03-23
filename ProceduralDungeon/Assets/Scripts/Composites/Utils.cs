using System;

namespace ProceduralDungeon
{
    public static class Utils
    {
        public static T StringToEnum<T>(string str) => (T)Enum.Parse(typeof(T), str);
        public static bool IsStringValid(string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
    }
}