using SFB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

namespace ProceduralDungeon
{
    public class PersistentBlackboard
    {
        public event Action<string> OnLog;
        
        private Blackboard blackboard;
        private const string STRING = "string";
        private const string INT = "int";
        private const string FLOAT = "float";
        private const string BOOL = "bool";
        private const char FIRST_SPLITTER = ';';
        private const char SECOND_SPLITTER = ' ';
        private const char QUOTE = '"';

        public void Setup(Blackboard blackboard) => this.blackboard = blackboard;
        public void LoadTextAsset(TextAsset text) => StringToBlackboard(text.text, blackboard);

        public void Save()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter("Text Documents ", "txt") };
            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "dungeon", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            File.WriteAllText(path, BlackboardToString(blackboard));
        }

        public void Load()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false);
            if (paths.Length <= 0)
                return;

            string path = paths[0];
            if (!Utils.IsStringValid(path) || !File.Exists(path))
                return;

            StringToBlackboard(File.ReadAllText(path), blackboard);
        }

        private void StringToBlackboard(string str, Blackboard blackboard)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            if (!Utils.IsStringValid(str))
                return;

            string[] lines = str.Split(FIRST_SPLITTER, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                line.Trim();
                if (!Utils.IsStringValid(line))
                    continue;

                try
                {
                    string[] tokens = line.Split(SECOND_SPLITTER, 3);
                    ParseTokens(tokens[0].Trim(), tokens[1].Trim(), tokens[2].Trim(), blackboard);
                }
                catch (Exception exception)
                {
                    OnLog?.Invoke(exception.Message);
                }
            }
        }

        private void ParseTokens(string type, string key, string value, Blackboard blackboard)
        {
            if (!Utils.IsStringValid(type) || !Utils.IsStringValid(key) || !Utils.IsStringValid(value))
                return;

            type = type.ToLowerInvariant();
            switch (type)
            {
                case FLOAT:
                    value = value.Replace(',', '.');
                    blackboard.SetValue(key, float.Parse(value));
                    break;
                case INT:
                    value = value.Replace(",", string.Empty);
                    value = value.Replace(".", string.Empty);
                    blackboard.SetValue(key, int.Parse(value));
                    break;
                case BOOL:
                    blackboard.SetValue(key, bool.Parse(value.ToLowerInvariant()));
                    break;
                case STRING:
                    if (value.Length <= 1 || value[0] != QUOTE || value[^1] != QUOTE)
                    {
                        blackboard.SetValue(key, value);
                        break;
                    }

                    // REMOVE FIRST + LAST CHARACTERS OF STRING, REMOVE THE "".
                    blackboard.SetValue(key, value.Substring(1, value.Length - 2));
                    break;
                default:
                    OnLog?.Invoke($"Blackboard doesn't know '{type}'   '{value}'. This is fine.");
                    break;
            }
        }

        private string BlackboardToString(Blackboard blackboard)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<string, object> pair in blackboard.Dictionary)
            {
                string key = pair.Key;
                object value = pair.Value;
                if (value is float f)
                {
                    string str = f.ToString();
                    str.Replace(',', '.');
                    if (!str.Contains('.'))
                        str += ".0";

                    builder.Append($"{FLOAT}{SECOND_SPLITTER}{key}{SECOND_SPLITTER}{str}");
                }
                else if (value is int i)
                {
                    builder.Append($"{INT}{SECOND_SPLITTER}{key}{SECOND_SPLITTER}{i}");
                }
                else if (value is string s)
                {
                    s = s.Replace(FIRST_SPLITTER.ToString(), string.Empty);
                    builder.Append($"{STRING}{SECOND_SPLITTER}{key}{SECOND_SPLITTER}\"{s}\"");
                }
                else if (value is bool b)
                {
                    builder.Append($"{BOOL}{SECOND_SPLITTER}{key}{SECOND_SPLITTER}{b}");
                }
                else
                {
                    OnLog?.Invoke($"Blackboard doesn't know '{key}'   '{value}'. This is fine.");
                    continue;
                }

                builder.Append(FIRST_SPLITTER).AppendLine();
            }

            return builder.ToString().Trim();
        }
    }
}