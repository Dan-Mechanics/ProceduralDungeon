using SFB;
using System.IO;
using UnityEngine;

namespace ProceduralDungeon
{
    public class PersistentBlackboard
    {
        private Blackboard blackboard;

        public void Setup(Blackboard blackboard)
        {
            this.blackboard = blackboard;
            blackboard.Clear();
            blackboard.LoadFromString(Resources.Load<TextAsset>("defaults").text);
        }

        public void Save()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter("Text Documents ", "txt") };
            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "dungeon", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            File.WriteAllText(path, blackboard.AsString());
        }

        public void Load()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false);
            if (paths.Length <= 0)
                return;

            string path = paths[0];
            if (!Utils.IsStringValid(path) || !File.Exists(path))
                return;

            blackboard.LoadFromString(File.ReadAllText(path));
        }
    }
}