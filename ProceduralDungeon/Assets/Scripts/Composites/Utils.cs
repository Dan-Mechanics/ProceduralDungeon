using System;

namespace ProceduralDungeon
{
    public static class Utils
    {
        public static T StringToEnum<T>(string str)
        {
            return (T)System.Enum.Parse(typeof(T), str);
        }

        public static bool IsStringValid(string str)
        {
            return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        }
    }
}