using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralDungeon
{
    public class Blackboard 
    {
        // THIS IS REALLY FUNNY:
        private const string STRING = "text";
        private const string INT = "whole";
        private const string FLOAT = "decimal";
        private const string BOOL = "yesno";
        
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
                    string[] tokens = line.Split(',', 4);
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
            type = type.ToLowerInvariant();
            switch (type)
            {
                case FLOAT:
                    SetValue(key, float.Parse(value));
                    break;
                case INT:
                    SetValue(key, int.Parse(value));
                    break;
                case BOOL:
                    SetValue(key, bool.Parse(value));
                    break;
                case STRING:
                    if (!Utils.IsStringValid(value))
                        break;

                    if (value.Length <= 1 || value[0] != '"' || value[1] != '"')
                    {
                        SetValue(key, value);
                        break;
                    }

                    // https://www.w3resource.com/csharp-exercises/basic/csharp-basic-exercise-70.php
                    // REMOVE FIRST + LAST CHARACTERS OF STRING, REMOVE THE ''.
                    SetValue(key, value.Substring(1, value.Length - 2));
                    break;
                default:
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
