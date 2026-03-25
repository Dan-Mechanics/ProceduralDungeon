using System;
using UnityEngine;

namespace ProceduralDungeon
{
    public static class Utils
    {
        public static T StringToEnum<T>(string str) => (T)Enum.Parse(typeof(T), str);
        public static bool IsStringValid(string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);

        public static bool Has<T>(int x, int y, T[,] grid, int width, int height)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return false;

            return grid[x, y] != null;
        }

        /// <summary>
        /// Enter chance percentage as [0 to 1] inclusive value range.
        /// </summary>
        /// <returns>Random bool.</returns>
        public static bool Roll(float odds)
        {
            if (odds <= 0f)
                return false;

            odds = Mathf.Clamp01(odds);
            return UnityEngine.Random.value <= odds;
        }
    }
}