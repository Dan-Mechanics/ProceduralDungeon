using System;
using System.Collections.Generic;

namespace ProceduralDungeon
{
    public class Blackboard 
    {
        public Dictionary<string, object> Dictionary => dictionary;
        public event Action<string> OnLog;

        private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public T GetValue<T>(string key) => dictionary.ContainsKey(key) ? (T)dictionary[key] : default;
        public void SetValue<T>(string key, T value) => dictionary[key] = value;

        public void LogAll()
        {
            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                OnLog?.Invoke($"'{pair.Key}' --> '{pair.Value}'");
            }
        }
    }
}