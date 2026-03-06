using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralDungeon
{
    public class Blackboard 
    {
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
                    // THERE IS A REASON COUNT == 4 BUT I CAN'T REMEMBER.
                    string[] tokens = line.Split(',', 4);
                    AddEntry(tokens[0].Trim()[0], tokens[1].Trim(), tokens[2]);
                }
                catch (Exception exception)
                {
                    OnLog?.Invoke(exception.Message);
                }
            }
        }

        public void LogAll()
        {
            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                //OnLog?.Invoke($"'{pair.Key}'_'{pair.Value}'");
                OnLog?.Invoke($"'{pair.Key}'   '{pair.Value}'");
            }
        }

        private void AddEntry(char type, string key, string value)
        {
            type = type.ToString().ToLowerInvariant()[0];
            switch (type)
            {
                case 'f':
                    SetValue(key, float.Parse(value));
                    break;
                case 'i':
                    SetValue(key, int.Parse(value));
                    break;
                case 'b':
                    SetValue(key, bool.Parse(value));
                    break;
                case 's':
                    SetValue(key, value);
                    break;
                default:
                    break;
            }
        }

        public string AsString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                object value = pair.Value;
                string key = pair.Key;
                if (value is float f)
                {
                    builder.Append($"{nameof(f)},{key},{f}");
                }
                else if (value is int i)
                {
                    builder.Append($"{nameof(i)},{key},{i}");
                }
                else if (value is string s)
                {
                    builder.Append($"{nameof(s)},{key},{s}");
                }
                else if (value is bool b)
                {
                    builder.Append($"{nameof(b)},{key},{b}");
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
