using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralDungeon
{
    public class Blackboard 
    {
        private const string STRING = "text";
        private const string INT = "whole";
        private const string FLOAT = "decimal";
        private const string BOOL = "yesno";
        private const char QUOTE = '"';
        
        public event Action<string> OnLog;
        private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public T GetValue<T>(string key) => dictionary.ContainsKey(key) ? (T)dictionary[key] : default;
        public void SetValue<T>(string key, T value) => dictionary[key] = value;

        public void LoadFromString(string str)
        {
            string[] lines = str.Split(';', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                line.Trim();
                if (!Utils.IsStringValid(line))
                    continue;

                try
                {
                    string[] tokens = line.Split(',', 3);
                    ParseTokens(tokens[0].Trim(), tokens[1].Trim(), tokens[2].Trim());
                }
                catch (Exception exception)
                {
                    OnLog?.Invoke(exception.Message);
                }
            }
        }

        private void ParseTokens(string type, string key, string value)
        {
            if (!Utils.IsStringValid(type) || !Utils.IsStringValid(key) || !Utils.IsStringValid(value))
                return;

            type = type.ToLowerInvariant();
            switch (type)
            {
                case FLOAT:
                    SetValue(key, float.Parse(value.Replace(',', '.')));
                    break;
                case INT:
                    SetValue(key, int.Parse(value.Replace(",", string.Empty).Replace(".", string.Empty)));
                    break;
                case BOOL:
                    SetValue(key, bool.Parse(value.ToLowerInvariant()));
                    break;
                case STRING:
                    if (value.Length <= 1 || value[0] != QUOTE || value[^1 ] != QUOTE)
                    {
                        SetValue(key, value);
                        break;
                    }

                    // REMOVE FIRST + LAST CHARACTERS OF STRING, REMOVE THE "".
                    SetValue(key, value.Substring(1, value.Length - 2));
                    break;
                default:
                    OnLog?.Invoke($"Blackboard doesn't know '{type}'.");
                    break;
            }
        }

        public void LogAll()
        {
            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                OnLog?.Invoke($"'{pair.Key}'   '{pair.Value}'");
            }
        }

        public string AsString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                string key = pair.Key;
                object value = pair.Value;
                if (value is float f)
                {
                    builder.Append($"{FLOAT},{key},{f}");
                }
                else if (value is int i)
                {
                    builder.Append($"{INT},{key},{i}");
                }
                else if (value is string s)
                {
                    builder.Append($"{STRING},{key},\"{s}\"");
                }
                else if (value is bool b)
                {
                    builder.Append($"{BOOL},{key},{b}");
                }
                else
                {
                    OnLog?.Invoke($"Blackboard doesn't know '{key}'   '{value}'. This is fine.");
                    continue;
                }

                builder.Append(";").AppendLine();
            }

            return builder.ToString();
        }
    }
}
