using System;

namespace ProceduralDungeon
{
    public static class Utils
    {
        public static T StringToEnum<T>(string str) => (T)Enum.Parse(typeof(T), str);
        public static bool IsStringValid(string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);

        public static bool Has(int x, int y, TileType[,] tiles, int width, int height)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return false;

            return tiles[x, y] != null;
        }
    }
}